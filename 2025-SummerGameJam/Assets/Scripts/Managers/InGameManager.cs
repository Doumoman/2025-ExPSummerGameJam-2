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

    [Header("Placement per Turn")]
    [SerializeField] int placementsPerTurn = 3;   // 한 턴에 필요한 배치 횟수(=3)
    int placedThisTurn = 0;

    private List<GameObject> _lightings = new List<GameObject>();
    
    public eBeehiveType[,] _beeHive = new eBeehiveType[9, 9]; // color 저장
    public WormInfo[,] _worms = new WormInfo[9, 9]; // 애벌레 저장 (타입별로 다르게)

    private int[] _colorNum = { 20, 20, 20, 20 };

    private void Start()
    {
        _wormSpawner = GetComponent<WormSpawner>();

        MakeMap();
        ScoreManager.Instance.StartStage(ScoreManager.Instance.CurrentStage);
        ScoreManager.Instance.OnTurnStarted += OnTurnStarted_ResetPlacement;
    }

    public void MakeMap()
    {
        InitBeeHive();
        
        OnMapChanged?.Invoke();

        _wormSpawner.SpawnNewSet();
    }
    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnTurnStarted -= OnTurnStarted_ResetPlacement;
    }
    void OnTurnStarted_ResetPlacement(int stage, int remTurns)
    {
        placedThisTurn = 0;
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

    public int OddPlacement = 0;
    public int EvenPlacement = 0;
    
    public void PlaceWorm(WormTile Worm)
    {
        int DamageSum = 0;
        if (Worm.transform.childCount == _lightings.Count)
        {
            foreach (var obj in _lightings)
            {
                BoardCell cell = obj.GetComponent<BoardCell>();
                cell.bIsOccupied = true;

                InGameManager.Instance._worms[cell.y, cell.x] = Worm._wormInfo;
            }
            DrawManager.Instance.RefreshWorm();
            int Damage = Worm.transform.childCount;
            if (Damage % 2 == 0)
            {
                EvenPlacement = 0;
                OddPlacement++;
                if (OddPlacement > 2 && HasItem(eItemType.LikeOddPosition) > 0)
                {
                    Debug.Log("Kexi2");  

                    Damage += 1 + OddPlacement * 2 * HasItem(eItemType.LikeOddPosition);
                }
            }
            else
            {
                OddPlacement = 0;
                EvenPlacement++;
                if (EvenPlacement > 2 && HasItem(eItemType.LikeEvenPosition) > 0)
                {
                    Debug.Log("Kexi");  
                    Damage += 1 + EvenPlacement * 2 * HasItem(eItemType.LikeEvenPosition);
                }
            }
            Damage *= (1 + HasItem(eItemType.PositionScoreUp));
            DamageSum += Damage;
            
            ScoreManager.Instance.AddPlacementScore(Damage);
            Destroy(Worm.gameObject);
            OnWormPlacedOnce();
        }
        
        var result = GetLines();
        foreach (var pos in result.coords)
        {
            InGameManager.Instance._worms[pos.x, pos.y] = null;
        }
        
        DamageSum += ScoreManager.Instance.AddRowClearScore(result.coords.Count, result.count);
        
        GameManager.Inst.ShowDamage(DamageSum);
        
        DrawManager.Instance.RefreshWorm();
    }

    (int count, HashSet<Vector2Int> coords) GetLines()
    {
        int count = 0;
        HexAllLines hal = new HexAllLines();
        var lines = hal.GetAllLines();
        HashSet<Vector2Int> Vectors = new HashSet<Vector2Int>();
        for (int i = 0; i < lines.Count; i++)
        {
            bool bIsLine = true;
            List<Vector2Int> TVectors = new List<Vector2Int>();
            for (int j = 0; j < lines[i].Count; j++)
            {
                if (_worms[lines[i][j].x, lines[i][j].y] == null)
                {
                    bIsLine = false;
                }
                else
                {
                    TVectors.Add(lines[i][j]);
                }
            }

            if (bIsLine)
            {
                count++;
                foreach (var TVector in TVectors)
                {
                    Vectors.Add(TVector);
                }
            }
        }

        return (count, Vectors);
    }
    
    public (int count, List<Vector2Int> coords) GetNonNullWorms()
    {
        var coords = new List<Vector2Int>();
        int rows = _worms.GetLength(0);
        int cols = _worms.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (_worms[r, c] != null)
                {
                    coords.Add(new Vector2Int(r, c)); // (row, col)
                }
            }
        }

        return (coords.Count, coords);
    }
    
    
    void Noki(int x, int y)
    {
        OnWormChanged?.Invoke();
    }
    //벌레가 한마리 배치될 때
    void OnWormPlacedOnce()
    {
        placedThisTurn = Mathf.Max(0, placedThisTurn + 1);

        if (placedThisTurn >= placementsPerTurn)
        {
            placedThisTurn = 0; // 다음 턴을 위해 초기화

            // 턴 종료 (클리어/패배 분기는 ScoreManager가 처리)
            ScoreManager.Instance?.EndTurn();

            // ★ 스테이지가 계속 진행 중이면, 즉시 다음 3마리 스폰
            //    (클리어/패배로 패널이 열렸다면 Turns는 0이므로 스폰되지 않음)
            if (ScoreManager.Instance != null && ScoreManager.Instance.GetTurn() > 0)
            {
                _wormSpawner?.SpawnNewSet(); // 기존 SpawnWorm()을 쓰고 있다면 그걸 호출
            }
        }
    }
    // 새로운 맵 생성
    public void RefreshMap()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _worms[i, j] = null;
            }
        }
        
        DrawManager.Instance.RefreshWorm();
    }
    
    // ============ 아이템 관련 코드 =============
    
    List<(eItemType Type, int Count)> _itemList = new List<(eItemType, int)>();

    public bool CanGetItem()
    {
        int itemNum = 0;
        foreach (var item in _itemList)
        {
            itemNum += item.Count;
            if (itemNum >= 5)
            {
                return false;
            }
        }

        return true;
    }

    public void GetItem(eItemType itemType)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].Type == itemType)
            {
                _itemList[i] = (itemType, _itemList[i].Count + 1);
                break;
            }
        }
        _itemList.Add((itemType, 1));
        
        Debug.Log($"{itemType} 레벨 1만큼 증가");
    }

    public int HasItem(eItemType itemType)
    {
        foreach (var item in _itemList)
        {
            if (item.Type == itemType)
            {
                return item.Count;
            }
        }

        return 0;
    }
}
