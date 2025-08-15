using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    public static ScoreManager Instance { get; private set; }
    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    const string PP_HIGHEST_CLEARED_STAGE = "HighestClearedStage";

    [Header("Goal Formula Params (Goal = ceil((a*b^(n-c) + s - a*b^(1-c)) * 10))")]
    [SerializeField] float a = 50f;
    [SerializeField] float b = 1.15f;
    [SerializeField] float c = 1f;
    [SerializeField] float s = 0f;

    [Header("Fixed Stage Settings")]
    [SerializeField] int fixedStageTurns = 20;

    [Header("initial set")]
    [SerializeField] int startTurns = 0;
    [SerializeField] int startRerolls = 3;

    [Header("Progress Load Options")]
    [Tooltip("���� ���� �� PlayerPrefs���� ���൵�� �ڵ����� �ҷ����� ����")]
    [SerializeField] bool autoLoadFromPrefs = true;

    [Tooltip("����� '�ְ� Ŭ���� ��������'�� ���� ������������ �������� ���� (true�� saved+1, false�� saved)")]
    [SerializeField] bool startAtNextAfterLoad = true;

    [Tooltip("�α׶���ũ ���� �������� �ε���(���� ����/���� �� ����)")]
    [SerializeField] int firstStageIndex = 1;

    [Header("Row remove score multiply")]
    [SerializeField] int rowCellPoint = 5;

    [Header("Remain Turn score multiply")]
    [SerializeField] int remainTurnScore = 3;      // ���� �� 1���� �߰� ����

    [Header("Combo Bonus(%)")]
    [SerializeField] int combo1Bonus = 10;
    [SerializeField] int combo2Bonus = 25;
    [SerializeField] int combo3Bonus = 50;
    [SerializeField] int combo4PlusBonus = 100;

    int ComputeStageGoal(int stageIndex)
    {
        // stageIndex = n
        float x = a * Mathf.Pow(b, stageIndex - c) + s - a * Mathf.Pow(b, 1f - c);
        // �Ҽ��� ù°�ڸ� �ø� �� *10 ȿ�� == ceil(x * 10)
        int goal = Mathf.CeilToInt(x * 10f);
        return Mathf.Max(0, goal);
    }

    /* ������������������ ���� ������������������ */
    public int CurrentStage { get; private set; } = -1;
    public int StageTotalTurns { get; private set; }
    public int CurrentStageGoal { get; private set; }
    bool stageCleared;

    public int Score { get; private set; }
    public int Turns { get; private set; }
    public int Rerolls { get; private set; }

    /* ������������������ �̺�Ʈ ������������������ */
    public event Action<int> OnGoalScoreChanged;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnTurnChanged;
    public event Action<int> OnRerollChanged;
    public event Action<int, int> OnTurnStarted;
    public event Action<int, int> OnTurnEnded;

    [Header("When Stage Cleared �� Clear Panel")]
    [SerializeField] GameObject clearPanel;        // �� Ŭ���� �г�
    [SerializeField] UnityEvent onStageCleared;    // �� Ŭ���� �� ��(ȿ����/���� ��)

    [Header("If Stage Cleared, shopPopup")]
    [SerializeField] GameObject shopUIPanel;
    [SerializeField] UnityEvent onStageEnded;

    [Header("When Stage Defeated �� Defeat Panel")]
    [SerializeField] GameObject defeatPanel;       // �й� �г�
    [SerializeField] UnityEvent onStageDefeated;   // �й� �� ��
    
    void Start()
    {
        if (GameManager.Inst.isReset)
        {
            RestartRun();
        }
        Rerolls = startRerolls;
        Turns = startTurns;

        if (autoLoadFromPrefs)
        {
            StartFromSavedProgress(resetRerolls: true, startAtNext: startAtNextAfterLoad);
            return; // ���⼭ �����ϸ� �ʱ� �̺�Ʈ�鵵 StartStage �ȿ��� ����
        }

        FireAllEvents();
    }

    

    void FireAllEvents()
    {
        OnScoreChanged?.Invoke(Score);
        OnTurnChanged?.Invoke(Turns);
        OnRerollChanged?.Invoke(Rerolls);
    }

    /* ========================= �������� ���� ========================= */

    // �ν����� �ܿ� ��Ÿ�ӿ����� ���� ����
    

    public void StartStage(int stageIndex, bool resetRerolls = false)
    {
        CurrentStage = stageIndex;

        // ��ǥ ���ھ� �ڵ� ���
        CurrentStageGoal = ComputeStageGoal(stageIndex);
        OnGoalScoreChanged?.Invoke(CurrentStageGoal);
        stageCleared = false;

        /* �������� ���� �� ���� �ʱ�ȭ */
        Score = 0;
        OnScoreChanged?.Invoke(Score);
        
        // �ʵ� �ʱ�ȭ
        InGameManager.Instance.RefreshMap();

        // �� ���� ������ ���
        StageTotalTurns = fixedStageTurns;
        Turns = StageTotalTurns;

        if (resetRerolls) Rerolls = startRerolls + InGameManager.Instance.HasItem(eItemType.TileRerollUp);

        OnTurnChanged?.Invoke(Turns);
        OnRerollChanged?.Invoke(Rerolls);
        OnTurnStarted?.Invoke(CurrentStage, Turns);
    }

    public void EndStage()
    {
        onStageEnded?.Invoke();
        if (shopUIPanel) shopUIPanel.SetActive(true); // ���� ����
    }
    /* ========================= ���൵ ����/�ε� API ========================= */

    /// <summary> �������� Ŭ���� �� �ְ� Ŭ���� ���������� PlayerPrefs�� ���� </summary>
    void SaveStageClearProgress()
    {
        // ���ݱ��� ����� �ְ� Ŭ���� ��������
        int prev = PlayerPrefs.GetInt(PP_HIGHEST_CLEARED_STAGE, firstStageIndex - 1);
        int next = Mathf.Max(prev, CurrentStage);
        PlayerPrefs.SetInt(PP_HIGHEST_CLEARED_STAGE, next);
        PlayerPrefs.Save();
    }

    public void UpdateScore(int newScore)
    {
        Score = newScore;
        OnScoreChanged?.Invoke(newScore);
    }
    
    /// <summary> ����� �ְ� Ŭ���� �������� �б� (������ firstStageIndex-1 ��ȯ) </summary>
    public int GetSavedClearedStage()
    {
        return PlayerPrefs.GetInt(PP_HIGHEST_CLEARED_STAGE, firstStageIndex - 1);
    }

    /// <summary> ���൵ �ʱ�ȭ(�����/�׽�Ʈ��) </summary>
    public void ResetSavedProgress()
    {
        PlayerPrefs.DeleteKey(PP_HIGHEST_CLEARED_STAGE);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ����� ���൵�� ����. startAtNext=true�� (���尪+1)����, false�� ���尪���� ����.
    /// ���尪�� ���ų� firstStageIndex���� ������ firstStageIndex�� ����.
    /// </summary>
    public void StartFromSavedProgress(bool resetRerolls = true, bool startAtNext = true)
    {
        int saved = GetSavedClearedStage(); // ��: ó���� firstStageIndex-1
        int target = startAtNext ? (saved + 1) : saved;
        target = Mathf.Max(firstStageIndex, target);
        StartStage(target, resetRerolls);
    }
    /* ========================= ��/���� API ========================= */

    public void EndTurn()
    {
        if (Turns <= 0)  // ������ġ
        {
            if (stageCleared) EndStage(); else EndStageDefeat();
            return;
        }

        Turns = Mathf.Max(0, Turns - 1);
        OnTurnChanged?.Invoke(Turns);
        OnTurnEnded?.Invoke(CurrentStage, Turns);

        if (Turns <= 0)
        {
            // ���⼭ �б� 
            if (stageCleared) EndStage();      // ��ǥ �޼� �� ���� 0 �� ����(Ŭ����)
            else EndStageDefeat(); // ��ǥ �̴� & �� 0 �� �й�
        }
        else
        {
            OnTurnStarted?.Invoke(CurrentStage, Turns);
        }
    }

    public void AddTurns(int add)
    {
        Turns = Mathf.Max(0, Turns + add);
        StageTotalTurns = Mathf.Max(StageTotalTurns, Turns);
        OnTurnChanged?.Invoke(Turns);
    }
    public void AddRerolls(int add)
    {
        Rerolls = Mathf.Max(0, Rerolls + add);
        OnRerollChanged?.Invoke(Rerolls);
    }

    public int AddPlacementScore(int placedTileCount)
    {
        if (placedTileCount <= 0) return 0;

        SoundManager.Instance.EffectSoundOn("TileV1");

        Score += placedTileCount;
        OnScoreChanged?.Invoke(Score);
        CheckStageGoal();
        return placedTileCount;
    }
    public int AddRowClearScore(int cellsPerRow, int rowsCleared)
    {
        if (cellsPerRow <= 0 || rowsCleared <= 0) return 0;
        

        SoundManager.Instance.EffectSoundOn("TransformationV1");

        int gain = (cellsPerRow * rowCellPoint) * rowsCleared;
        gain *= (1 + InGameManager.Instance.HasItem(eItemType.EreaseScoreUp));
        if (cellsPerRow / 2 == 0 && InGameManager.Instance.HasItem(eItemType.LikeEvenErease) > 0)
        {
            gain *= 3 * InGameManager.Instance.HasItem(eItemType.LikeEvenErease);
        }
        if (cellsPerRow / 2 == 1 && InGameManager.Instance.HasItem(eItemType.LikeOddErease) > 0)
        {
            gain *= 3 * InGameManager.Instance.HasItem(eItemType.LikeOddErease);
        }
        if (rowsCleared >= 2 && InGameManager.Instance.HasItem(eItemType.ComboUp) > 0)
        {
            gain *= 4 * InGameManager.Instance.HasItem(eItemType.ComboUp);
        }
        Score += gain;
        OnScoreChanged?.Invoke(Score);
        // �� 1���� 1���� (���ÿ� N�� �� +N ����)
        // �� 1���� 1���� (���ÿ� N�� �� +N ����)
        CoinManager.Instance?.AddCoin(rowsCleared);

        /* �� �߿�: ���� ��ǥ �޼� üũ */
        CheckStageGoal();

        /* �� ��ǥ �޼����� �ʾҴٸ� �׶��� �� �Ҹ� */
        if (!stageCleared)
            EndTurn();

        CheckStageGoal();
        return gain;
    }
    public int AddComboBonus(int comboCount, int lastGain)
    {
        if (comboCount <= 0 || lastGain <= 0) return 0;
        int addPercent = comboCount switch
        {
            1 => combo1Bonus,
            2 => combo2Bonus,
            3 => combo3Bonus,
            _ => combo4PlusBonus
        };
        int bonus = Mathf.RoundToInt(lastGain * (addPercent / 100f));
        Score += bonus;
        OnScoreChanged?.Invoke(Score);

        // �޺� ī��Ʈ��ŭ ���� �߰�
        CoinManager.Instance?.AddCoin(comboCount);
        CheckStageGoal();
        return bonus;
    }

    /* ========================= ��ǥ �޼� ó�� ========================= */

    void CheckStageGoal()
    {
        if (stageCleared) return;
        if (CurrentStage < 0) return;

        if (CurrentStageGoal > 0 && Score >= CurrentStageGoal)
            StageClear();
    }

    void StageClear()
    {
        stageCleared = true;

        // ���� �� ���ʽ�: ������ �� remainTurnScore
        if (Turns > 0 && remainTurnScore > 0)
        {
            Score += Turns * remainTurnScore;
            OnScoreChanged?.Invoke(Score);
        }
        // ���� ���ʽ�(���� �� ����ŭ)
        if (Turns > 0)
            CoinManager.Instance?.AddCoin(Turns);

        SaveStageClearProgress(); // �������� ����
        CoinManager.Instance?.SaveCoin(); // ���� ����
        Turns = 0;
        OnTurnChanged?.Invoke(Turns);
        OpenClearPanel();
    }
    public void CloseShop()
    {
        if (shopUIPanel) shopUIPanel.SetActive(false);
    }
    public void OpenClearPanel()
    {
        onStageCleared?.Invoke();

        SoundManager.Instance.OnBgmVolumeChange(0.5f);
        SoundManager.Instance.EffectSoundOn("WinV1");

        if (clearPanel) clearPanel.SetActive(true);
    }

    // �� Ŭ���� �г� �ݰ�, ���� UI ����
    public void CloseClearPanelAndOpenShopUI()
    {
        if (clearPanel) clearPanel.SetActive(false);
        SoundManager.Instance.OnBgmVolumeChange(1f);
        EndStage(); // ���� ���� ���� ���� ����
    }

    // ���� �ݰ� ����(�Ǵ� ����) ���������� ����
    public void NextStage(int? stageIndexOverride = null, bool resetRerolls = true)
    {
        CloseShop();
        int next = stageIndexOverride.HasValue ? stageIndexOverride.Value : (CurrentStage + 1);
        StartStage(next, resetRerolls);
    }
    // ���� DEFEAT ���: �й� �г� ����/�ݱ� ����������������������������������
    public void EndStageDefeat()   // NEW
    {
        onStageDefeated?.Invoke();

        SoundManager.Instance.OnBgmVolumeChange(0.5f);
        SoundManager.Instance.EffectSoundOn("LoseV1");

        if (defeatPanel) defeatPanel.SetActive(true);
    }
    public void CloseDefeatPanel() // NEW
    {
        if (defeatPanel) defeatPanel.SetActive(false);
        SoundManager.Instance.OnBgmVolumeChange(1f);
    }

    public void RestartRun(bool resetRerolls = true)
    {
        CloseDefeatPanel();

        // ���� ���� ����
        CoinManager.Instance?.ResetCoin(0);
        // �κ��丮�� ������ ���� ����
        ItemManager.Instance?.ClearAllItems();
        // ���������� ó������ ����(������ StartStage ���ο��� 0���� �ʱ�ȭ��)
        StartStage(firstStageIndex, resetRerolls);
        ResetSavedProgress();
    }
    /* ========== Getter ========== */
    public int GetTurn() => Turns;
    public int GetScore() => Score;
    public int GetStage() => CurrentStage;
    public int GetReroll() => Rerolls;
    public int GetStageGoal() => CurrentStageGoal;
    public int GetStageTotalTurns() => StageTotalTurns;
    public int GetCoin() => CoinManager.Instance ? CoinManager.Instance.GetCoin() : 0;
}
