using Events;
using Player.Wallet;
using UnityEngine;

public class AddCoinsController : MonoBehaviour
{
    [SerializeField] private EventSO buttonPressed;
    [SerializeField] private WalletSO wallet;
    [SerializeField] private int addCoinsCount;

    private void OnEnable()
    {
        buttonPressed.Subscribe(AddCoins, 1);
    }

    private void OnDisable()
    {
        buttonPressed.Unsubscribe(AddCoins);
    }
    
    private void AddCoins()
    {
        var coinsAdd = wallet.TryAddCoins(addCoinsCount, out var difference);
        if (!coinsAdd || difference != 0)
        {
            Debug.LogWarning($"Wallet full of gold!");
        }
    }
}
