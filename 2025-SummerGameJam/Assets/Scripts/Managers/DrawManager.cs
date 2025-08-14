using System;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    #region Singleton
    private static DrawManager _instance;
    public static DrawManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DrawManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InGameManager");
                    _instance = go.AddComponent<DrawManager>();
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
    #endregion
    
    // ================= 인게임 코드 ===============

    public Transform TileContainer;
    public GameObject hexPrefab;

    public Transform WormContainer;
    public GameObject wormPrefab;

    private int centerCol = 4;
    private int centerRow = 4;

    private float hexSize = 1f;

    void Initialize()
    {
        InGameManager.Instance.OnMapChanged += Build;
    }
    
    void Build()
    {
        RemoveTile();
        
        var _beeHive = InGameManager.Instance._beeHive;
        
        if (!hexPrefab) return;

        int centerRow = 4;
        int centerCol = 4;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                float spawnX = i % 2 == 0 ? (j - centerCol) *  Mathf.Sqrt(3) / 2f * hexSize : (j - centerCol + 0.5f) * Mathf.Sqrt(3) / 2f * hexSize;
                float SpawnY = (i - centerRow) * 0.75f * hexSize;
                Vector2 spawnPos = new Vector2(spawnX, SpawnY);

                if (_beeHive[i, j] != eBeehiveType.None)
                {
                    GameObject obj = Instantiate(hexPrefab, spawnPos, Quaternion.identity, TileContainer);
                    obj.GetComponent<BoardCell>().x = j;
                    obj.GetComponent<BoardCell>().y = i;
                }
            }
        }
    }

    public void RefreshWorm()
    {
        RemoveWorm();
        
        var _worms = InGameManager.Instance._worms;

        BoardCell[] cells = FindObjectsByType<BoardCell>(FindObjectsSortMode.None);
        Debug.Log($"찾은 {cells.Length}");
        
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                float spawnX = i % 2 == 0 ? (j - centerCol) *  Mathf.Sqrt(3) / 2f * hexSize : (j - centerCol + 0.5f) * Mathf.Sqrt(3) / 2f * hexSize;
                float SpawnY = (i - centerRow) * 0.75f * hexSize;
                Vector2 spawnPos = new Vector2(spawnX, SpawnY);

                foreach (var cell in cells)
                {
                    if (cell.x == j && cell.y == i)
                    {
                        cell.bIsOccupied = _worms[i, j] != null;
                    }
                }
                
                if (_worms[i, j] != null)
                {
                    GameObject obj = Instantiate(wormPrefab, spawnPos, Quaternion.identity, WormContainer);
                    obj.GetComponent<Worm>().Initialize(_worms[i, j]);
                    
                }
            }
        }
    }
    
    void RemoveTile()
    {
        foreach (Transform child in TileContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void RemoveWorm()
    {
        foreach (Transform child in WormContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
