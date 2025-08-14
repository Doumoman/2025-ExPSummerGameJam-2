using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    private static CoinManager instance = null;
    public static CoinManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public int coin = 10;

    // ���� �߰�
    public void AddCoin(int amount)
    {
        coin += amount;
    }

    public bool UseCoin(int cost)
    {
        if (coin < cost)
        {
            Debug.Log("������ ���� ����");
            return false;
        }
        else
        {
            coin -= cost;
            Debug.Log("������ ���� ����");
            return true;
        }

    }
    public void ResetCoin(int value = 0)
    {
        coin = Mathf.Max(0, value);
    }
    public int GetCoin() => coin;
}

