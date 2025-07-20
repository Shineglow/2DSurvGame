using System.Collections.Generic;
using Events;
using Gameplay;
using Player.Inventory;
using Player.Inventory.Items;
using UnityEngine;

public class AddBulletsController : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;
    [SerializeField] private EventSO addItemButton;
    [SerializeField] private string addressablesGroup;
    [SerializeField] private int count;
    private IList<AmmoSO> _assets;

    private async void Awake()
    {
        _assets = await AssetsLoader.LoadAll<AmmoSO>(addressablesGroup);
    }

    private void OnEnable()
    {
        addItemButton.Subscribe(AddItemAction, 1);
    }

    private void OnDisable()
    {
        addItemButton.Unsubscribe(AddItemAction);
    }
    
    private void AddItemAction()
    {
        foreach (var ammoSo in _assets)
        {
            inventory.TryAddItem(ammoSo, count);
        }
    }
}
