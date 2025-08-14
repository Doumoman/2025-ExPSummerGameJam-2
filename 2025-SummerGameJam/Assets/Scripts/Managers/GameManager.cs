using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region  Singleton

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

    #endregion
    
    // =============== 인게임 코드 =============

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

    
    public int Score = 0;

    public void GetBatchScore(int kan)
    {
        Score += kan;
        Debug.Log(kan);
        UpdateScore();
    }
    public void GetDeleteScore(int line, int kan)
    {
        Score += line * kan * 5;
        Debug.Log(line * kan * 5);
        UpdateScore();
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    private void UpdateScore()
    {
        Debug.Log($"현재 점수 : {Score}");
    }
}