using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform gridTransform;
    [SerializeField] Image itemImagePrefab;   // 반드시 UI용 프리팹(Image+RectTransform)

    readonly List<Image> pool = new();
    bool subscribed;
    Coroutine waitCo;

    void OnEnable()
    {
        TrySubscribeAndRefresh();
        if (!subscribed)
            waitCo = StartCoroutine(WaitAndSubscribe());
    }

    void OnDisable()
    {
        if (waitCo != null) { StopCoroutine(waitCo); waitCo = null; }
        if (subscribed && ItemManager.Instance != null)
            ItemManager.Instance.OnItemsChanged -= Refresh;
        subscribed = false;
    }

    void TrySubscribeAndRefresh()
    {
        if (ItemManager.Instance == null) return;
        if (!subscribed)
        {
            ItemManager.Instance.OnItemsChanged += Refresh;
            subscribed = true;
        }
        // 패널이 켜질 때, “현재 보유 아이템”으로 즉시 1회 렌더
        Refresh(ItemManager.Instance.items);
    }

    IEnumerator WaitAndSubscribe()
    {
        while (ItemManager.Instance == null) yield return null;
        TrySubscribeAndRefresh();
        waitCo = null;
    }

    public void Refresh(IReadOnlyList<Item> items)
    {
        // 풀 확장
        while (pool.Count < items.Count)
            pool.Add(Instantiate(itemImagePrefab, gridTransform));

        int i = 0;
        for (; i < items.Count; i++)
        {
            var img = pool[i];
            var btn = img.GetComponent<Button>() ?? img.gameObject.AddComponent<Button>();
            btn.targetGraphic = img;

            var item = items[i];    // 로컬 캡쳐

            img.gameObject.SetActive(true);
            img.sprite = item.icon;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => ItemManager.Instance.UseItem(item)); // 사용→삭제→Refresh 재호출
        }

        for (; i < pool.Count; i++)
            pool[i].gameObject.SetActive(false);
    }
}