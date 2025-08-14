using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance = null;

    public event Action OnChanged;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public List<Item> items = new List<Item>();

    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        UpdateUI(newItem);
    }

    public void UpdateUI(Item newItem)
    {

    }
}

