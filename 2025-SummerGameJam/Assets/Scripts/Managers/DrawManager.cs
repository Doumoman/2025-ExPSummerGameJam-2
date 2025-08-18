using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.WSA;

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

    public GameObject BlowPrefab;
    
    public Transform TileContainer;
    public GameObject hexPrefab;

    public Transform WormContainer;
    public GameObject wormPrefab;

    private int centerCol = 4;
    private int centerRow = 4;

    private float hexSize = 1f;
    bool suppressConsumeThisRefresh = false;

    /// <summary>리롤 직전 WormSpawner가 호출: 이번 Refresh에서 '소비' 카운팅을 하지 않음</summary>
    public void SuppressConsumeOnce()
    {
        suppressConsumeThisRefresh = true;
    }
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
                Vector2 spawnPos = new Vector2(spawnX, SpawnY + 3f);

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

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                float spawnX = i % 2 == 0 ? (j - centerCol) *  Mathf.Sqrt(3) / 2f * hexSize : (j - centerCol + 0.5f) * Mathf.Sqrt(3) / 2f * hexSize;
                float SpawnY = (i - centerRow) * 0.75f * hexSize;
                Vector2 spawnPos = new Vector2(spawnX, SpawnY + 3f);

                foreach (var cell in cells)
                {
                    if (cell.x == j && cell.y == i)
                    {
                        bool wasOccupied = cell.bIsOccupied;
                        cell.bIsOccupied = _worms[i, j] != null;

                        if (wasOccupied && !cell.bIsOccupied)
                        {
                            Instantiate(BlowPrefab, cell.transform.position, quaternion.identity, this.transform);

                        }
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
        if (TileContainer == null) return;
        
        foreach (Transform child in TileContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void RemoveWorm()
    {
        if (WormContainer == null) return;

        for (int i = WormContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = WormContainer.GetChild(i);
            if (child != null)
                Destroy(child.gameObject);
        }
    }
}
