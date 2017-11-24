using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    public event Action<NetworkPlayer> syncVarsChanged;
    // Server only event
    public event Action<NetworkPlayer> becameReady;

    [SerializeField]
    protected GameObject m_TankPrefab;
    [SerializeField]
    protected GameObject m_LobbyPrefab;

    [ClientRpc]
    public void RpcPrepareForLoad()
    {
        if (isLocalPlayer)
        {
            // Show loading screen
            LoadingModal loading = LoadingModal.s_Instance;

            if (loading != null)
            {
                loading.FadeIn();
            }
        }
    }
}
