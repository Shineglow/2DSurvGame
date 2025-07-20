using System.Linq;
using Events;
using Gameplay.Items;
using Player.Inventory;
using Player.Inventory.Items;
using UnityEngine;

public class WeaponFireController : MonoBehaviour
{
    [SerializeField] private WeaponSystemSO weaponSystem;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private EventSO fireButtonEvent;

    private void OnEnable()
    {
        fireButtonEvent.Subscribe(OnFirePressed, 1);
    }

    private void OnDisable()
    {
        fireButtonEvent.Unsubscribe(OnFirePressed);
    }

    private void OnFirePressed()
    {
        var weapons = inventory.Slots.
                                Where(i => i.AbstractItemBase is WeaponBaseSO).
                                Select(i => i.AbstractItemBase).
                                ToList();
        int index = Random.Range(0, weapons.Count);
        var weapon = weapons.ElementAtOrDefault(index);
        if (weapon == null)
        {
            Debug.LogError($"No weapon in inventory!");
        }
        else
        {
            weaponSystem.UseWeapon(weapon as WeaponBaseSO);
        }
    }
}
