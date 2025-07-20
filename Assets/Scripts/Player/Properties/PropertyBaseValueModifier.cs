using Properties;
using Properties.Modifiers;
using UnityEngine;

namespace Player.Properties
{
	[CreateAssetMenu(menuName = "2DSurvGame/Properties/PropertyBaseValueModifier", fileName = "PropertyBaseValueModifier")]
	public class PropertyBaseValueModifier : PropertyModifierBase
	{
		[field: SerializeField] public PropertyBaseModifyActionSO ModifierAction { get; private set; }
	}
}