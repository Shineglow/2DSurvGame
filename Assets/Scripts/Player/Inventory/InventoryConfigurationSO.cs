using UnityEngine;

namespace Inventory
{
	[CreateAssetMenu(menuName = "2DSurvGame/Configuration/InventoryConfiguration", fileName = "InventoryConfiguration")]
	public class InventoryConfigurationSO : ScriptableObject
	{
		[field: SerializeField] public int InventoryMaxSize { get; private set; }
		[field: SerializeField] public int DefaultSize { get; private set; }
		[field: SerializeField] public int AdditionalSlotCost { get; private set; }
	}
}