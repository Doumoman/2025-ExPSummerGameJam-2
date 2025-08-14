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
            case 0: // ¦�� �� ���� ����
                Debug.Log("¦�� �� ���� ���� ������ ���");
                myItemPlusOne();
                break;

            case 1: // �� ����� ���� 2��
                Debug.Log("�� ����� ���� 2�� ������ ���");
                break;

            case 2: // Ȧ�� �� ���� ����
                Debug.Log("Ȧ�� �� ���� ���� ������ ���");
                break;

            case 3: // �޺� ���� ����
                Debug.Log("�޺� ���� ���� ������ ���");
                myItemPlusOne();
                break;

            case 4: // �ԾƳ���
                Debug.Log("�ԾƳ��� ������ ���");
                break;

            case 5: // ¦�� Ÿ�� ���� ��ġ
                Debug.Log("¦�� Ÿ�� ���� ��ġ ������ ���");
                break;

            case 6: // ������
                Debug.Log("������ ������ ���");
                myItemPlusOne();
                break;

            case 7: // Ȧ�� Ÿ�� ���� ��ġ
                Debug.Log("Ȧ�� Ÿ�� ���� ��ġ ������ ���");
                break;

            case 8: // ����
                Debug.Log("���� ������ ���");
                break;

            case 9: // ����
                Debug.Log("���� ������ ���");
                myItemPlusOne();
                break;

            case 10: // Ÿ�� ��ġ ���� ����
                Debug.Log("Ÿ�� ��ġ ���� ���� ������ ���");
                break;

            case 11: // �� ����
                Debug.Log("�� ���� ������ ���");
                break;

            default:
                Debug.LogWarning($"�� �� ���� ������ �ε���: {itemIndex}");
                break;
        }
    }
    public void myItemPlusOne()
    {
        myItem++;
    }
}

