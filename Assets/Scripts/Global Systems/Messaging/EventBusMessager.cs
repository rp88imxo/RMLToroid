using UnityEngine.Events;
using System.Collections.Generic;
using System;


namespace RML.Messaging
{
public class EventBusMessager : Singleton<EventBusMessager>
{
    private Dictionary<string, List<Action<IPayload>>> events;

    public override void Awake()
    {
        base.Awake();
        Instance.Init();
    }

    private void Init()
    {
        Instance.events = new Dictionary<string, List<Action<IPayload>>>();
    }

    public void AddListener(string eventName, Action<IPayload> listenerMethod)
    {
        if (!Instance.events.ContainsKey(eventName))
        {
            Instance.events.Add(eventName, new List<Action<IPayload>>());
        }

        Instance.events[eventName].Add(listenerMethod);
    }

    public void RemoveListener(string eventName, Action<IPayload> listenerMethod)
    {
        if (Instance.events.ContainsKey(eventName))
        {
            Instance.events[eventName].Remove(listenerMethod);
        }
    }

    public void PublishEvent(IPayload payload)
    {
        var eventName = payload.EventName;
        if (Instance.events.ContainsKey(eventName))
        {
           Instance.events[eventName].ForEach(method => method.Invoke(payload));
        }
    }
}
}