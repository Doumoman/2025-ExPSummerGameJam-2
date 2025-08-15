using UnityEngine;
using UnityEngine.UI;

public class ScoreGauge : MonoBehaviour
{
    [SerializeField] Image fillImage;

    float rate;
    private void Update()
    {
        //rate = ScoreManager.Instance.Score / ScoreManager.Instance.CurrentStageGoal;
        //fillImage.fillAmount = Mathf.Clamp01(rate);
    }
}
