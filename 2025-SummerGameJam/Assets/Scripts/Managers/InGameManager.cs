using System;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    #region Singleton
    private static InGameManager _instance;
    public static InGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InGameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InGameManager");
                    _instance = go.AddComponent<InGameManager>();
                }
            }
            return _instance;
        }
    }
    
    public event Action OnMapChanged;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private int[,] _beeHive = new int[9, 9];
    
    void Initialize()
    {
        OnMapChanged?.Invoke();
    }

    void Noki(int x, int y)
    {
        OnMapChanged?.Invoke();
    }

    // ���ο� �� ����
    void RefreshMap()
    {
        // 5x5 ���� �� ���� : 61
        OnMapChanged?.Invoke();
    }
}
