using Player.Inventory;
using UnityEngine;

namespace Player.Utilities.DataAccessors
{
	[CreateAssetMenu(menuName = "2DSurvGame/Inventory/GetTotalWeightAccessor", fileName = "GetTotalWeightAccessor")]
	public class GetTotalWeightAccessor : GetRawDataAccessor<float>
	{
		[SerializeField] private InventorySO inventory;
		
		public override float GetData()
		{
			return inventory.TotalWeight;
		}
	}
}