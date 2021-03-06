﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventBroadcast : MonoBehaviour
{
	private Dictionary<Event, List<IEventSubscriber>> subscriptions = new Dictionary<Event, List<IEventSubscriber>>();

	public enum Event
	{
		OBSTACLE_UPDATED
	}

	public void SubscribeToEvent(Event _event, IEventSubscriber _subscriber)
	{
		if (!subscriptions.ContainsKey(_event))
		{
			List<IEventSubscriber> newList = new List<IEventSubscriber>();
			subscriptions.Add(_event, newList);
		}
		subscriptions[_event].Add(_subscriber);
	}

	public void TriggerEvent(Event _event)
	{
		if (subscriptions.ContainsKey(_event))
		{
			int numLoops = subscriptions[_event].Count;
			for (int i = 0; i < numLoops; i++)
			{
				IEventSubscriber s = subscriptions[_event][i];
				MonoBehaviour m = s as MonoBehaviour;
				if (s != null && m != null)
				{
					s.InformOfEvent(_event);
				}
				else
				{
					subscriptions[_event].RemoveAt(i);
				}
			}
		}
	}
}
