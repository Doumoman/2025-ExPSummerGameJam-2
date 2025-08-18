using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WormSpawner : MonoBehaviour
{
    // �� Ÿ�Ժ� ����ġ ����
    public GameObject wormTile;
    [FormerlySerializedAs("wormContainer")] public Transform WormContainer;
    public List<Transform> spawnPoint;
    
    private Dictionary<eWormType, int> weights;

    const int WormsPerTurn = 3;
    int remainingThisTurn = 0;

    private static int consumedCount = 0;
    private static int maxNotifyCount = 3;

    ScoreManager SM => ScoreManager.Instance;

    public WormSpawner()
    {
        // �⺻ ����ġ = 1
        weights = new Dictionary<eWormType, int>();
        foreach (eWormType type in Enum.GetValues(typeof(eWormType)))
        {
            if (type == eWormType.None) continue;
            weights[type] = 1;
        }
    }

    
    void OnEnable()
    {
        // ���� ���۵� ������ �ڵ� ����
        if (SM != null)
        {
            SM.OnTurnStarted += OnTurnStartedHandler;
        }
    }
    void OnDisable()
    {
        if (SM != null)
        {
            SM.OnTurnStarted -= OnTurnStartedHandler;
        }
    }/* ===================== �̺�Ʈ �ڵ鷯 ===================== */

    // ���� ���۵� �� ȣ��� �� �̹� ���� 3���� ����
    void OnTurnStartedHandler(int stage, int remainingTurns)
    {
        SpawnNewSet(unconditional: true); // �� ���� ����(�� �Ҹ� X)
    }
    /// <summary>
    /// �ܺ�(UI ���� ��ư ��)���� ȣ��: ���� ������ ���� ���� ��� �� 3������ ����
    /// (���� �Ҹ����� ����)
    /// </summary>
    /// 
    public void TryRerollSpawn()
    {
        if (SM == null) return ;
        if (SM.GetReroll() <= 0)
        {
            Debug.Log("[WormSpawner] Reroll failed: no rerolls left.");
            return ;
        }

        // ���� 1 �Ҹ�
        SM.AddRerolls(-1);

        // ���ѷ� ���� Refresh���� '�Һ�' ī��Ʈ ���� ��û
        DrawManager.Instance?.SuppressConsumeOnce();

        // �� 3������ ����(�� �Ҹ� X)
        SpawnNewSet(unconditional: true);
    }
    /// <summary>
    /// Ư�� Ÿ���� ����ġ�� ���� (1 �̻�)
    /// </summary>
    public void SetWeight(eWormType type, int newWeight)
    {
        if (newWeight < 1) newWeight = 1; // �ּ� 1
        weights[type] = newWeight;
    }
    /// <summary>
    /// ����ġ ������� �ߺ� ���� 3�� ����
    /// </summary>
    public List<eWormType> PickThree()
    {
        var available = new Dictionary<eWormType, int>(weights);
        var result = new List<eWormType>();

        for (int i = 0; i < 3 && available.Count > 0; i++)
        {
            // ��ü ����ġ ��
            int totalWeight = 0;
            foreach (var w in available.Values) totalWeight += w;

            // ������ ����
            int rand = UnityEngine.Random.Range(0, totalWeight);
            foreach (var kvp in available)
            {
                rand -= kvp.Value;
                if (rand < 0)
                {
                    result.Add(kvp.Key);
                    available.Remove(kvp.Key); // �ߺ� ����
                    break;
                }
            }
        }

        return result;
    }
    /// <summary>
    /// ���� 1���� "�Һ�"�ߴٰ� �˷��ִ� �ݹ�.
    /// �� WormTile���� ���� ��ġ/��� ������ �� �޼��带 ȣ������� ��.
    /// </summary>
    public static void NotifyWormConsumed()
    {
        if (consumedCount >= maxNotifyCount) return;

        consumedCount++;
        Debug.Log($"���� �Һ��: {consumedCount} / {maxNotifyCount}");

        if (consumedCount == maxNotifyCount)
        {
            Debug.Log("���� 3���� �� ����!");
            // ���⿡ ���� ���� or ���� �ܰ� ����
        }
    }
    public static void ResetConsumedCount()
    {
        consumedCount = 0;
    }
    public WormInfo GetWormInfo(eWormType wormType)
    {
        // eWormType�� �̸��� ������ ���ϸ� �˻�
        string path = $"WormTileInfo/{wormType}"; // Resources ���� ���� ���

        WormInfo info = Resources.Load<WormInfo>(path);
        if (info == null)
        {
            Debug.LogError($"[GetWormInfo] {path} ��ο��� WormInfo�� ã�� �� �����ϴ�.");
        }

        return info;
    }

    /// <summary>
    /// ���� ���� ���� �Լ�.
    /// - unconditional=true : (�� ����/����) �׳� ������ �Ѵ�. �� �Ҹ� X.
    /// - unconditional=false: (���ϸ�) ���� �� �� �Ҹ���� ó���ϵ��� Ȯ�� ����.
    /// </summary>
    public void SpawnNewSet(bool unconditional = true)
    {
        RemoveWorm();

        List<eWormType> worms = PickThree();
        for (int i = 0; i < worms.Count; i++)
        {
            GameObject worm = Instantiate(wormTile, Vector3.zero, Quaternion.identity, WormContainer);

            var wt = worm.GetComponent<WormTile>();
            wt.Initialize(GetWormInfo(worms[i]), spawnPoint[i].position, worms[i]);

            // �� WormTile�� ���/��ġ�Ǿ��� �� �����ʿ� �˷��ֵ��� ����
            // �Ʒ��� OnConsumed(�Ǵ� ������ �̺�Ʈ/�ݹ�)�� WormTile�� ��������� ��.
            // ��) public Action OnConsumed;  ��� ������ OnConsumed?.Invoke();
            wt.OnConsumed += NotifyWormConsumed;
        }

        // �̹� �Ͽ��� �����Ǵ� ���� ����(���ѵ� �� 3���� ����)
        remainingThisTurn = WormsPerTurn;
    }

    private void RemoveWorm()
    {
        if (WormContainer == null) return;

        for (int i = WormContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = WormContainer.GetChild(i);
            if (child != null)
                Destroy(child.gameObject);
        }
    }
}
