using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Inst
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InGameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
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