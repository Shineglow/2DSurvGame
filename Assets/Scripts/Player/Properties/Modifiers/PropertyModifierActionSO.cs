using UnityEngine;

namespace Properties.Modifiers
{
	public abstract class PropertyValueModifyActionSO : ScriptableObject
	{
		public abstract float ModifyValue(float baseValue, float actualValue, float modifierValue);
	}
	
	
	public abstract class PropertyBaseModifyActionSO : ScriptableObject
	{
		public abstract float ModifyValue(float baseValue, float modifiedBase, float modifierValue);
	}
}