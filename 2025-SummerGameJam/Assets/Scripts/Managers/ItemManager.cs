using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance = null;

    [SerializeField] Inventory inven;
    public List<Item> items = new List<Item>();
    public int myItem = 0;
    public event Action OnChanged;


    public static ItemManager Instance => instance == null ? null : instance;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }



    public void AddItem(Item newItem)
    {
        items.Add(newItem);

        UpdateUI(newItem);
    }

    private void UpdateUI(Item newItem)
    {
        inven.AddItem(newItem);
    }
    public void OnClickItem(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0: // 짝수 줄 점수 증가
                Debug.Log("짝수 줄 점수 증가 아이템 사용");
                myItemPlusOne();
                break;

            case 1: // 줄 지우기 점수 2배
                Debug.Log("줄 지우기 점수 2배 아이템 사용");
                break;

            case 2: // 홀수 줄 점수 증가
                Debug.Log("홀수 줄 점수 증가 아이템 사용");
                break;

            case 3: // 콤보 점수 증가
                Debug.Log("콤보 점수 증가 아이템 사용");
                myItemPlusOne();
                break;

            case 4: // 솎아내기
                Debug.Log("솎아내기 아이템 사용");
                break;

            case 5: // 짝수 타일 연속 배치
                Debug.Log("짝수 타일 연속 배치 아이템 사용");
                break;

            case 6: // 꿀조달
                Debug.Log("꿀조달 아이템 사용");
                myItemPlusOne();
                break;

            case 7: // 홀수 타일 연속 배치
                Debug.Log("홀수 타일 연속 배치 아이템 사용");
                break;

            case 8: // 리롤
                Debug.Log("리롤 아이템 사용");
                break;

            case 9: // 성장
                Debug.Log("성장 아이템 사용");
                myItemPlusOne();
                break;

            case 10: // 타일 배치 점수 증가
                Debug.Log("타일 배치 점수 증가 아이템 사용");
                break;

            case 11: // 턴 증가
                Debug.Log("턴 증가 아이템 사용");
                break;

            default:
                Debug.LogWarning($"알 수 없는 아이템 인덱스: {itemIndex}");
                break;
        }
    }
    public void myItemPlusOne()
    {
        myItem++;
    }
}

