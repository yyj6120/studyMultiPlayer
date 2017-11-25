using UnityEngine;
using UnityEngine.UI;

public class LobbyInfoPanel : MonoBehaviour
{
    [SerializeField]
    protected Text infoText;
    [SerializeField]
    protected Text buttonText;
    [SerializeField]
    protected Button cancelButton;

    public void Display(string info, UnityEngine.Events.UnityAction buttonClbk, bool displayButton = true)
    {
        infoText.text = info;

        cancelButton.gameObject.SetActive(displayButton);
        cancelButton.onClick.RemoveAllListeners();
        if (buttonClbk != null)
        {
            cancelButton.onClick.AddListener(buttonClbk);
        }

        cancelButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        gameObject.SetActive(true);
    }
}
