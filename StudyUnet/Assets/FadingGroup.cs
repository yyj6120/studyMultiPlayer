﻿using UnityEngine;
using System;
public enum Fade
{
    None,
    In,
    Out
}
[RequireComponent(typeof(CanvasGroup))]
public class FadingGroup : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private Fade fade = Fade.None;

    private float fadeTime = 0f, fadeOutValue = 0f;

    private Action finishFade;

    public Fade currentFade
    {
        get
        {
            return fade;
        }
    }

    private float fadeStep
    {
        get
        {
            if (fadeTime == 0f)
            {
                return 1f;
            }

            return Time.unscaledDeltaTime / fadeTime;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (fade == Fade.Out)
        {
            FadeOut();
        }
        else if (fade == Fade.In)
        {
            FadeIn();
        }
    }

    private void FadeOut()
    {
        canvasGroup.alpha -= fadeStep;
        if (canvasGroup.alpha <= fadeOutValue + Mathf.Epsilon)
        {
            canvasGroup.alpha = fadeOutValue;
            if (fadeOutValue == 0)
            {
                gameObject.SetActive(false);
            }

            EndFade();
        }
    }

    private void FadeIn()
    {
        canvasGroup.alpha += fadeStep;
        if (canvasGroup.alpha >= 1 - Mathf.Epsilon)
        {
            canvasGroup.alpha = 1;
            EndFade();
        }
    }

    private void EndFade()
    {
        fade = Fade.None;
        FireEvent();
    }

    private void FireEvent()
    {
        if (finishFade != null)
        {
            finishFade.Invoke();
        }
    }

    public void StartFade(Fade fade, float fadeTime, Action finishFade = null, bool reactivate = true)
    {
        this.fade = fade;
        this.fadeTime = fadeTime;
        this.finishFade = finishFade;
        this.fadeOutValue = 0f;
        if (reactivate)
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = (fade == Fade.In ? 0f : 1f);
        }
    }

    public void FadeOutToValue(float fadeTime, float fadeOutValue, Action finishFade = null)
    {
        this.fade = Fade.Out;
        this.fadeTime = fadeTime;
        this.fadeOutValue = fadeOutValue;
        this.finishFade = finishFade;
    }

    public void StartFadeOrFireEvent(Fade fade, float fadeTime, Action finishFade = null)
    {
        StartFade(fade, fadeTime, finishFade, false);
        if (!gameObject.activeInHierarchy)
        {
            FireEvent();
        }
    }

    public void StopFade(bool setVisible)
    {
        fade = Fade.None;
        gameObject.SetActive(setVisible);
        if (setVisible)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
}
