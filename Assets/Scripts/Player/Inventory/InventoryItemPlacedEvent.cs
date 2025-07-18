using Player.Inventory.Items;
using UnityEngine;

namespace Events
{
	[CreateAssetMenu(menuName = "2DSurvGame/Events/InventoryItemPlacedEvent")]
	public class InventoryItemPlacedEvent : EventSO<AbstractItemBaseSO, int, int>{}
}