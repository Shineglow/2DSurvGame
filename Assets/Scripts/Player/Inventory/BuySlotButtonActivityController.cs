using Events;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

public class BuySlotButtonActivityController : MonoBehaviour
{
    [SerializeField] private InventoryConfigurationSO inventoryConfiguration;
    [SerializeField] private Button button;
    [SerializeField] private EventSO<int, int> walletBalanceChangedEvent;

    private void OnEnable()
    {
        walletBalanceChangedEvent.Subscribe(OnCoinsAmountChanged, 1);
    }

    private void OnDisable()
    {
        walletBalanceChangedEvent.Unsubscribe(OnCoinsAmountChanged);
    }
    
    private void OnCoinsAmountChanged(int oldValue, int newValue)
    {
        button.interactable = newValue < inventoryConfiguration.AdditionalSlotCost;
    }
}
