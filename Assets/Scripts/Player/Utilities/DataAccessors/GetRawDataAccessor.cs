using Unity.VisualScripting;
using UnityEngine;

namespace Player.Utilities.DataAccessors
{
	public abstract class GetRawDataAccessor<T> : ScriptableObject
	{
		public abstract T GetData();
	}
}