using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;
using Inventory;
using Player.Inventory.Items;
using Player.Wallet;
using UnityEngine;

namespace Player.Inventory
{
	[CreateAssetMenu(menuName = "2DSurvGame/Global/Inventory", fileName = "Inventory")]
	public class InventorySO : ScriptableObject
	{
		[SerializeField] private InventoryConfigurationSO configuration;
		[SerializeField] private List<SlotInfoExtended> slots;
		public IReadOnlyList<SlotInfo> Slots => slots;
			
		[field: SerializeField] public int ActiveSlotsCount { get; private set; }

		[SerializeField] private EventSO buySlotEvent;
		[SerializeField] private InventorySlotAdd inventorySlotAddEvent;
		[SerializeField] private WalletSO wallet;
		
		public float TotalWeight { get; private set; }
		[SerializeField] private EventSO<float, float> totalWeightChangedEvent;
		[SerializeField] private EventSO<SlotInfo> slotUpdated;

		private void OnValidate()
		{
			for (var index = 0; index < slots.Count; index++)
			{
				slots[index].Index = index;
				slotUpdated.Invoke(slots[index]);
			}

			RecalculateWeight();
		}

		private void Awake()
		{
			ActiveSlotsCount = slots.Count;
			for (var index = 0; index < slots.Count; index++)
			{
				slots[index].Index = index;
			}
		}

		private void OnEnable()
		{
			buySlotEvent.Subscribe(BuySlot, 100);
		}
		
		private void OnDisable()
		{
			buySlotEvent.Unsubscribe(BuySlot);
		}

		private void BuySlot()
		{
			if (wallet.TrySpendCoins(configuration.AdditionalSlotCost))
			{
				if (ActiveSlotsCount + 1 > slots.Count && slots.Count < configuration.InventoryMaxSize)
				{
					slots.Add(new());
				}
				else
				{
					Debug.LogError($"The inventory already has the maximum number of slots.");
					return;
				}
				slots[ActiveSlotsCount].Reset();
				inventorySlotAddEvent.Invoke(slots[ActiveSlotsCount], ActiveSlotsCount);
				ActiveSlotsCount++;
			}
		}

		public bool TryAddItem(AbstractItemBaseSO abstractItemBase, int count)
		{
			StringBuilder logMessage = new StringBuilder();
			int itemsToPlace = count;
			if (abstractItemBase.CanStack)
			{
				foreach (var slotInfo in slots.Where(i => i.AbstractItemBase == null || i.AbstractItemBase == abstractItemBase).OrderBy(i => abstractItemBase != null))
				{
					var cashback = slotInfo.Place(abstractItemBase, itemsToPlace);
					logMessage.Append($"{abstractItemBase.Name}x{count-cashback.count} placed into slot {slotInfo.Index}\n");
					itemsToPlace = cashback.count;
					slotUpdated.Invoke(slotInfo);
					if(cashback.count == 0) break;
				}
			}
			else
			{
				foreach (var slotInfo in slots.Where(i => i.AbstractItemBase == null))
				{
					logMessage.Append($"{abstractItemBase.Name} placed into slot {slotInfo.Index}\n");
					slotInfo.Place(abstractItemBase);
					slotUpdated.Invoke(slotInfo);
					itemsToPlace--;
					if (itemsToPlace == 0) break;
				}
			}

			Debug.Log(logMessage);
			if (itemsToPlace > 0)
			{
				Debug.LogError("There is no more space in the inventory.");
				return false;
			}

			if (itemsToPlace != count)
			{
				RecalculateWeight();
			}

			return true;
		}

		public bool TrySpendConsumables(ConsumablesSO consumable, int count)
		{
			bool result = false;
			var slotsWithConsumables = slots.
									   Where(i => i.AbstractItemBase != null && string.Equals(i.AbstractItemBase.Name, consumable.Name)).ToList();
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
				RecalculateWeight();
			}

			return result;
		}

		public bool IsSlotActive(int index)
		{
			return index < ActiveSlotsCount;
		}

		public bool IsSlotActive(SlotInfoExtended slotInfo)
		{
			return IsSlotActive(slots.IndexOf(slotInfo));
		}

		public void RemoveItem(AbstractItemBaseSO equipment)
		{
			int index = -1;
			SlotInfo slotInfo = null;
			for (int i = 0; i < slots.Count; i++)
			{
				if (ReferenceEquals(slots[i].AbstractItemBase, equipment))
				{
					index = i;
					slotInfo = slots[i];
				}
			}
			if (slotInfo == null)
			{
				Debug.LogError("There is no such item in the inventory.");
			}
			else
			{
				slotInfo.Reset();
				slotUpdated.Invoke(slotInfo);
				RecalculateWeight();
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
				Debug.Log($"{slot.AbstractItemBase.Name}x{slot.Count} was removed from slot {slot.Index}");
				slot.Reset();
				slotUpdated.Invoke(slot);
				RecalculateWeight();
			}
		}

		private void RecalculateWeight()
		{
			float newWeight = 0;
			foreach (var slotInfo in slots)
			{
				newWeight += slotInfo.AbstractItemBase == null ? 0 : slotInfo.AbstractItemBase.Weight * slotInfo.Count;
			}

			if (Math.Abs(newWeight - TotalWeight) > 0.00001)
			{
				var previousWeight = TotalWeight;
				TotalWeight = newWeight;
				totalWeightChangedEvent.Invoke(previousWeight, newWeight);
			}
		}
	}

	[Serializable]
	public class SlotInfoExtended : SlotInfo
	{
		public new int Index { get => index; set => index = value;}
	}
	
	[Serializable]
	public class SlotInfo
	{
		protected int index;
		public int Index => index;
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
					Count = AbstractItemBase.MaxItemsInStack;
					result = (AbstractItemBase, Count + count - AbstractItemBase.MaxItemsInStack);
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
				Count = 0;
				result = (AbstractItemBase, count-Count);
				AbstractItemBase = null;
			}
			else
			{
				Count -= count;
				result = (AbstractItemBase, 0);
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