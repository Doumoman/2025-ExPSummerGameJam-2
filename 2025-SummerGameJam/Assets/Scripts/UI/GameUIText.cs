using System;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class GameUIText : MonoBehaviour
{
    [SerializeField] TMP_Text myTurn;
    [SerializeField] TMP_Text pointScore;
    [SerializeField] TMP_Text reroll;
    [SerializeField] private TMP_Text ScoreText;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += OnScoreChanged;
        ScoreManager.Instance.OnTurnChanged += OnTurnChanged;
        ScoreManager.Instance.OnRerollChanged += OnRerollChanaged;

    }

    private void Update()
    {
        myTurn.text = ScoreManager.Instance.Turns.ToString();
        pointScore.text = ScoreManager.Instance.GetStageGoal().ToString();
        reroll.text = ScoreManager.Instance.GetReroll().ToString();
    }

    void OnScoreChanged(int newScore)
    {
        ScoreText.SetText(newScore.ToString());
    }

    void OnTurnChanged(int newTurn)
    {
        myTurn.SetText(newTurn.ToString());
    }

    void OnRerollChanaged(int newReroll)
    {
        reroll.SetText(newReroll.ToString());
    }
}