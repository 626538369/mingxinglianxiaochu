using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ISubscriber
{
    System.Action<object[]> Handler
    {
        set;
    }
    void Unsubscribe();
}

public static class EventManager
{
    static Dictionary<string, List<Subscriber>> ms_subscribers = new Dictionary<string, List<Subscriber>>();

    private class Subscriber : ISubscriber
    {
        string m_subscribeKey;
        System.Action<object[]> m_handler;

        public Subscriber(string key)
        {
            m_subscribeKey = key;
        }

        ~Subscriber()
        {
            if (string.IsNullOrEmpty(m_subscribeKey))
                return;
            Unsubscribe();
        }


        public System.Action<object[]> Handler
        {
            set { m_handler = value; }
        }

        public void Notify(params object[] args)
        {
            if (m_handler != null)
                m_handler(args);
        }

        public void Unsubscribe()
        {
            MWDebug.Log("Unsubscribe:" + m_subscribeKey);
            List<Subscriber> sublist = null;
            if (!ms_subscribers.TryGetValue(m_subscribeKey, out sublist))
            {
                return;
            }
            sublist.Remove(this);
            if (sublist.Count == 0)
            {
                ms_subscribers.Remove(m_subscribeKey);
                MWDebug.Log("Subscriber removed!");
            }
            m_subscribeKey = null;
        }
    }

    public static ISubscriber Subscribe(string name)
    {
        List<Subscriber> sublist = null;
        if (!ms_subscribers.TryGetValue(name, out sublist))
        {
            sublist = new List<Subscriber>();
            ms_subscribers.Add(name, sublist);
        }

        Subscriber sub = new Subscriber(name);
        sublist.Add(sub);
        return sub;
    }
	
	public static void UnsubscribeAll()
	{
		foreach (List<Subscriber> list in ms_subscribers.Values)
		{
			list.Clear();
		}
		ms_subscribers.Clear();
	}

    private static void Notify(string name, params object[] args)
    {
        List<Subscriber> sublist = null;
        if (!ms_subscribers.TryGetValue(name, out sublist))
        {
            return;
        }

        Subscriber[] subs = sublist.ToArray();
        foreach (var sub in subs)
        {
            if(!sublist.Contains(sub))
                continue;
            //MWDebug.Log("Notify:" + name);
            sub.Notify(args);
        }
    }

    public abstract class Publisher
    {
        public abstract string Name
        {
            get;
        }

        protected void Notify(string name, params object[] args)
        {
            EventManager.Notify(Name + ":" + name, args);
        }

    }
}



