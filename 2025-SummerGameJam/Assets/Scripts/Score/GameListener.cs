using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameListener : MonoBehaviour
{
    [Header("Texts (TMP)")]
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] TextMeshProUGUI rerollText;
    [SerializeField] TextMeshProUGUI goalScoreText;
    [SerializeField] TextMeshProUGUI currentScoreText;

    [Header("Buttons")]
    [SerializeField] Button turnButton;
    [SerializeField] Button stageClearButton;
    [SerializeField] Button startGameButton;
    [SerializeField] Button erase1Button;
    [SerializeField] Button erase2Button;
    [SerializeField] Button erase3Button;
    [SerializeField] Button nextStageButton;
    [SerializeField] Button addTurnButton;          // NEW: 턴 +1
    [SerializeField] Button restartRunButton; // 재시작 버튼

    [Header("Stage Start (Inspector int)")]
    [Tooltip("Start Game 버튼으로 시작할 스테이지 인덱스")]
    [SerializeField] int stageToStart = 0;          // NEW: 인스펙터에서 지정

    [Header("Scoring Params")]
    [SerializeField] int cellsPerRow = 10;

    ScoreManager SM => ScoreManager.Instance;
    private void Update()
    {
        RefreshTexts();
    }
    void OnEnable()
    {
        if (turnButton) turnButton.onClick.AddListener(OnClickTurn);
        if (stageClearButton) stageClearButton.onClick.AddListener(OnClickStageClear);
        if (startGameButton) startGameButton.onClick.AddListener(OnClickStartGame);
        if (erase1Button) erase1Button.onClick.AddListener(() => EraseLines(1));
        if (erase2Button) erase2Button.onClick.AddListener(() => EraseLines(2));
        if (erase3Button) erase3Button.onClick.AddListener(() => EraseLines(3));
        if (nextStageButton) nextStageButton.onClick.AddListener(OnClickNextStage);
        if (addTurnButton) addTurnButton.onClick.AddListener(OnClickAddTurn); // NEW
        if (restartRunButton) restartRunButton.onClick.AddListener(OnClickRestartStage);
        if (SM != null)
        {
            SM.OnScoreChanged += _ => RefreshTexts();
            SM.OnTurnChanged += _ => RefreshTexts();
            SM.OnRerollChanged += _ => RefreshTexts();
            SM.OnTurnStarted += (_, __) => RefreshTexts();
            SM.OnTurnEnded += (_, __) => RefreshTexts();
        }
        RefreshTexts();
    }

    void OnDisable()
    {
        if (turnButton) turnButton.onClick.RemoveListener(OnClickTurn);
        if (stageClearButton) stageClearButton.onClick.RemoveListener(OnClickStageClear);
        if (startGameButton) startGameButton.onClick.RemoveListener(OnClickStartGame);
        if (erase1Button) erase1Button.onClick.RemoveAllListeners();
        if (erase2Button) erase2Button.onClick.RemoveAllListeners();
        if (erase3Button) erase3Button.onClick.RemoveAllListeners();
        if (nextStageButton) nextStageButton.onClick.RemoveListener(OnClickNextStage);
        if (addTurnButton) addTurnButton.onClick.RemoveListener(OnClickAddTurn);
        if (restartRunButton) restartRunButton.onClick.RemoveListener(OnClickRestartStage);

        if (SM != null)
        {
            SM.OnScoreChanged -= _ => RefreshTexts();
            SM.OnTurnChanged -= _ => RefreshTexts();
            SM.OnRerollChanged -= _ => RefreshTexts();
            SM.OnTurnStarted -= (_, __) => RefreshTexts();
            SM.OnTurnEnded -= (_, __) => RefreshTexts();
        }
    }

    /* ───────── 버튼 핸들러 ───────── */

    void OnClickTurn()
    {
        SM?.EndTurn();
        RefreshTexts();
    }

    void OnClickStageClear()
    {
        if (SM == null) return;
        int need = Mathf.Max(0, SM.GetStageGoal() - SM.GetScore());
        if (need > 0) SM.AddPlacementScore(need);
        RefreshTexts();
    }

    void OnClickStartGame()
    {
        if (SM == null) return;
        SM.StartStage(stageToStart, resetRerolls: true); // 인스펙터 int로 시작
        RefreshTexts();
    }

    void OnClickNextStage()
    {
        if (SM == null) return;
        SM.NextStage(); // 상점 닫고 현재+1 스테이지
        RefreshTexts();
    }

    void OnClickAddTurn()                           // NEW
    {
        if (SM == null) return;
        SM.AddTurns(1);
        RefreshTexts();
    }

    void EraseLines(int lines)
    {
        if (SM == null || lines <= 0) return;
        int gained = SM.AddRowClearScore(cellsPerRow, lines);
        SM.AddComboBonus(lines, gained);
        RefreshTexts();
    }
    public void OnClickRestartStage()
    {
        ScoreManager.Instance?.RestartRun(resetRerolls: true);
    }
    /* ───────── UI 갱신 ───────── */

    void RefreshTexts()
    {
        if (SM == null) return;
        if (stageText) stageText.text = $"Stage\n{SM.GetStage()}";
        if (turnText) turnText.text = $"Turn\n{SM.GetTurn()}";
        if (rerollText) rerollText.text = $"Reroll\n{SM.GetReroll()}";
        if (goalScoreText) goalScoreText.text = $"GoalScore\n{SM.GetStageGoal()}";
        if (currentScoreText) currentScoreText.text = $"CurrentScore\n{SM.GetScore()}";
        if (coinText) coinText.text = $"Coin\n{SM.GetCoin()}";
    }
}
