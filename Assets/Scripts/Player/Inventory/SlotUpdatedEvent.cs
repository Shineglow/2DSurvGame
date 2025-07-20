using Events;
using UnityEngine;

namespace Player.Inventory
{
	[CreateAssetMenu(menuName = "2DSurvGame/Inventory/SlotUpdateEvent", fileName = "SlotUpdatedEvent")]
	public class SlotUpdatedEvent : EventSO<SlotInfo>
	{
		
	}
}