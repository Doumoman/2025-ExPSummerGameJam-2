using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BoardCell : MonoBehaviour
{
    public int x;
    public int y;

    public bool bIsOccupied = false;

    void OnMouseDown()
    {
        Debug.Log($"{x}, {y}");
    }
}