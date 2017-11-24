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

    #region Constants

    private static readonly string lobbySceneName = "LobbyScene";

    #endregion

    public event Action<NetworkPlayer> playerJoined;
    public event Action<NetworkPlayer> playerLeft;

    public event Action serverPlayersReadied;

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
}
