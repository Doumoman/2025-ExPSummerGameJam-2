using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance = null;
    [SerializeField] int capacity = 5;                 // �κ��丮 �ִ�ġ
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
            case 0: // ¦�� �� ���� ����
                Debug.Log("¦�� �� ���� ���� ������ ���");
                break;

            case 1: // �� ����� ���� 2��
                Debug.Log("�� ����� ���� 2�� ������ ���");
                break;

            case 2: // Ȧ�� �� ���� ����
                Debug.Log("Ȧ�� �� ���� ���� ������ ���");
                break;

            case 3: // �޺� ���� ����
                Debug.Log("�޺� ���� ���� ������ ���");
                break;

            case 4: // �ԾƳ���
                Debug.Log("�ԾƳ��� ������ ���");
                break;

            case 5: // ¦�� Ÿ�� ���� ��ġ
                Debug.Log("¦�� Ÿ�� ���� ��ġ ������ ���");
                break;

            case 6: // ������
                Debug.Log("������ ������ ���");
                break;

            case 7: // Ȧ�� Ÿ�� ���� ��ġ
                Debug.Log("Ȧ�� Ÿ�� ���� ��ġ ������ ���");
                break;

            case 8: // ����
                Debug.Log("���� ������ ���");
                break;

            case 9: // ����
                Debug.Log("���� ������ ���");
                break;

            case 10: // Ÿ�� ��ġ ���� ����
                Debug.Log("Ÿ�� ��ġ ���� ���� ������ ���");
                break;

            case 11: // �� ����
                Debug.Log("�� ���� ������ ���");
                break;

            default:
                Debug.LogWarning($"�� �� ���� ������ �ε���: {item.itemIndex}");
                break;
        }
        RemoveItem(item);
    }
    /// <summary>�ش� ���� ������ �� �� ����(���� SO ���� ���� ó�� ������ �� 1��)</summary>
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

    /// <summary>��� ������ ����(�й�/����ۿ�)</summary>
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

