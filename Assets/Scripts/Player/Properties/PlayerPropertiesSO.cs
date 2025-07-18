using System;
using System.Collections.Generic;
using Properties.Modifiers;
using UnityEngine;

namespace Properties
{
	[CreateAssetMenu(menuName = "2DSurvGame/Properties/PlayerProperties", fileName = "PlayerProperties")]
	public class PlayerPropertiesSO : ScriptableObject
	{
		private Dictionary<PropertyIdSO, PropertySO> _properties;
		[SerializeField] protected List<IdAndProperty> _propertiesSerializable;

		private void OnValidate()
		{
			_properties ??= new();
			if (_properties.Count != _propertiesSerializable.Count)
			{
				foreach (var property in _propertiesSerializable)
				{
					_properties.TryAdd(property.Id, property.Property);
				}
			}
		}

		[Serializable]
		public struct IdAndProperty
		{
			[field: SerializeField] public PropertyIdSO Id { get; private set; }
			[field: SerializeField] public PropertySO Property { get; private set; }
		}
	}
}