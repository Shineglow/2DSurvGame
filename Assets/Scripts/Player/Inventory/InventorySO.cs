using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Player.Inventory;
using Player.Inventory.Items;
using Player.Wallet;
using UnityEngine;

namespace Inventory
{
	[CreateAssetMenu(menuName = "2DSurvGame/Global/Inventory", fileName = "Inventory")]
	public class InventorySO : ScriptableObject
	{
		[SerializeField] private InventoryConfigurationSO _configuration;
		[SerializeField] private List<SlotInfo> slots;
		public IReadOnlyList<SlotInfo> Slots => slots;
			
		[field: SerializeField] public int ActiveSlotsCount { get; private set; }

		[SerializeField] private EventSO _buySlotEvent;
		[SerializeField] private InventorySlotAdd _inventorySlotAddEvent;
		[SerializeField] private WalletSO _wallet;
		
		public float TotalWeight { get; private set; }
		[SerializeField] private EventSO<float, float> totalWeightChangedEvent;

		private void Awake()
		{
			_buySlotEvent.Subscribe(BuySlot, 100);
			ActiveSlotsCount = _configuration.DefaultSize;
		}

		private void BuySlot()
		{
			if (_wallet.TrySpendCoins(_configuration.AdditionalSlotCost))
			{
				slots[ActiveSlotsCount].Reset();
				_inventorySlotAddEvent.Invoke(slots[ActiveSlotsCount], ActiveSlotsCount);
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
	}

	[Serializable]
	public class SlotInfo
	{
		[field: SerializeField] public AbstractItemBaseSO AbstractItemBase { get; private set;}
		[field: SerializeField] public int Count { get; private set;}

		public event Action SlotUpdated;

		public (AbstractItemBaseSO item, int count) Place((AbstractItemBaseSO item, int count) items)
		{
			return Place(items.item, items.count);
		}
		
		public (AbstractItemBaseSO item, int count) Place(AbstractItemBaseSO abstractItem, int count = 1)
		{
			(AbstractItemBaseSO item, int count) result = (null, -1);
			if (AbstractItemBase.GetType() == abstractItem.GetType() && AbstractItemBase.CanStack)
			{
				if (Count + count > AbstractItemBase.MaxItemsInStack)
				{
					result = (AbstractItemBase, Count + count % AbstractItemBase.MaxItemsInStack);
				}
				else
				{
					Count += count;
				}
			}
			else
			{
				result = (AbstractItemBase, Count);
				Count = Count;
				AbstractItemBase = abstractItem;
			}
			SlotUpdated?.Invoke();
			return result;
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
			SlotUpdated?.Invoke();
			return result;
		}
		
		public void Reset()
		{
			AbstractItemBase = null;
			Count = 0;
		}
	}
}