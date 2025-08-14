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
        BuildStageMap();
    }
    #endregion

    [Header("initial set")]
    [SerializeField] int startTurns = 0;
    [SerializeField] int startRerolls = 3;

    [Header("Row remove score multiply")]
    [SerializeField] int rowCellPoint = 5;

    [Header("Remain Turn score multiply")]
    [SerializeField] int remainTurnScore = 3;      // 남은 턴 1개당 추가 점수

    [Header("Combo Bonus(%)")]
    [SerializeField] int combo1Bonus = 10;
    [SerializeField] int combo2Bonus = 25;
    [SerializeField] int combo3Bonus = 50;
    [SerializeField] int combo4PlusBonus = 100;

    /* ───────── 단일 스테이지 테이블 ───────── */
    [Serializable]
    public class StageConfig
    {
        public int stageIndex;
        public int turns;
        public int goalScore;
    }

    [Header("Stage Table (Turns + Goal)")]
    [SerializeField] List<StageConfig> stageTable = new();   // 인스펙터에서 관리
    Dictionary<int, StageConfig> stageMap = new();           // 런타임 조회 캐시

    /* ───────── 상태 ───────── */
    public int CurrentStage { get; private set; } = -1;
    public int StageTotalTurns { get; private set; }
    public int CurrentStageGoal { get; private set; }
    bool stageCleared;

    public int Score { get; private set; }
    public int Turns { get; private set; }
    public int Rerolls { get; private set; }

    /* ───────── 이벤트 ───────── */
    public event Action<int> OnScoreChanged;
    public event Action<int> OnTurnChanged;
    public event Action<int> OnRerollChanged;
    public event Action<int, int> OnTurnStarted;
    public event Action<int, int> OnTurnEnded;

    [Header("If Stage Cleared, shopPopup")]
    [SerializeField] GameObject shopUIPanel;
    [SerializeField] UnityEvent onStageEnded;

    [Header("When Stage Defeated → Defeat Panel")]
    [SerializeField] GameObject defeatPanel;       // 패배 패널
    [SerializeField] UnityEvent onStageDefeated;   // 패배 시 훅
    void Start()
    {
        Rerolls = startRerolls;
        Turns = startTurns;
        FireAllEvents();
    }

    void BuildStageMap()
    {
        stageMap.Clear();
        foreach (var cfg in stageTable)
        {
            if (cfg == null) continue;
            stageMap[cfg.stageIndex] = new StageConfig
            {
                stageIndex = cfg.stageIndex,
                turns = Mathf.Max(0, cfg.turns),
                goalScore = Mathf.Max(0, cfg.goalScore)
            };
        }
    }

    void FireAllEvents()
    {
        OnScoreChanged?.Invoke(Score);
        OnTurnChanged?.Invoke(Turns);
        OnRerollChanged?.Invoke(Rerolls);
    }

    /* ========================= 스테이지 제어 ========================= */

    // 인스펙터 외에 런타임에서도 수정 가능
    public void SetStageConfig(int stageIndex, int turns, int goalScore)
    {
        var cfg = new StageConfig { stageIndex = stageIndex, turns = Mathf.Max(0, turns), goalScore = Mathf.Max(0, goalScore) };
        stageMap[stageIndex] = cfg;

        int idx = stageTable.FindIndex(s => s.stageIndex == stageIndex);
        if (idx >= 0) stageTable[idx] = cfg;
        else stageTable.Add(cfg);
    }

    public void StartStage(int stageIndex, bool resetRerolls = false)
    {
        BuildStageMap(); // 인스펙터 수정 반영
        CurrentStage = stageIndex;

        StageConfig cfg;
        if (!stageMap.TryGetValue(stageIndex, out cfg))
            cfg = new StageConfig { stageIndex = stageIndex, turns = startTurns, goalScore = 0 };

        StageTotalTurns = cfg.turns;
        CurrentStageGoal = cfg.goalScore;
        stageCleared = false;

        /* 스테이지 시작 시 점수 초기화 */
        Score = 0;
        OnScoreChanged?.Invoke(Score);

        Turns = StageTotalTurns;
        if (resetRerolls) Rerolls = startRerolls;

        OnTurnChanged?.Invoke(Turns);
        OnRerollChanged?.Invoke(Rerolls);
        OnTurnStarted?.Invoke(CurrentStage, Turns);
    }

    public void EndStage()
    {
        onStageEnded?.Invoke();
        if (shopUIPanel) shopUIPanel.SetActive(true); // 상점 오픈
    }

    /* ========================= 턴/점수 API ========================= */

    public void EndTurn()
    {
        if (Turns <= 0)  // 안전장치
        {
            if (stageCleared) EndStage(); else EndStageDefeat();
            return;
        }

        Turns = Mathf.Max(0, Turns - 1);
        OnTurnChanged?.Invoke(Turns);
        OnTurnEnded?.Invoke(CurrentStage, Turns);

        if (Turns <= 0)
        {
            // 여기서 분기 
            if (stageCleared) EndStage();      // 목표 달성 후 턴이 0 → 상점(클리어)
            else EndStageDefeat(); // 목표 미달 & 턴 0 → 패배
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
        Score += placedTileCount;
        OnScoreChanged?.Invoke(Score);
        CheckStageGoal();
        return placedTileCount;
    }
    public int AddRowClearScore(int cellsPerRow, int rowsCleared)
    {
        if (cellsPerRow <= 0 || rowsCleared <= 0) return 0;
        int gain = (cellsPerRow * rowCellPoint) * rowsCleared;
        Score += gain;
        OnScoreChanged?.Invoke(Score);
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
        CheckStageGoal();
        return bonus;
    }

    /* ========================= 목표 달성 처리 ========================= */

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

        // 남은 턴 보너스: 남은턴 × remainTurnScore
        if (Turns > 0 && remainTurnScore > 0)
        {
            Score += Turns * remainTurnScore;
            OnScoreChanged?.Invoke(Score);
        }

        Turns = 0;
        OnTurnChanged?.Invoke(Turns);
        EndStage();
    }
    public void CloseShop()
    {
        if (shopUIPanel) shopUIPanel.SetActive(false);
    }

    // 상점 닫고 다음(또는 지정) 스테이지로 시작
    public void NextStage(int? stageIndexOverride = null, bool resetRerolls = true)
    {
        CloseShop();
        int next = stageIndexOverride.HasValue ? stageIndexOverride.Value : (CurrentStage + 1);
        StartStage(next, resetRerolls);
    }
    // ── DEFEAT 경로: 패배 패널 열기/닫기 ─────────────────
    public void EndStageDefeat()   // NEW
    {
        onStageDefeated?.Invoke();
        if (defeatPanel) defeatPanel.SetActive(true);
    }
    public void CloseDefeatPanel() // NEW
    {
        if (defeatPanel) defeatPanel.SetActive(false);
    }

    // ── 재시작: 해당 스테이지 다시 시작 ─────────────────
    public void RestartStage(bool resetRerolls = true)  // NEW
    {
        CloseDefeatPanel();
        // 점수 0으로 초기화는 StartStage 안에서 이미 수행한다고 가정
        StartStage(CurrentStage, resetRerolls);
    }
    /* ========== Getter ========== */
    public int GetTurn() => Turns;
    public int GetScore() => Score;
    public int GetStage() => CurrentStage;
    public int GetReroll() => Rerolls;
    public int GetStageGoal() => CurrentStageGoal;
    public int GetStageTotalTurns() => StageTotalTurns;
}
