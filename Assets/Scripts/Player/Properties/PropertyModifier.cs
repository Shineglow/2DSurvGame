using Properties;
using UnityEngine;

namespace Player.Properties
{
	public class PropertyModifierBase : ScriptableObject
	{
		[field: SerializeField] public PropertyIdSO Property { get; private set; }
		[field: SerializeField] public float ModifierValue { get; private set; }
	}
}