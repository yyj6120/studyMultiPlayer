using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public enum MapEffectsGroup
{
    Desert,
    Snow
}

[Serializable]
public class MapDetails
{
    [SerializeField]
    protected string name;

    public string Name
    {
        get { return name; }
    }

    //This is marked as a serialized field for debugging purposes only
    [SerializeField]
    protected bool isLocked;

    public bool IsLocked
    {
        get { return isLocked; }
    }

    [SerializeField]
    [Multiline]
    protected string description;

    public string Description
    {
        get { return description; }
    }

    [SerializeField]
    protected string sceneName, id;

    public string SceneName
    {
        get { return sceneName; }
    }

    public string Id
    {
        get { return id; }
    }

    [SerializeField]
    protected Sprite image;

    public Sprite Image
    {
        get { return image; }
    }

    [SerializeField]
    protected int unlockCost;

    public int UnlockCost
    {
        get { return unlockCost; }
    }

    [SerializeField]
    protected MapEffectsGroup effectsGroup;

    public MapEffectsGroup EffectsGroup
    {
        get { return effectsGroup; }
    }

    [SerializeField]
    protected AudioClip levelMusic;

    public AudioClip LevelMusic
    {
        get { return levelMusic; }
    }
}