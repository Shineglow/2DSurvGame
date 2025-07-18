using System;
using Properties.Modifiers;
using UnityEngine;

namespace Properties
{
	[Serializable]
	public class PropertyModifierBase
	{
		[field: SerializeField] public PropertyIdSO Property { get; private set; }
		[field: SerializeField] public float ModifierValue { get; private set; }
	}

	[Serializable]
	public sealed class PropertyBaseValueModifier : PropertyModifierBase
	{
		[field: SerializeField] public PropertyBaseModifyActionSO ModifierAction { get; private set; }
	}

	[Serializable]
	public sealed class PropertyValueModifier : PropertyModifierBase
	{
		[field: SerializeField] public PropertyValueModifyActionSO ModifierAction { get; private set; }
	}
}