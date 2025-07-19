using System.Collections.Generic;
using Events;
using Gameplay;
using Inventory;
using Player.Inventory;
using Player.Inventory.Items;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddItemController : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;
    [SerializeField] private EventSO addItemButton;
    [SerializeField] private string addressablesGroup;
    private IList<EquipmentBaseSO> _assets;

    private async void Awake()
    {
        _assets = await AssetsLoader.LoadAll<EquipmentBaseSO>(addressablesGroup);
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
        inventory.TryAddItem(_assets[Random.Range(0, _assets.Count)], 1);
    }
}
