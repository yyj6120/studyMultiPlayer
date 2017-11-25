using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public enum MenuPage
{
    Home,
    SinglePlayer,
    Lobby,
    CustomizationPage
}

public class MainMenuUI : Singleton<MainMenuUI>
{
    #region Static config
    public static MenuPage returnPage;
    #endregion

    [SerializeField]
    protected CanvasGroup defaultPanel;
    [SerializeField]
    protected CanvasGroup createGamePanel;
    [SerializeField]
    protected CanvasGroup lobbyPanel;

    [SerializeField]
    protected LobbyInfoPanel lobbyInfoPanel;

    private CanvasGroup currentPanel;

    private Action waitTask;
    private bool readyToFireTask;

    public void OnCreateGameClicked()
    {
        DoIfNetworkReady(GoToCreateGamePanel);
    }

    public void DoIfNetworkReady(Action task)
    {
        if (task == null)
        {
            throw new ArgumentNullException("task");
        }

        NetworkManager netManager = NetworkManager.instance;

        if (netManager.isNetworkActive)
        {
            waitTask = task;

            LoadingModal modal = LoadingModal.instance;
            if (modal != null)
            {
                modal.FadeIn();
            }

            readyToFireTask = false;
            netManager.clientStopped += OnClientStopped;
        }
        else
        {
            task();
        }
    }

    protected void Start()
    {
        LoadingModal modal = LoadingModal.instance;

        if (modal != null)
        {
            modal.FadeOut();
        }

        //Used to return to correct page on return to menu
        switch (returnPage)
        {
            case MenuPage.Home:
            default:
                ShowDefaultPanel();
                break;
        }
    }

    public void ShowPanel(CanvasGroup newPanel)
    {
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }

        currentPanel = newPanel;
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(true);
        }
    }

    public void ShowDefaultPanel()
    {
        ShowPanel(defaultPanel);
    }

    private void GoToCreateGamePanel()
    {
        ShowPanel(createGamePanel);
    }

    public void ShowLobbyPanel()
    {
        ShowPanel(lobbyPanel);
    }

    public void ShowConnectingModal(bool reconnectMatchmakingClient)
    {
        ShowInfoPopup("Connecting...", () =>
        {
            if (NetworkManager.instanceExists)
            {
                if (reconnectMatchmakingClient)
                {
                    NetworkManager.instance.Disconnect();
                    NetworkManager.instance.StartMatchingmakingClient();
                }
                else
                {
                    NetworkManager.instance.Disconnect();
                }
            }
        });
    }

    private void OnClientStopped()
    {
        NetworkManager netManager = NetworkManager.instance;
        netManager.clientStopped -= OnClientStopped;
        readyToFireTask = true;
    }

    public void ShowInfoPopup(string label, UnityAction callback)
    {
        if (lobbyInfoPanel != null)
        {
            lobbyInfoPanel.Display(label, callback, true);
        }
    }

    public void ShowInfoPopup(string label)
    {
        if (lobbyInfoPanel != null)
        {
            lobbyInfoPanel.Display(label, null, false);
        }
    }

    public void HideInfoPopup()
    {
        if (lobbyInfoPanel != null)
        {
            lobbyInfoPanel.gameObject.SetActive(false);
        }
    }

}
