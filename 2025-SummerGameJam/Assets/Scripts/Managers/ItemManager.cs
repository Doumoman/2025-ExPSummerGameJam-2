using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance = null;
    [SerializeField] int capacity = 5;                 // 인벤토리 최대치
    [SerializeField] Inventory inven;
    public List<Item> items = new List<Item>();
    public int myItem = 0;

    public event Action OnChanged;

    public event Action<IReadOnlyList<Item>> OnItemsChanged;

    public static ItemManager Instance => instance == null ? null : instance;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }


    public void AddItem(Item newItem) => TryAddItem(newItem);
    public bool TryAddItem(Item newItem)
    {
        if (newItem == null) return false;
        if (items.Count >= capacity) { Debug.Log("Inventory full"); return false; }

        items.Add(newItem);
        NotifyChanged();
        return true;
    }

    void NotifyChanged() => OnItemsChanged?.Invoke(items);
    
    public void UseItem(Item item)
    {
        if (item == null) return;
        switch (item.itemIndex)
        {
            case 0: // 짝수 줄 점수 증가
                Debug.Log("짝수 줄 점수 증가 아이템 사용");
                break;

            case 1: // 줄 지우기 점수 2배
                Debug.Log("줄 지우기 점수 2배 아이템 사용");
                break;

            case 2: // 홀수 줄 점수 증가
                Debug.Log("홀수 줄 점수 증가 아이템 사용");
                break;

            case 3: // 콤보 점수 증가
                Debug.Log("콤보 점수 증가 아이템 사용");
                break;

            case 4: // 솎아내기
                Debug.Log("솎아내기 아이템 사용");
                break;

            case 5: // 짝수 타일 연속 배치
                Debug.Log("짝수 타일 연속 배치 아이템 사용");
                break;

            case 6: // 꿀조달
                Debug.Log("꿀조달 아이템 사용");
                break;

            case 7: // 홀수 타일 연속 배치
                Debug.Log("홀수 타일 연속 배치 아이템 사용");
                break;

            case 8: // 리롤
                Debug.Log("리롤 아이템 사용");
                break;

            case 9: // 성장
                Debug.Log("성장 아이템 사용");
                break;

            case 10: // 타일 배치 점수 증가
                Debug.Log("타일 배치 점수 증가 아이템 사용");
                break;

            case 11: // 턴 증가
                Debug.Log("턴 증가 아이템 사용");
                break;

            default:
                Debug.LogWarning($"알 수 없는 아이템 인덱스: {item.itemIndex}");
                break;
        }
        RemoveItem(item);
    }
    /// <summary>해당 참조 아이템 한 개 제거(동일 SO 여러 개면 처음 만나는 것 1개)</summary>
    public void RemoveItem(Item item)
    {
        if (item == null) return;

        int idx = items.IndexOf(item);
        if (idx >= 0)
        {
            items.RemoveAt(idx);
            NotifyChanged();
        }
    }

    /// <summary>모든 아이템 제거(패배/재시작용)</summary>
    public void ClearAllItems()
    {
        items.Clear();
        NotifyChanged();
    }
    public void myItemPlusOne()
    {
        myItem++;
    }
}

