using TMPro;
using UnityEngine;

public class ClearPanelCoinCount : MonoBehaviour
{
    [SerializeField] TMP_Text clearLineCoinText;
    [SerializeField] TMP_Text LeftTurnCoinText;
    [SerializeField] TMP_Text LeftRerollCoinText;
    [SerializeField] TMP_Text TotalCoinText;

    int clearLineCoin = 0;
    int LeftTurnCoin = 0;
    int LeftRerollCoin = 0;
    int TotalCoin = 0;

    private void Awake()
    {
        clearLineCoinText.text = "00";
        LeftTurnCoinText.text = "00";
        LeftRerollCoinText.text = "00";
        TotalCoinText.text = "00";
    }

    private void Start()
    {
        
    }
}
