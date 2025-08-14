using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WormInfo", menuName = "ScriptableObjects/WormInfo")]
public class WormInfo : ScriptableObject
{
    [System.Serializable]
    public struct WormPos
    {
        public int x;
        public int y;
    }

    [SerializeField]
    private List<WormPos> wormPosList;

    [SerializeField] public Sprite WormSprite;

    public List<WormPos> WormPosList => wormPosList;
}