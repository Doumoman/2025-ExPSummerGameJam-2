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
            LoadCoin(); // ★ 게임 시작 시 PlayerPrefs에서 불러오기
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public int coin = 10;

    // 코인 추가
    public void AddCoin(int amount)
    {
        coin += amount;
    }

    public bool UseCoin(int cost)
    {
        if (coin < cost)
        {
            Debug.Log("아이템 구입 실패");
            return false;
        }
        else
        {
            coin -= cost;
            Debug.Log("아이템 구입 성공");
            SaveCoin(); // ★ 변경될 때마다 저장
            return true;
        }

    }
    //playerPrefs로 저장
    public void SaveCoin()
    {
        PlayerPrefs.SetInt(PP_COIN_KEY, coin);
        PlayerPrefs.Save();
    }

    public void LoadCoin()
    {
        coin = PlayerPrefs.GetInt(PP_COIN_KEY, coin); // 없으면 현재 값 유지
    }

    public void ResetCoin(int value = 0)
    {
        coin = Mathf.Max(0, value);
        SaveCoin(); // ★ 변경될 때마다 저장
    }
    public int GetCoin() => coin;
}

