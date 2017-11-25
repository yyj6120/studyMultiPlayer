using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CreateGame : MonoBehaviour
{
    [SerializeField]
    //Internal reference to the InputField used to enter the server name.
    protected InputField matchNameInput;
    private MainMenuUI menuUi;
    private NetworkManager netManager;

    //[SerializeField]
    ////Internal reference to the MapSelect instance used to flip through multiplayer maps.
    //protected MapSelect m_MapSelect;

    //[SerializeField]
    ////Internal reference to the ModeSelect instance used to cycle multiplayer modes.
    //protected ModeSelect m_ModeSelect;

    //Cached references to other UI singletons.

    protected virtual void Start()
    {
        menuUi = MainMenuUI.Instance;
        netManager = NetworkManager.instance;
    }

    public void OnBackClicked()
    {
        menuUi.ShowDefaultPanel();
    }

    /// <summary>
    /// Create button method. Validates entered server name and launches game server.
    /// </summary>
    public void OnCreateClicked()
    {
        if (string.IsNullOrEmpty(matchNameInput.text))
        {
            menuUi.ShowInfoPopup("Server name cannot be empty!", null);
            return;
        }

        StartMatchmakingGame();
    }

    /// <summary>
    /// Populates game settings for broadcast to clients and attempts to start matchmaking server session.
    /// </summary>
    private void StartMatchmakingGame()
    {
        //GameSettings settings = GameSettings.s_Instance;
        //settings.SetMapIndex(m_MapSelect.currentIndex);
        //settings.SetModeIndex(m_ModeSelect.currentIndex);

        menuUi.ShowConnectingModal(false);

        netManager.StartMatchmakingGame(GetGameName(), (success, matchInfo) =>
        {
            if (!success)
            {
                menuUi.ShowInfoPopup("Failed to create game.", null);
            }
            else
            {
                menuUi.HideInfoPopup();
                menuUi.ShowLobbyPanel();
            }
        });
    }

  //  Returns a formatted string containing server name and game mode information.
    private string GetGameName()
    {
        return string.Format("{0}", matchNameInput.text);
    }
}