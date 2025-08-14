using System;
using System.Collections.Generic;
using UnityEngine;

public class WormSpawner : MonoBehaviour
{
    // �� Ÿ�Ժ� ����ġ ����
    public GameObject wormTile;
    public Transform wormContainer;
    public List<Transform> spawnPoint;
    
    private Dictionary<eWormType, int> weights;

    public WormSpawner()
    {
        // �⺻ ����ġ = 1
        weights = new Dictionary<eWormType, int>();
        foreach (eWormType type in Enum.GetValues(typeof(eWormType)))
        {
            weights[type] = 1;
        }
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
    
    public void SpawnWorm()
    {
        List<eWormType> worms = PickThree();
        
        for (int i = 0; i < 3; i++)
        {
            GameObject worm = Instantiate(wormTile, Vector3.zero, Quaternion.identity, wormContainer);
            worm.GetComponent<WormTile>().Initialize(GetWormInfo(worms[i]), spawnPoint[i].position);
        }
    }

    private void RemoveWorm()
    {
        foreach (Transform child in wormContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
