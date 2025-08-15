using TMPro;
using UnityEngine;

public class PointScore : MonoBehaviour
{
    [SerializeField] TMP_Text pointScoreText;
    int pointScore;

    private void Awake()
    {
        pointScore = ScoreManager.Instance.CurrentStageGoal;
    }

    private void Update()
    {
        pointScoreText.text = pointScore.ToString();
    }
}
