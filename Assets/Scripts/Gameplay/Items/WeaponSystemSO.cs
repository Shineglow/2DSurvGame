using Inventory;
using Player.Inventory.Items;
using UnityEngine;

namespace Gameplay.Items
{
	public class WeaponSystemSO : ScriptableObject
	{
		[SerializeField] private InventorySO inventory;
		
		public bool UseWeapon(WeaponBaseSO weapon)
		{
			bool result;
			if (inventory.TrySpendConsumables(weapon.AmmoType, weapon.AmmoPerUse))
			{
				Debug.Log($"The {weapon.Name} weapon used. Attacks {weapon.AttacksCount} times.");
				result = true;
			}
			else
			{
				Debug.LogError($"Out of ammo {weapon.AmmoType.Name}");
				result = false;
			}
			return result;
		}
	}
}