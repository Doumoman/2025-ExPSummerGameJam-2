using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    static GameManager s_inst;
    public static GameManager Inst
    {
        get
        {
            if (s_inst == null)
            {
                s_inst = new GameManager();
            }
            return s_inst;
        }
    }

    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();

    [SerializeField]
    public static ResourceManager Resource { get { return Inst._resource; } }
    public static SoundManager Sound { get { return Inst._sound; } }
    public static bool isPlayerZoomOutAllowed = false;
    public static bool isFinishBossZoominAllowed = false;

    public static bool isStage1Cleared = false;
    public static bool isStage2Cleared = false;
    public static bool isStage3Cleared = false;

    private void Awake()
    {
        Init();
    }
    static void Init()
    {
        if (s_inst == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_inst = go.GetComponent<GameManager>();

        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
    }
}