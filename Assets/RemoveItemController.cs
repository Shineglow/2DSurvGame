using System.Linq;
using Events;
using Inventory;
using Player.Inventory;
using Player.Inventory.Items;
using UnityEngine;

public class RemoveItemController : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;
    [SerializeField] private EventSO addItemButton;

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
        var equipments = inventory.Slots.Where(i => i.GetType() == typeof(EquipmentBaseSO)).ToList();
        var index = Random.Range(0, equipments.Count);
        var slot = equipments[index];
        for (var i = 0; i < inventory.Slots.Count; i++)
        {
            if (!ReferenceEquals(inventory.Slots[i], slot)) continue;
            inventory.RemoveItem(i);
            break;
        }
    }
}
