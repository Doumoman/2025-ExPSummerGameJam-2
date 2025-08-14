using System;
using System.Collections.Generic;
using UnityEngine;

public class WormSpawner : MonoBehaviour
{
    // 각 타입별 가중치 관리
    public GameObject wormTile;
    public Transform wormContainer;
    public List<Transform> spawnPoint;
    
    private Dictionary<eWormType, int> weights;

    public WormSpawner()
    {
        // 기본 가중치 = 1
        weights = new Dictionary<eWormType, int>();
        foreach (eWormType type in Enum.GetValues(typeof(eWormType)))
        {
            weights[type] = 1;
        }
    }

    /// <summary>
    /// 특정 타입의 가중치를 변경 (1 이상)
    /// </summary>
    public void SetWeight(eWormType type, int newWeight)
    {
        if (newWeight < 1) newWeight = 1; // 최소 1
        weights[type] = newWeight;
    }

    /// <summary>
    /// 가중치 기반으로 중복 없이 3개 선택
    /// </summary>
    public List<eWormType> PickThree()
    {
        var available = new Dictionary<eWormType, int>(weights);
        var result = new List<eWormType>();

        for (int i = 0; i < 3 && available.Count > 0; i++)
        {
            // 전체 가중치 합
            int totalWeight = 0;
            foreach (var w in available.Values) totalWeight += w;

            // 랜덤값 선택
            int rand = UnityEngine.Random.Range(0, totalWeight);
            foreach (var kvp in available)
            {
                rand -= kvp.Value;
                if (rand < 0)
                {
                    result.Add(kvp.Key);
                    available.Remove(kvp.Key); // 중복 방지
                    break;
                }
            }
        }

        return result;
    }

    public WormInfo GetWormInfo(eWormType wormType)
    {
        // eWormType의 이름과 동일한 파일명 검색
        string path = $"WormTileInfo/{wormType}"; // Resources 폴더 기준 경로

        WormInfo info = Resources.Load<WormInfo>(path);
        if (info == null)
        {
            Debug.LogError($"[GetWormInfo] {path} 경로에서 WormInfo를 찾을 수 없습니다.");
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
