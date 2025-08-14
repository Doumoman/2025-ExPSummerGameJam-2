using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

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
    
    private List<GameObject> _lightings = new List<GameObject>();
    
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
                    _beeHive[i, j] = eBeehiveType.Normal;
                }
                else
                {
                    _beeHive[i, j] = eBeehiveType.None;
                }
            }
        }
    }

    public void TurnOff()
    {
        foreach (var light in _lightings)
        {
            Color c = light.GetComponent<SpriteRenderer>().color;
            c.a = 1f;
            light.GetComponent<SpriteRenderer>().color = c;
        }
    }
    public bool CanPlaceWorm(WormTile Worm)
    {
        TurnOff();
        _lightings.Clear();
        
        int layerMask = 1 << LayerMask.NameToLayer("Board");

        foreach (Transform child in Worm.transform)
        {
            Collider2D col = Physics2D.OverlapPoint(child.position, layerMask);
            if (!col) return false;

            var tile = col.GetComponent<BoardCell>() ?? col.GetComponentInParent<BoardCell>();
            if (!tile || tile.bIsOccupied) return false;
            
            _lightings.Add(col.gameObject);
        }
        
        foreach (var light in _lightings)
        {
            Color c = light.GetComponent<SpriteRenderer>().color;
            c.a = 224f/ 255f;
            light.GetComponent<SpriteRenderer>().color = c;
        }
        
        return true;
    }

    public void PlaceWorm(WormTile Worm)
    {
        if (Worm.transform.childCount == _lightings.Count)
        {
            foreach (var obj in _lightings)
            {
                BoardCell cell = obj.GetComponent<BoardCell>();
                cell.bIsOccupied = true;

                InGameManager.Instance._worms[cell.x, cell.y] = Worm.WormType;
            }
        }
        DrawManager.Instance.RefreshWorm();
        
        // TODO
        // 1자가 생성되었는지 확인
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
