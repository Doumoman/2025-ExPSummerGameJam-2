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
    [SerializeField] Button goShopFromClearButton; // 클리어 후 상점 UI 여는 버튼
    [SerializeField] Button continueGameButton;
    [SerializeField] Button quitGameButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button returnToMenuButton;

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
        if (goShopFromClearButton) goShopFromClearButton.onClick.AddListener(OnClickGoShopFromClear);
        if (continueGameButton) continueGameButton.onClick.AddListener(OnClickContinue);
        if (quitGameButton) quitGameButton.onClick.AddListener(OnClickQuit);
        if (resumeButton) resumeButton.onClick.AddListener(OnclickResume);
        if (returnToMenuButton) returnToMenuButton.onClick.AddListener(OnClickReturn);
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
        if (goShopFromClearButton) goShopFromClearButton.onClick.RemoveListener(OnClickGoShopFromClear);
        if (continueGameButton) continueGameButton.onClick.RemoveListener(OnClickContinue);
        if (quitGameButton) quitGameButton.onClick.RemoveListener(OnClickQuit);
        if (resumeButton) resumeButton.onClick.RemoveListener(OnclickResume);
        if (returnToMenuButton) returnToMenuButton.onClick.RemoveListener(OnClickReturn);

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

        SoundManager.Instance.EffectSoundOn("SelectV1");
    }

    void OnClickStageClear()
    {
        if (SM == null) return;
        int need = Mathf.Max(0, SM.GetStageGoal() - SM.GetScore());
        if (need > 0) SM.AddPlacementScore(need);
        RefreshTexts();

        SoundManager.Instance.EffectSoundOn("SelectV1");
    }

    void OnClickStartGame()
    {
        if (SM == null) return;
        SM.StartStage(stageToStart, resetRerolls: true); // 인스펙터 int로 시작
        RefreshTexts();

        SoundManager.Instance.EffectSoundOn("SelectV1");
    }

    void OnClickNextStage()
    {
        if (SM == null) return;
        SM.NextStage(); // 상점 닫고 현재+1 스테이지
        RefreshTexts();

        SoundManager.Instance.EffectSoundOn("SelectV1");
    }

    void OnClickAddTurn()                           // NEW
    {
        if (SM == null) return;
        SM.AddTurns(1);
        RefreshTexts();

        SoundManager.Instance.EffectSoundOn("SelectV1");
    }
    void OnClickGoShopFromClear()
    {
        ScoreManager.Instance?.CloseClearPanelAndOpenShopUI();
        RefreshTexts();

        SoundManager.Instance.EffectSoundOn("SelectV1");
    }
    void EraseLines(int lines)
    {
        if (SM == null || lines <= 0) return;
        int gained = SM.AddRowClearScore(cellsPerRow, lines);
        SM.AddComboBonus(lines, gained);
        RefreshTexts();

        SoundManager.Instance.EffectSoundOn("SelectV1");
    }
    public void OnClickRestartStage()
    {
        ScoreManager.Instance?.RestartRun(resetRerolls: true);

        SoundManager.Instance.EffectSoundOn("SelectV1");
    }

    public void OnClickContinue()
    {
        SoundManager.Instance.EffectSoundOn("SelectV1");
    }

    private void OnClickQuit()
    {
        SoundManager.Instance.EffectSoundOn("SelectV1");
    }

    private void OnclickResume()
    {
        SoundManager.Instance.EffectSoundOn("SelectV1");
    }

    private void OnClickReturn()
    {
        SoundManager.Instance.EffectSoundOn("SelectV1");
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
