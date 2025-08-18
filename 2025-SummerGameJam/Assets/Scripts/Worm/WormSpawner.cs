using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WormSpawner : MonoBehaviour
{
    // 각 타입별 가중치 관리
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
        // 기본 가중치 = 1
        weights = new Dictionary<eWormType, int>();
        foreach (eWormType type in Enum.GetValues(typeof(eWormType)))
        {
            if (type == eWormType.None) continue;
            weights[type] = 1;
        }
    }

    
    void OnEnable()
    {
        // 턴이 시작될 때마다 자동 스폰
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
    }/* ===================== 이벤트 핸들러 ===================== */

    // 턴이 시작될 때 호출됨 → 이번 턴의 3마리 스폰
    void OnTurnStartedHandler(int stage, int remainingTurns)
    {
        SpawnNewSet(unconditional: true); // 턴 시작 스폰(턴 소모 X)
    }
    /// <summary>
    /// 외부(UI 리롤 버튼 등)에서 호출: 남은 리롤이 있을 때만 즉시 새 3마리로 갱신
    /// (턴은 소모하지 않음)
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

        // 리롤 1 소모
        SM.AddRerolls(-1);

        // 리롤로 인한 Refresh에서 '소비' 카운트 억제 요청
        DrawManager.Instance?.SuppressConsumeOnce();

        // 새 3마리로 갱신(턴 소모 X)
        SpawnNewSet(unconditional: true);
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
    /// <summary>
    /// 벌레 1개를 "소비"했다고 알려주는 콜백.
    /// 각 WormTile에서 실제 배치/사용 시점에 이 메서드를 호출해줘야 함.
    /// </summary>
    public static void NotifyWormConsumed()
    {
        if (consumedCount >= maxNotifyCount) return;

        consumedCount++;
        Debug.Log($"벌레 소비됨: {consumedCount} / {maxNotifyCount}");

        if (consumedCount == maxNotifyCount)
        {
            Debug.Log("벌레 3마리 다 먹음!");
            // 여기에 보상 로직 or 다음 단계 로직
        }
    }
    public static void ResetConsumedCount()
    {
        consumedCount = 0;
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

    /// <summary>
    /// 내부 공용 스폰 함수.
    /// - unconditional=true : (턴 시작/리롤) 그냥 스폰만 한다. 턴 소모 X.
    /// - unconditional=false: (원하면) 스폰 시 턴 소모까지 처리하도록 확장 가능.
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

            // ★ WormTile이 사용/배치되었을 때 스포너에 알려주도록 연결
            // 아래의 OnConsumed(또는 적절한 이벤트/콜백)을 WormTile에 구현해줘야 함.
            // 예) public Action OnConsumed;  사용 시점에 OnConsumed?.Invoke();
            wt.OnConsumed += NotifyWormConsumed;
        }

        // 이번 턴에서 제공되는 남은 개수(리롤도 새 3마리 제공)
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
