using UnityEngine;

namespace Properties.Modifiers
{
	[CreateAssetMenu(menuName = "2DSurvGame/Properties/BaseAddictionModifierAction", fileName = "BaseAddictionModifierAction")]
	public class BaseAddictionModifierActionSO : PropertyBaseModifyActionSO
	{
		public override float ModifyValue(float baseValue, float modifiedBaseValue, float modifierValue)
		{
			return modifiedBaseValue + modifierValue;
		}
	}
}