using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
	[CreateAssetMenu(menuName = "2DSurvGame/Events/Event", fileName = "Event")]
	public class EventSO : ScriptableObject
	{
		private class Listener : IComparable<Listener>
		{
			public int Priority { get; }
			public Action Callback { get; }

			public Listener(int priority, Action callback)
			{
				Priority = priority;
				Callback = callback;
			}

			public int CompareTo(Listener other)
			{
				int cmp = -Priority.CompareTo(other.Priority);
				if (cmp == 0)
					cmp = Callback.GetHashCode().CompareTo(other.Callback.GetHashCode());
				return cmp;
			}
		}

		private readonly SortedSet<Listener> _listeners = new();

		public void Subscribe(Action callback, int priority)
		{
			_listeners.Add(new Listener(priority, callback));
		}

		public void Unsubscribe(Action callback)
		{
			_listeners.RemoveWhere(l => l.Callback == callback);
		}

		public void Invoke()
		{
			foreach (var listener in _listeners)
			{
				listener.Callback?.Invoke();
			}
		}
	}
}
