using TMPro;
using UnityEngine;

public class StageEnterance : MonoBehaviour
{
    [SerializeField] TMP_Text Stage;
    [SerializeField] TMP_Text GoalScore;

    private void Start()
    {
        Stage.text = ScoreManager.Instance.CurrentStage.ToString();
        GoalScore.text = ScoreManager.Instance.GetStageGoal().ToString();
    }
}
