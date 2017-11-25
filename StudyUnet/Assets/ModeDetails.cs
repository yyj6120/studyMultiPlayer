//using UnityEngine;
//using System.Collections;
//using System;

//[Serializable]
//public class ModeDetails
//{
//    [SerializeField]
//    private string modeName;


//    [SerializeField]
//    private string abbreviation;



//    [SerializeField]
//    private string description;



//    [SerializeField]
//    private string id;



//    [SerializeField]
//    protected RulesProcessor rulesProcessor;


//    [SerializeField]
//    private GameObject hudScoreObject;



//    private int index;

//    protected string ModeName
//    {
//        get
//        {
//            return modeName;
//        }

//        set
//        {
//            modeName = value;
//        }
//    }

//    protected string Abbreviation
//    {
//        get
//        {
//            return abbreviation;
//        }

//        set
//        {
//            abbreviation = value;
//        }
//    }

//    protected string Description
//    {
//        get
//        {
//            return description;
//        }

//        set
//        {
//            description = value;
//        }
//    }

//    protected string Id
//    {
//        get
//        {
//            return id;
//        }

//        set
//        {
//            id = value;
//        }
//    }

//    protected GameObject HudScoreObject
//    {
//        get
//        {
//            return hudScoreObject;
//        }

//        set
//        {
//            hudScoreObject = value;
//        }
//    }

//    public int Index
//    {
//        get
//        {
//            return index;
//        }

//        set
//        {
//            index = value;
//        }
//    }

//    public ModeDetails(string name, string description)
//    {
//        this.modeName = name;
//        this.description = description;
//    }

//    public ModeDetails(string name, string description, RulesProcessor rulesProcessor)
//    {
//        this.modeName = name;
//        this.description = description;
//        this.rulesProcessor = rulesProcessor;
//    }
//}
