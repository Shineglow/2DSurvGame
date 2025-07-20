using System.Collections.Generic;
using System.Linq;
using Events;
using Gameplay.UI.Inventory;
using Inventory;
using Player.Inventory;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventoryConfigurationSO configuration;
    [SerializeField] private InventorySO inventory;

    [SerializeField] private List<SlotView> slots;
    [SerializeField] private EventSO<SlotInfo, int> inventorySlotAddEvent;

    [SerializeField] private Transform slotsParent;
    [SerializeField] private SlotView slotPrefab;

    private void TryBuySlots(SlotInfo slotInfo, int index)
    {
        if (index + 1 > configuration.InventoryMaxSize) return;
        var slot = slots.ElementAtOrDefault(index);
        if (slot == null)
        {
            slot = CreateSlot(index);
        }
        else
        {
            slot.gameObject.SetActive(true);
        }
        slot.Bind(slotInfo);
    }

    private SlotView CreateSlot(int index)
    {
        var slotView = Instantiate(slotPrefab, slotsParent, false);
        slotView.transform.SetSiblingIndex(index);
        return slotView;
    }

    private void OnEnable()
    {
        BindAll();
        Subscribes();
    }

    private void OnDisable()
    {
        UnbindAll();
        Unsubscribes();
    }

    private void BindAll()
    {
        for (var i = 0; i < slots.Count; i++)
        {
            var slotView = slots[i];
            if (inventory.ActiveSlotsCount > i)
            {
                slotView.gameObject.SetActive(true);
                var slotInfo = inventory.Slots[i];
                slotView.Bind(slotInfo);
            }
            else
            {
                slotView.gameObject.SetActive(false);
            }
        }
    }

    private void UnbindAll()
    {
        for (var i = 0; i < slots.Count; i++)
        {
            var slotView = slots[i];
            slotView.Bind(null);
        }
    }

    private void Subscribes()
    {
        inventorySlotAddEvent.Subscribe(TryBuySlots, 1);
    }

    private void Unsubscribes()
    {
        inventorySlotAddEvent.Unsubscribe(TryBuySlots);
    }
}
