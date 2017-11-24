using System;
using UnityEngine;
using UnityEngine.Events;

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
    protected CanvasGroup createGamePanel;
}
