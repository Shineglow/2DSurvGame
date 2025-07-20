using Properties;
using Properties.Modifiers;
using UnityEngine;

namespace Player.Properties
{
	[CreateAssetMenu(menuName = "2DSurvGame/Properties/PropertyValueModifier", fileName = "PropertyValueModifier")]
	public class PropertyValueModifier : PropertyModifierBase
	{
		[field: SerializeField] public PropertyValueModifyActionSO ModifierAction { get; private set; }
	}
}