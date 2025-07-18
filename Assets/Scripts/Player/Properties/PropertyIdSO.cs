using UnityEngine;

namespace Properties
{
	[CreateAssetMenu(menuName = "2DSurvGame/Properties/PropertyId", fileName = "PropertyId")]
	public class PropertyIdSO : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; }
	}
}