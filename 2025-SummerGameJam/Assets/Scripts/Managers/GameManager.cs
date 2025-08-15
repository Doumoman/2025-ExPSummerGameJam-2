using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public bool isReset = false;

    public GameObject DamagePrefab;

    
    public int Score = 0;

    public int GetBatchScore(int kan)
    {
        Score += kan;
        return kan;
    }
    public int GetDeleteScore(int line, int kan)
    {
        Score += line * kan * 5;
        return line * kan * 5;
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        
    }
    public void ShowDamage(int damage)
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        GameObject obj = Instantiate(DamagePrefab, canvas.transform);
        obj.GetComponent<DamageEffect>().ShowDamage(damage);
        //ScoreManager.Instance.UpdateScore(Score);
    }
}