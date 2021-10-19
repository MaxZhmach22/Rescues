using System.Collections.Generic;
using UnityEngine;

internal sealed class Player : MonoBehaviour
{
    [SerializeField] private string _playerName;
    private List<string> _inventory = new List<string>();

    public string PlayerName => _playerName;

    public List<string> Inventory => _inventory;

    public void AddItemInventory(string item)
    {
        if (!Inventory.Contains(item))
        {
            Inventory.Add(item);
        }
    }
}
