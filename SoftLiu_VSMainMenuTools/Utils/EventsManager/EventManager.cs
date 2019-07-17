﻿using SoftLiu_VSMainMenuTools.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Utils.EventsManager
{
    /// <summary>
    ///     Manages global events for application flow. 
    ///     NW: HSW-4073 - Reduced GC by:
    ///         1. Made EventManager generic, reducing the conversion to Enum. 
    ///         2. Made EventManager pooled param overloads to prevent object array creation. 
    ///         3. Converted Action invocation list to List{Action} to save GC allocs on Count query.  
    ///         4. Added a list pooler for point 3 (PoolFactory.cs).  
    /// </summary>
    /// <typeparam name="T">Event type (enum).</typeparam>
    public class EventManager<T> : AutoGeneratedSingleton<EventManager<T>> where T : struct
    {
        private readonly Dictionary<T, List<Action<T, object[]>>> m_events;

        public EventManager(int expectedEventTypes, int expectedRegisterCalls)
        {
            
        }

        public EventManager()
            : this(5, 5)
        {
        }

        // NW: We cannot use single instances of arrays of each length because Event triggers might trigger inside other event triggers.
        // Using a single array would cause it to get overwritten, corrupting it.
        // We also use 3 different array pools (rather than just passing in m_reusableParams3Pool with 2 null arguments)
        // because some use-cases validate the length of the params.
        private readonly object[] m_reusableParam0 = new object[0];

        public void RegisterEvent(T eventType, Action<T, object[]> listener)
        {
            List<Action<T, object[]>> list;
            if (!m_events.TryGetValue(eventType, out list))
            {
                list.Add(listener);
                m_events.Add(eventType, list);
            }
            else
            {
                if (!list.Contains(listener))
                {
                    list.Add(listener);
                }
            }
        }

        public void TriggerEvent(T eventType, params object[] optParams)
        {
            List<Action<T, object[]>> list;
            m_events.TryGetValue(eventType, out list);

            if (list != null)
            {
                if (list.Count == 0)
                {
                    m_events.Remove(eventType);
                    return;
                }

                foreach (var action in list)
                {
                    action(eventType, optParams);
                }
            }
        }

        public void DeregisterEvent(T eventType, Action<T, object[]> listener)
        {
            List<Action<T, object[]>> list;
            if (m_events.TryGetValue(eventType, out list))
            {
                list.Remove(listener);

                // NW: Really, we SHOULD log when we fail to remove, but it's spammy. 
                // Debug.LogWarning("EventManager (DeregisterEvent) :: (Harmless) Cannot remove listener as it's not in the list: " + eventType + ", " + listener.Target + "." + listener.Method);

                // Unregister the event itself and re-pool the list.
                // This should speed up event lookups and reduce memory.
                if (list.Count == 0)
                {
                    m_events.Remove(eventType);
                }
            }
        }

        public Dictionary<T, List<Action<T, object[]>>> GetRegisteredEvents()
        {
            return m_events;
        }
    }
}
