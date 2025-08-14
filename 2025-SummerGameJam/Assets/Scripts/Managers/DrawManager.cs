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
                
                if(_beeHive[i, j] != eBeehiveType.None)
                    Instantiate(hexPrefab, spawnPos, Quaternion.identity, TileContainer);
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
    
    void ApplyMapImage(eBeehiveType beehiveType)
    {
        switch (beehiveType)
        {
            case eBeehiveType.Red:
                break;
            case eBeehiveType.Blue:
                break;
            case eBeehiveType.Black:
                break;
            case eBeehiveType.Normal:
                break;
        }
    }
}
