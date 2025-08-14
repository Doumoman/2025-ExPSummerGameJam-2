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
                _instance = FindAnyObjectByType<InGameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InGameManager");
                    _instance = go.AddComponent<InGameManager>();
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
            Initialize();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        
    }
    #endregion
    
    // ========== 인게임 코드 ==============
    private WormSpawner _wormSpawner;
    public event Action OnMapChanged;
    public event Action OnWormChanged;
    
    public eBeehiveType[,] _beeHive = new eBeehiveType[9, 9]; // color 저장
    public eWormType[,] _worms = new eWormType[9, 9]; // 애벌레 저장 (타입별로 다르게)

    private int[] _colorNum = { 20, 20, 20, 20 };

    private void Start()
    {
        _wormSpawner = GetComponent<WormSpawner>();
    }

    public void MakeMap()
    {
        InitBeeHive();
        
        OnMapChanged?.Invoke();

        _wormSpawner.SpawnWorm();
    }
    
    // 맵 초기화
    void InitBeeHive()
    {
        int blackColor = _colorNum[0];
        int redColor = _colorNum[1];
        int yellowColor = _colorNum[2];
        int blueColor = _colorNum[3];
        
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (Mathf.Abs(4 - i) / 2 <= j && j < 9 - (Mathf.Abs(4 - i) + 1) / 2)
                {
                    // TODO
                    // 색상 수에 맞게 컬러를 배치한다
                    
                    _beeHive[i, j] = eBeehiveType.Normal;
                }
                else
                {
                    _beeHive[i, j] = eBeehiveType.None;
                }
            }
        }
    }

    void Noki(int x, int y)
    {
        OnWormChanged?.Invoke();
    }

    // 새로운 맵 생성
    void RefreshMap()
    {
        OnMapChanged?.Invoke();
    }
}
