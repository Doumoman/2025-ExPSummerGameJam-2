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
    const string PP_COIN_KEY = "PlayerCoin";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadCoin(); // �� ���� ���� �� PlayerPrefs���� �ҷ�����
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
            SaveCoin(); // �� ����� ������ ����
            return true;
        }

    }
    //playerPrefs�� ����
    public void SaveCoin()
    {
        PlayerPrefs.SetInt(PP_COIN_KEY, coin);
        PlayerPrefs.Save();
    }

    public void LoadCoin()
    {
        coin = PlayerPrefs.GetInt(PP_COIN_KEY, coin); // ������ ���� �� ����
    }

    public void ResetCoin(int value = 0)
    {
        coin = Mathf.Max(0, value);
        SaveCoin(); // �� ����� ������ ����
    }
    public int GetCoin() => coin;
}

