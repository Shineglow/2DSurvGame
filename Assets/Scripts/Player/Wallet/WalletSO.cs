using Cysharp.Threading.Tasks;
using Events;
using UnityEngine;

namespace Player.Wallet
{
	[CreateAssetMenu(menuName = "2DSurvGame/Global/Wallet", fileName = "Wallet")]
	public class WalletSO : ScriptableObject
	{
		[field: SerializeField] private WalletConfigurationSO configuration;
		[field: SerializeField, Min(0)] public int CoinsCount { get; private set; }
		private int _oldCoins = -1;
		[SerializeField] private GenericIntegerIntegerEvent walletBalanceChangedEvent;
		
		private void OnValidate()
		{
			CoinsChanged();
		}

		/// <summary>
		/// Adds coins up to the maximum the wallet can hold. Returns the difference if more is added than the wallet can hold.
		/// </summary>
		/// <param name="count">amount of coins to add</param>
		/// <param name="difference"> coins that did not fit into the wallet</param>
		/// <returns>False if no coins were included; otherwise, true.</returns>
		public bool TryAddCoins(int count, out int difference)
		{
			bool result;
			
			if (CoinsCount == configuration.MaxCoins)
			{
				difference = count;
				result = false;
			}
			else
			{
				if (count + CoinsCount <= configuration.MaxCoins)
				{
					CoinsCount += count;
					difference = 0;
				}
				else
				{
					difference = CoinsCount + count - configuration.MaxCoins;
					CoinsCount = configuration.MaxCoins;
				}
				CoinsChanged();
				result = true;
			}

			return result;
		}
		
		/// <summary>
		/// Spends coins if there are enough. Returns true on success, false otherwise.
		/// </summary>
		public bool TrySpendCoins(int spendCount)
		{
			bool result = false;
			if (spendCount <= CoinsCount)
			{
				CoinsCount -= spendCount;
				result = true;
				CoinsChanged();
			}

			return result;
		}
		
		private void CoinsChanged()
		{
			if (_oldCoins != CoinsCount)
			{
				walletBalanceChangedEvent.Invoke(_oldCoins, CoinsCount);
				_oldCoins = CoinsCount;
			}
		}
	}
}