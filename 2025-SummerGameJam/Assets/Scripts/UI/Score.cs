using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    int score;

    private void Awake()
    {
        score = ScoreManager.Instance.Score;
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }
}
