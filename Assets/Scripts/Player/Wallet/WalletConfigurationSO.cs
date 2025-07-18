using UnityEngine;

namespace Player.Wallet
{
	[CreateAssetMenu(menuName = "2DSurvGame/Global/WalletConfiguration", fileName = "WalletConfiguration")]
	public class WalletConfigurationSO : ScriptableObject
	{
		[field: SerializeField] public int MaxCoins { get; private set; } = int.MaxValue;
	}
}