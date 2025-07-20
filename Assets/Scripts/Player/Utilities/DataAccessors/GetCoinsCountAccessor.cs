using Player.Wallet;
using UnityEngine;

namespace Player.Utilities.DataAccessors
{
	[CreateAssetMenu(menuName = "2DSurvGame/Player/Wallet/GetCoinsCountAccessor", fileName = "GetCoinsCountAccessor")]
	public class GetCoinsCountAccessor : GetRawDataAccessor<int>
	{
		[SerializeField] private WalletSO wallet;
		
		public override int GetData()
		{
			return wallet.CoinsCount;
		}
	}
}