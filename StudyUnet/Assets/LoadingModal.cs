using UnityEngine;
[RequireComponent(typeof(FadingGroup))]
public class LoadingModal : Modal
{
    private FadingGroup fader;

    [SerializeField]
    protected float fadeTime = 0.5f;

    public static LoadingModal instance
    {
        get;
        private set;
    }

    public bool readyToTransition
    {
        get
        {
            return fader.currentFade == Fade.None && gameObject.activeSelf;
        }
    }

    public FadingGroup Fader
    {
        get
        {
            return fader;
        }
    }

    public void FadeIn()
    {
        Show();
        fader.StartFade(Fade.In, fadeTime);
    }

    public void FadeOut()
    {
        Show();
        fader.StartFade(Fade.Out, fadeTime, CloseModal);
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.Log("<color=lightblue>Trying to create a second instance of LoadingModal</color");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        fader = GetComponent<FadingGroup>();
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}