using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [Header("������ Ǯ�� �־�� �����۵�")]
    [SerializeField] private List<Item> allItems = new();   // �ĺ��� (�ߺ� ���� ���� ���)

    [Header("�̹� ������ ������ ����")]
    [SerializeField] private int offerCount = 4;            // �̹��� ������ ī�� ��

    [Header("UI")]
    [SerializeField] private Transform gridParent;          // Grid Layout Group�� ���� �θ�
    [SerializeField] private ShopItemCard cardPrefab;       // ī�� ������ (Bind(Item) ����)

    private readonly List<ShopItemCard> pool = new();
    private readonly List<Item> currentOffers = new();

    private void OnEnable()
    {
        ResetSession();
        RollOffers();
        Render();
    }

    // (��ư ������) ���� ������ �� ȣ��
    public void ReRoll()
    {
        RollOffers();
        Render();
    }

    private void RollOffers()
    {
        currentOffers.Clear();

        if (allItems == null || allItems.Count == 0)
        {
            Debug.LogWarning("[ShopUI] allItems�� ����ֽ��ϴ�.");
            return;
        }

        // �ߺ� ���� �̱�: �ĺ� ���纻���� �������� �̰� ����
        int n = Mathf.Clamp(offerCount, 0, allItems.Count); // �ĺ����� ������ �ڵ� ���
        var bag = new List<Item>(allItems);

        for (int i = 0; i < n; i++)
        {
            int idx = UnityEngine.Random.Range(0, bag.Count);
            currentOffers.Add(bag[idx]);
            bag.RemoveAt(idx); // �����Ͽ� �ߺ� ����
        }
    }

    private void Render()
    {
        while (pool.Count < currentOffers.Count)
        {
            var card = Instantiate(cardPrefab, gridParent);
            pool.Add(card);
        }

        int i = 0;
        for (; i < currentOffers.Count; i++)
        {
            var card = pool[i];
            pool[i].gameObject.SetActive(true);
            card.ResetForNewSession();
            pool[i].Bind(currentOffers[i]); // ������/�̸�/���� ǥ�õ�
        }
        for (; i < pool.Count; i++)
            pool[i].gameObject.SetActive(false);
    }
    private void ResetSession()
    {
        // Ǯ�� �ִ� ī��� ���־� ����
        foreach (var card in pool)
        {
            if (card) card.ResetForNewSession();
        }
        currentOffers.Clear();
    }
}
