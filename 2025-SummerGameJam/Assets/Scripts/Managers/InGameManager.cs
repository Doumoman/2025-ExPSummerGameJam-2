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
    
    // ========== �ΰ��� �ڵ� ==============
    public event Action OnMapChanged;
    public event Action OnWormChanged;
    
    private int[,] _beeHive = new int[9, 9]; // color ����
    private int[,] _worms = new int[9, 9]; // �ֹ��� ���� (Ÿ�Ժ��� �ٸ���)

    private int[] _colorNum = { 20, 20, 20, 20 };
    
    void Initialize()
    {
        InitBeeHive();
        
        OnMapChanged?.Invoke();
    }

    // �� �ʱ�ȭ
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
                    // ���� ���� �°� �÷��� ��ġ�Ѵ�
                    
                    _beeHive[i, j] = 3;
                }
                else
                {
                    _beeHive[i, j] = -1;
                }
                
                Debug.Log($"{_beeHive[i, j]}");
            }
        }
    }

    void Noki(int x, int y)
    {
        OnWormChanged?.Invoke();
    }

    // ���ο� �� ����
    void RefreshMap()
    {
        // 5x5 ���� �� ���� : 61
        OnMapChanged?.Invoke();
    }
}
