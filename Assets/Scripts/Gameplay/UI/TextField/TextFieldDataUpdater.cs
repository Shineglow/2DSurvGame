using Events;
using Player.Utilities.DataAccessors;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.TextField
{
	public abstract class TextFieldDataUpdater<T> : MonoBehaviour
	{
		[SerializeField] private string textTemplate;
		[SerializeField] private TextMeshProUGUI textField;
		[SerializeField] private EventSO<T, T> dataEvent;
		[SerializeField] private GetRawDataAccessor<T> rawDataAccessor;

		private void OnEnable()
		{
			dataEvent.Subscribe(Validate, 1);
			Validate(default, rawDataAccessor.GetData());
		}

		private void OnDisable()
		{
			dataEvent.Unsubscribe(Validate);
		}

		private void Validate(T oldValue, T newValue)
		{
			textField.text = string.Format(textTemplate, newValue);
		}
	}
}
