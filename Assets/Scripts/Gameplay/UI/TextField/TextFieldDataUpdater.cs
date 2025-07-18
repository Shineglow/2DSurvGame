using System;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gameplay.UI.TextField
{
	public abstract class TextFieldDataUpdater<T> : MonoBehaviour
	{
		[SerializeField] private string textTemplate;
		[SerializeField] private TextMeshProUGUI textField;
		[SerializeField] private EventSO<T, T> dataEvent;

		private void OnEnable()
		{
			dataEvent.Subscribe(Validate, 1);
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
