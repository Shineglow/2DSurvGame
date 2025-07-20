using System.Collections.Generic;
using Player.Properties;
using UnityEngine;

namespace Properties
{
	[CreateAssetMenu(menuName = "2DSurvGame/Properties/Property", fileName = "Property")]
	public class PropertySO : ScriptableObject
	{
		[field: SerializeField] public float BaseValue { get; private set; }
		[field: SerializeField] public float Value { get; private set; }
		[SerializeField] protected List<PropertyBaseValueModifier> _baseModifiers;
		[SerializeField] protected List<PropertyValueModifier> _valueModifiers;

		private void OnValidate()
		{
			UpdateValue();
		}

		protected void UpdateValue()
		{
			float flatBase = BaseValue;
			foreach (var mod in _baseModifiers)
			{
				flatBase = mod.ModifierAction.ModifyValue(BaseValue, flatBase, mod.ModifierValue);
			}

			float actualValue = flatBase;
			foreach (var mod in _valueModifiers)
			{
				actualValue = mod.ModifierAction.ModifyValue(flatBase, actualValue, mod.ModifierValue);
			}

			Value = actualValue;
		}

		public void AddModifier(PropertyModifierBase modifierBase)
		{
			switch (modifierBase)
			{
				case PropertyBaseValueModifier baseMod:
					_baseModifiers.Add(baseMod);
					break;
				case PropertyValueModifier valueMod:
					_valueModifiers.Add(valueMod);
					break;
			}

			UpdateValue();
		}
	}
}