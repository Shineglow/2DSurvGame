using System.Collections.Generic;
using Events;
using Inventory;
using Player.Inventory;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;
    
    [SerializeField] private RectTransform contentTransform;

    [SerializeField] private List<RectTransform> slots;
    [SerializeField] private EventSO buySlotEvent;
    
    private void TryBuySlots()
    {
        Debug.Log("The slot has been purchased");
    }

    private void OnEnable()
    {
        OrderSlots();

        Subscribes();
    }

    private void OrderSlots()
    {
        int lastInactive = -1;
        for (var i = 0; i < slots.Count; i++)
        {
            if (slots[i].gameObject.activeSelf) continue;
            lastInactive = i;
            break;
        }
        for (var i = lastInactive; i < slots.Count; i++)
        {
            if (!slots[i].gameObject.activeSelf)
            {
                lastInactive = i;
            }
            else
            {
                (slots[i], slots[lastInactive]) = (slots[lastInactive], slots[i]);
            }
        }
    }

    private void OnDisable()
    {
        Unsubscribes();
    }

    private void Subscribes()
    {
        buySlotEvent.Subscribe(TryBuySlots, 1);
        // inventory.
    }
    
    private void Unsubscribes()
    {
        buySlotEvent.Unsubscribe(TryBuySlots);
    }
}
