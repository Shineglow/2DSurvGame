using UnityEngine;

namespace Player.Inventory.Items
{
	[CreateAssetMenu(menuName = "2DSurvGame/Items/WeaponBase", fileName = "WeaponBase")]
	public class WeaponBaseSO : EquipmentBaseSO
	{
		[field: SerializeField] public float Damage { get; private set; }
		[field: SerializeField] public AmmoSO AmmoType { get; private set; }
		[field: SerializeField] public int AttacksCount { get; private set; }
		[field: SerializeField] public int AmmoPerUse { get; private set; }
	}
}