using Events;
using Inventory;
using UnityEngine;

namespace Player.Inventory
{
	/// <summary>
	/// first argument is slot info class; second argument slot index;
	/// </summary>
	[CreateAssetMenu(menuName = "2DSurvGame/Inventory/InventorySlotAddEvent", fileName = "InventorySlotAddEvent")]
	public class InventorySlotAdd : EventSO<SlotInfo, int>{}
}