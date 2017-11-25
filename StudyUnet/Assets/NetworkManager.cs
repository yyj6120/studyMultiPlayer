using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    public enum SceneChangeMode
    {
        None,
        Game,
        Menu
    }

    public enum NetworkState
    {
        Inactive,
        Pregame,
        Connecting,
        InLobby,
        InGame
    }

    public enum NetworkGameType
    {
        Matchmaking,
        Direct,
        Singleplayer
    }

    public NetworkGameType gameType
    {
        get;
        protected set;
    }

    #region Constants

    private static readonly string lobbySceneName = "LobbyScene";

    #endregion

    public event Action<NetworkPlayer> playerJoined;
    public event Action<NetworkPlayer> playerLeft;

    public event Action<NetworkPlayer> syncVarsChanged;

    public event Action serverPlayersReadied;

    public event Action clientStopped;

    private Action<bool, MatchInfo> nextMatchJoinedCallback;

    #region Singleton
    public static NetworkManager instance
    {
        get;
        protected set;
    }

    public static bool instanceExists
    {
        get
        {
            return instance != null;
        }
    }
    #endregion

    private SceneChangeMode sceneChangeMode;

    public List<NetworkPlayer> connectedPlayers
    {
        get;
        private set;
    }

    public NetworkState state
    {
        get;
        protected set;
    }

    public static bool IsServer
    {
        get
        {
            return NetworkServer.active;
        }
    }

    [SerializeField]
    protected int multiplayerMaxPlayers = 4;

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            connectedPlayers = new List<NetworkPlayer>();
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ProgressToGameScene()
    {
        // Clear all client's ready states
        ClearAllReadyStates();

        // Remove us from matchmaking lists
        UnlistMatch();

        // Update will change scenes once loading screen is visible
        sceneChangeMode = SceneChangeMode.Game;

        // Tell NetworkPlayers to show their loading screens
        for (int i = 0; i < connectedPlayers.Count; ++i)
        {
            NetworkPlayer player = connectedPlayers[i];
            if (player != null)
            {
                player.RpcPrepareForLoad();
            }
        }
    }

    public void ClearAllReadyStates()
    {
        for (int i = 0; i < connectedPlayers.Count; ++i)
        {
            NetworkPlayer player = connectedPlayers[i];
            if (player != null)
            {
                player.ClearReady();
            }
        }
    }

    protected void UnlistMatch()
    {
        if (gameType == NetworkGameType.Matchmaking && matchMaker != null)
        {
            matchMaker.SetMatchAttributes(matchInfo.networkId, false, 0, (success, info) => Debug.Log("Match hidden"));
        }
    }

    #region Methods
    public void Disconnect()
    {
        switch (gameType)
        {
            case NetworkGameType.Direct:
                StopDirectMultiplayerGame();
                break;
            case NetworkGameType.Matchmaking:
                StopMatchmakingGame();
                break;
            case NetworkGameType.Singleplayer:
                StopSingleplayerGame();
                break;
        }
    }

    protected void StopDirectMultiplayerGame()
    {
        switch (state)
        {
            case NetworkState.Connecting:
            case NetworkState.InLobby:
            case NetworkState.InGame:
                if (IsServer)
                {
                    StopHost();
                }
                else
                {
                    StopClient();
                }
                break;
        }

        state = NetworkState.Inactive;
    }

    protected void StopMatchmakingGame()
    {
        switch (state)
        {
            case NetworkState.Pregame:
                if (IsServer)
                {
                    Debug.LogError("Server should never be in this state.");
                }
                else
                {
                    StopMatchMaker();
                }
                break;

            case NetworkState.Connecting:
                if (IsServer)
                {
                    StopMatchMaker();
                    StopHost();
                    matchInfo = null;
                }
                else
                {
                    StopMatchMaker();
                    StopClient();
                    matchInfo = null;
                }
                break;

            case NetworkState.InLobby:
            case NetworkState.InGame:
                if (IsServer)
                {
                    if (matchMaker != null && matchInfo != null)
                    {
                        matchMaker.DestroyMatch(matchInfo.networkId, 0, (success, info) =>
                        {
                            if (!success)
                            {
                                Debug.LogErrorFormat("Failed to terminate matchmaking game. {0}", info);
                            }
                            StopMatchMaker();
                            StopHost();

                            matchInfo = null;
                        });
                    }
                    else
                    {
                        Debug.LogWarning("No matchmaker or matchInfo despite being a server in matchmaking state.");

                        StopMatchMaker();
                        StopHost();
                        matchInfo = null;
                    }
                }
                else
                {
                    if (matchMaker != null && matchInfo != null)
                    {
                        matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, (success, info) =>
                        {
                            if (!success)
                            {
                                Debug.LogErrorFormat("Failed to disconnect from matchmaking game. {0}", info);
                            }
                            StopMatchMaker();
                            StopClient();
                            matchInfo = null;
                        });
                    }
                    else
                    {
                        Debug.LogWarning("No matchmaker or matchInfo despite being a client in matchmaking state.");

                        StopMatchMaker();
                        StopClient();
                        matchInfo = null;
                    }
                }
                break;
        }

        state = NetworkState.Inactive;
    }

    protected void StopSingleplayerGame()
    {
        switch (state)
        {
            case NetworkState.InLobby:
                Debug.LogWarning("Single player game in lobby state. This should never happen");
                break;
            case NetworkState.Connecting:
            case NetworkState.Pregame:
            case NetworkState.InGame:
                StopHost();
                break;
        }

        state = NetworkState.Inactive;
    }

    public void StartMatchmakingGame(string gameName, Action<bool, MatchInfo> onCreate)
    {
        if (state != NetworkState.Inactive)
        {
            throw new InvalidOperationException("Network currently active. Disconnect first.");
        }

        state = NetworkState.Connecting;
        gameType = NetworkGameType.Matchmaking;

        StartMatchMaker();
        nextMatchJoinedCallback = onCreate;

        matchMaker.CreateMatch(gameName, (uint)multiplayerMaxPlayers, true, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchCreate);
    }

    public void StartMatchingmakingClient()
    {
        if (state != NetworkState.Inactive)
        {
            throw new InvalidOperationException("Network currently active. Disconnect first.");
        }
        // minPlayers = 2;
        // maxPlayers = multiplayerMaxPlayers;
        state = NetworkState.Pregame;
        gameType = NetworkGameType.Matchmaking;
        StartMatchMaker();
    }
    #endregion
}

