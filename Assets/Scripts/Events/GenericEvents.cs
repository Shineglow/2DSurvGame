using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
	public class EventSO<T> : ScriptableObject
	{
		private class Listener<Y> : IComparable<Listener<Y>>
		{
			public int Priority { get; }
			public Action<Y> Callback { get; }

			public Listener(int priority, Action<Y> callback)
			{
				Priority = priority;
				Callback = callback;
			}

			public int CompareTo(Listener<Y> other)
			{
				int cmp = -Priority.CompareTo(other.Priority);
				if (cmp == 0)
					cmp = Callback.GetHashCode().CompareTo(other.Callback.GetHashCode());
				return cmp;
			}
		}

		private readonly SortedSet<Listener<T>> _listeners = new();

		public void Subscribe(Action<T> callback, int priority)
		{
			_listeners.Add(new Listener<T>(priority, callback));
		}

		public void Unsubscribe(Action<T> callback)
		{
			_listeners.RemoveWhere(l => l.Callback == callback);
		}

		public void Invoke(T arg1)
		{
			foreach (var listener in _listeners)
			{
				listener.Callback?.Invoke(arg1);
			}
		}
	}
	
	public class EventSO<T1, T2> : ScriptableObject
	{
		private class Listener<Y1, Y2> : IComparable<Listener<Y1, Y2>>
		{
			public int Priority { get; }
			public Action<Y1, Y2> Callback { get; }

			public Listener(int priority, Action<Y1, Y2> callback)
			{
				Priority = priority;
				Callback = callback;
			}

			public int CompareTo(Listener<Y1, Y2> other)
			{
				int cmp = -Priority.CompareTo(other.Priority);
				if (cmp == 0)
					cmp = Callback.GetHashCode().CompareTo(other.Callback.GetHashCode());
				return cmp;
			}
		}

		private readonly SortedSet<Listener<T1, T2>> _listeners = new();

		public void Subscribe(Action<T1, T2> callback, int priority)
		{
			_listeners.Add(new Listener<T1, T2>(priority, callback));
		}

		public void Unsubscribe(Action<T1, T2> callback)
		{
			_listeners.RemoveWhere(l => l.Callback == callback);
		}

		public void Invoke(T1 arg1, T2 arg2)
		{
			foreach (var listener in _listeners)
			{
				listener.Callback?.Invoke(arg1, arg2);
			}
		}
	}
	
	public class EventSO<T1, T2, T3> : ScriptableObject
	{
		private class Listener<Y1, Y2, Y3> : IComparable<Listener<Y1, Y2, Y3>>
		{
			public int Priority { get; }
			public Action<Y1, Y2, Y3> Callback { get; }

			public Listener(int priority, Action<Y1, Y2, Y3> callback)
			{
				Priority = priority;
				Callback = callback;
			}

			public int CompareTo(Listener<Y1, Y2, Y3> other)
			{
				int cmp = -Priority.CompareTo(other.Priority);
				if (cmp == 0)
					cmp = Callback.GetHashCode().CompareTo(other.Callback.GetHashCode());
				return cmp;
			}
		}

		private readonly SortedSet<Listener<T1, T2, T3>> _listeners = new();

		public void Subscribe(Action<T1, T2, T3> callback, int priority)
		{
			_listeners.Add(new Listener<T1, T2, T3>(priority, callback));
		}

		public void Unsubscribe(Action<T1, T2, T3> callback)
		{
			_listeners.RemoveWhere(l => l.Callback == callback);
		}

		public void Invoke(T1 arg1, T2 arg2, T3 arg3)
		{
			foreach (var listener in _listeners)
			{
				listener.Callback?.Invoke(arg1, arg2, arg3);
			}
		}
	}
}