using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Inventory;
using Player.Inventory.Items;
using Player.Wallet;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Inventory
{
	[CreateAssetMenu(menuName = "2DSurvGame/Global/Inventory", fileName = "Inventory")]
	public class InventorySO : ScriptableObject
	{
		[SerializeField] private InventoryConfigurationSO configuration;
		[SerializeField] private List<SlotInfo> slots;
		public IReadOnlyList<SlotInfo> Slots => slots;
			
		[field: SerializeField] public int ActiveSlotsCount { get; private set; }

		[SerializeField] private EventSO buySlotEvent;
		[SerializeField] private InventorySlotAdd inventorySlotAddEvent;
		[SerializeField] private WalletSO wallet;
		
		public float TotalWeight { get; private set; }
		[SerializeField] private EventSO<float, float> totalWeightChangedEvent;
		[SerializeField] private EventSO<SlotInfo> slotUpdated;

		private void Awake()
		{
			buySlotEvent.Subscribe(BuySlot, 100);
			ActiveSlotsCount = configuration.DefaultSize;
		}

		private void BuySlot()
		{
			if (wallet.TrySpendCoins(configuration.AdditionalSlotCost))
			{
				slots[ActiveSlotsCount].Reset();
				inventorySlotAddEvent.Invoke(slots[ActiveSlotsCount], ActiveSlotsCount);
				ActiveSlotsCount++;
			}
		}

		public bool TryAddItem(AbstractItemBaseSO abstractItemBase, int count)
		{
			int itemsToPlace = count;
			if (abstractItemBase.CanStack)
			{
				foreach (var slotInfo in slots.Where(i => i.AbstractItemBase == null || i.AbstractItemBase == abstractItemBase).OrderBy(i => abstractItemBase != null))
				{
					var cashback = slotInfo.Place(abstractItemBase, itemsToPlace);
					itemsToPlace = cashback.count;
					if(cashback.count == 0) break;
				}
			}
			else
			{
				foreach (var slotInfo in slots.Where(i => abstractItemBase == null))
				{
					slotInfo.Place(abstractItemBase);
					slotUpdated.Invoke(slotInfo);
					itemsToPlace--;
				}
			}

			if (itemsToPlace > 0)
			{
				Debug.LogError("There is no more space in the inventory.");
				return false;
			}

			return true;
		}

		public bool TrySpendConsumables(ConsumablesSO consumable, int count)
		{
			bool result = false;
			var slotsWithConsumables = slots.Where(i => i.AbstractItemBase.GetType() == consumable.GetType()).ToList();
			if (slotsWithConsumables.Count < count)
			{
				Debug.LogError($"Not enough consumables of type {consumable.Name}");
			}
			else
			{
				(AbstractItemBaseSO item, int count) cashback = (consumable, count);
				foreach (var slotInfo in slotsWithConsumables)
				{
					cashback = slotInfo.Get(cashback.count);
					slotUpdated.Invoke(slotInfo);
					if (cashback.count == 0)
					{
						break;
					}
				}

				result = true;
			}

			return result;
		}

		public bool IsSlotActive(int index)
		{
			return index < ActiveSlotsCount;
		}

		public bool IsSlotActive(SlotInfo slotInfo)
		{
			return IsSlotActive(slots.IndexOf(slotInfo));
		}

		public void RemoveItem(AbstractItemBaseSO equipment)
		{
			var slot = slots.FirstOrDefault(i => ReferenceEquals(i.AbstractItemBase, equipment));
			if (slot == null)
			{
				Debug.LogError("There is no such item in the inventory.");
			}
			else
			{
				slot.Reset();
				slotUpdated.Invoke(slot);
			}
		}
		
		public void RemoveItem(int index)
		{
			var slot = slots.ElementAtOrDefault(index);
			if (slot == null)
			{
				Debug.LogError("Index out of range. Can not find inventory slot.");
			}
			else
			{
				slot.Reset();
				slotUpdated.Invoke(slot);
			}
		}
	}

	[Serializable]
	public class SlotInfo
	{
		[field: SerializeField] public AbstractItemBaseSO AbstractItemBase { get; private set;}
		[field: SerializeField] public int Count { get; private set;}

		public (AbstractItemBaseSO item, int count) Place((AbstractItemBaseSO item, int count) items)
		{
			return Place(items.item, items.count);
		}
		
		public (AbstractItemBaseSO item, int count) Place(AbstractItemBaseSO abstractItem, int count = 1)
		{
			(AbstractItemBaseSO item, int count) result = (null, 0);
			if (AbstractItemBase == null)
			{
				AbstractItemBase = abstractItem;
				result = ValueTuple();
			}
			else if (AbstractItemBase.GetType() == abstractItem.GetType() && AbstractItemBase.CanStack)
			{
				result = ValueTuple();
			}
			else
			{
				result = (AbstractItemBase, Count);
				Count = Count;
				AbstractItemBase = abstractItem;
			}
			return result;

			(AbstractItemBaseSO item, int count) ValueTuple()
			{
				if (Count + count > AbstractItemBase.MaxItemsInStack)
				{
					result = (AbstractItemBase, AbstractItemBase.MaxItemsInStack - (Count + count));
				}
				else
				{
					Count += count;
				}

				return result;
			}
		}

		public (AbstractItemBaseSO item, int count) Get(int count = 1)
		{
			(AbstractItemBaseSO item, int count) result;
			if (count >= Count)
			{
				result = (AbstractItemBase, Count);
				AbstractItemBase = null;
			}
			else
			{
				result = (AbstractItemBase, count);
			}
			return result;
		}
		
		public void Reset()
		{
			AbstractItemBase = null;
			Count = 0;
		}
	}
}