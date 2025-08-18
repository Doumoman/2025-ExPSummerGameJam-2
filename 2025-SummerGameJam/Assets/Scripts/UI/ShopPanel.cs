using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [Header("상점에 풀로 넣어둘 아이템들")]
    [SerializeField] private List<Item> allItems = new();   // 후보들 (중복 제거 추출 대상)

    [Header("이번 상점에 보여줄 개수")]
    [SerializeField] private int offerCount = 4;            // 이번에 보여줄 카드 수

    [Header("UI")]
    [SerializeField] private Transform gridParent;          // Grid Layout Group가 붙은 부모
    [SerializeField] private ShopItemCard cardPrefab;       // 카드 프리팹 (Bind(Item) 보유)

    private readonly List<ShopItemCard> pool = new();
    private readonly List<Item> currentOffers = new();

    private void OnEnable()
    {
        ResetSession();
        RollOffers();
        Render();
    }

    // (버튼 등으로) 수동 리롤할 때 호출
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
            Debug.LogWarning("[ShopUI] allItems가 비어있습니다.");
            return;
        }

        // 중복 없이 뽑기: 후보 복사본에서 랜덤으로 뽑고 제거
        int n = Mathf.Clamp(offerCount, 0, allItems.Count); // 후보보다 많으면 자동 축소
        var bag = new List<Item>(allItems);

        for (int i = 0; i < n; i++)
        {
            int idx = UnityEngine.Random.Range(0, bag.Count);
            currentOffers.Add(bag[idx]);
            bag.RemoveAt(idx); // 제거하여 중복 방지
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
            pool[i].Bind(currentOffers[i]); // 아이콘/이름/가격 표시됨
        }
        for (; i < pool.Count; i++)
            pool[i].gameObject.SetActive(false);
    }
    private void ResetSession()
    {
        // 풀에 있는 카드들 비주얼 리셋
        foreach (var card in pool)
        {
            if (card) card.ResetForNewSession();
        }
        currentOffers.Clear();
    }
}
