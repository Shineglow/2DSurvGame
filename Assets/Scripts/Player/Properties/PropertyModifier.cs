using System;
using Properties.Modifiers;
using UnityEngine;

namespace Properties
{
	public class PropertyModifierBase : ScriptableObject
	{
		[field: SerializeField] public PropertyIdSO Property { get; private set; }
		[field: SerializeField] public float ModifierValue { get; private set; }
	}

	[CreateAssetMenu(menuName = "2DSurvGame/Properties/PropertyBaseValueModifier", fileName = "PropertyBaseValueModifier")]
	public sealed class PropertyBaseValueModifier : PropertyModifierBase
	{
		[field: SerializeField] public PropertyBaseModifyActionSO ModifierAction { get; private set; }
	}

	[CreateAssetMenu(menuName = "2DSurvGame/Properties/PropertyValueModifier", fileName = "PropertyValueModifier")]
	public sealed class PropertyValueModifier : PropertyModifierBase
	{
		[field: SerializeField] public PropertyValueModifyActionSO ModifierAction { get; private set; }
	}
}