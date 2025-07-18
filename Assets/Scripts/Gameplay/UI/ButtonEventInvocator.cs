using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Buttons
{
	public class ButtonEventInvocator : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private EventSO buttonPressEvent;
		
		private void OnEnable()
		{
			button.onClick.AddListener(buttonPressEvent.Invoke);
		}

		private void OnDisable()
		{
			button.onClick.RemoveListener(buttonPressEvent.Invoke);
		}
	}
}