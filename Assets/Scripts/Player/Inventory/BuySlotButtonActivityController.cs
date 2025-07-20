using Events;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

public class BuySlotButtonActivityController : MonoBehaviour
{
    [SerializeField] private InventoryConfigurationSO inventoryConfiguration;
    [SerializeField] private Button button;
    [SerializeField] private EventSO<int, int> walletBalanceChangedEvent;
    [SerializeField] private EventSO buySlotEvent;

    private void OnEnable()
    {
        walletBalanceChangedEvent.Subscribe(OnCoinsAmountChanged, 1);
        button.onClick.AddListener(BuySlotInvoke);
    }

    private void OnDisable()
    {
        walletBalanceChangedEvent.Unsubscribe(OnCoinsAmountChanged);
        button.onClick.RemoveListener(BuySlotInvoke);
    }

    private void BuySlotInvoke()
    {
        buySlotEvent.Invoke();
    }

    private void OnCoinsAmountChanged(int oldValue, int newValue)
    {
        button.interactable = newValue > inventoryConfiguration.AdditionalSlotCost;
    }
}
