using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform gridTransform;
    [SerializeField] Image itemImagePrefab;   // �ݵ�� UI�� ������(Image+RectTransform)

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
        // �г��� ���� ��, ������ ���� �����ۡ����� ��� 1ȸ ����
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
        // Ǯ Ȯ��
        while (pool.Count < items.Count)
            pool.Add(Instantiate(itemImagePrefab, gridTransform));

        int i = 0;
        for (; i < items.Count; i++)
        {
            var img = pool[i];
            var btn = img.GetComponent<Button>() ?? img.gameObject.AddComponent<Button>();
            btn.targetGraphic = img;

            var item = items[i];    // ���� ĸ��

            img.gameObject.SetActive(true);
            img.sprite = item.icon;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => ItemManager.Instance.UseItem(item)); // ���������Refresh ��ȣ��
        }

        for (; i < pool.Count; i++)
            pool[i].gameObject.SetActive(false);
    }
}