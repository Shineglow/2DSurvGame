using System.Collections.Generic;
using Properties;
using UnityEngine;

namespace Player.Inventory.Items
{
	public abstract class AbstractItemBaseSO : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public Sprite Sprite { get; private set; }
		[field: SerializeField] public bool CanStack { get; private set; }
		[field: SerializeField] public int MaxItemsInStack { get; private set; }
		[field: SerializeField] public float Weight { get; private set; }
		[SerializeField] private List<PropertyModifierBase> _propertiesModifiers;
		public IReadOnlyList<PropertyModifierBase> PropertiesModifiers => _propertiesModifiers;
	}
}