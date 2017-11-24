using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerList : MonoBehaviour
{
    public static LobbyPlayerList instance = null;
    private NetworkManager netManager;
    protected virtual void Awake()
    {
        instance = this;
    }

    protected virtual void Start()
    {
        netManager = NetworkManager.instance;
        if (netManager != null)
        {
            netManager.playerJoined += PlayerJoined;
            netManager.playerLeft += PlayerLeft;
            netManager.serverPlayersReadied += PlayersReadied;
        }
    }

    protected virtual void PlayerJoined(TanksNetworkPlayer player)
    {
        Debug.LogFormat("Player joined {0}", player.name);
    }

    protected virtual void PlayerLeft(TanksNetworkPlayer player)
    {
        Debug.LogFormat("Player left {0}", player.name);
    }

    protected virtual void PlayersReadied()
    {
        netManager.ProgressToGameScene();
    }
}
