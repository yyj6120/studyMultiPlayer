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

    [SyncVar(hook = "OnReadyChanged")]
    private bool ready = false;

    public bool Ready
    {
        get
        {
            return ready;
        }
    }

    [ClientRpc]
    public void RpcPrepareForLoad()
    {
        if (isLocalPlayer)
        {
            // Show loading screen
            LoadingModal loading = LoadingModal.instance;

            if (loading != null)
            {
                loading.FadeIn();
            }
        }
    }

    private void OnReadyChanged(bool value)
    {
        ready = value;

        if (syncVarsChanged != null)
        {
            syncVarsChanged(this);
        }
    }

    [Server]
    public void ClearReady()
    {
        ready = false;
    }
}
