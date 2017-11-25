//using System;
//using UnityEngine;
//using UnityEngine.Networking;

//public class GameSettings : PersistentSingleton<GameSettings>
//{
//    public event Action<MapDetails> mapChanged;
//    public event Action<ModeDetails> modeChanged;

//    [SerializeField]
//    protected MapList m_MapList;

//    [SerializeField]
//    protected SinglePlayerMapList m_SinglePlayerMapList;

//    [SerializeField]
//    protected ModeList m_ModeList;

//    public MapDetails map
//    {
//        get;
//        private set;
//    }

//    public int mapIndex
//    {
//        get;
//        private set;
//    }

//    public ModeDetails mode
//    {
//        get;
//        private set;
//    }

//    public int modeIndex
//    {
//        get;
//        private set;
//    }

//    public int scoreTarget
//    {
//        get;
//        private set;
//    }

//    public bool isSinglePlayer
//    {
//        get { return NetworkManager.instance.isSinglePlayer; }
//    }

//    public void SetMapIndex(int index)
//    {
//        map = m_MapList[index];
//        mapIndex = index;

//        if (mapChanged != null)
//        {
//            mapChanged(map);
//        }
//    }

//    public void SetModeIndex(int index)
//    {
//        SetMode(m_ModeList[index], index);
//    }

//    public void SetupSinglePlayer(int mapIndex, ModeDetails modeDetails)
//    {
//        this.map = m_SinglePlayerMapList[mapIndex];
//        this.mapIndex = mapIndex;
//        if (mapChanged != null)
//        {
//            mapChanged(map);
//        }

//        SetMode(modeDetails, -1);
//    }

//    public void SetupSinglePlayer(MapDetails map, ModeDetails modeDetails)
//    {
//        this.map = map;
//        this.mapIndex = -1;
//        if (mapChanged != null)
//        {
//            mapChanged(map);
//        }

//        SetMode(modeDetails, -1);
//    }

//    private void SetMode(ModeDetails mode, int modeIndex)
//    {
//        this.mode = mode;
//        this.modeIndex = modeIndex;
//        if (modeChanged != null)
//        {
//            modeChanged(mode);
//        }

//        mode.rulesProcessor.GetColorProvider().Reset();
//        scoreTarget = mode.rulesProcessor.scoreTarget;
//    }
//}
