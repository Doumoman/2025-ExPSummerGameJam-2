using TMPro;
using UnityEngine;

public class MyCoin : MonoBehaviour
{
    [SerializeField]
    TMP_Text coin;

    private void Update()
    {
        coin.text = CoinManager.Instance.GetCoin().ToString();
    }
}
