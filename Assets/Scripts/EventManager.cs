using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

/* 
    A system to manage custom events. All custom events will be identified by a 
    human readable string of characters, and objects with reference to this class
    can register a listener with this class. Then, in the XML script for the
    text engine, can use those string names and invoke methods using those, thus
    triggering events in the game world.
 */

class EventManager : MonoBehaviour
{
    [SerializeField] string[] event_names;
    Dictionary<string, UnityEvent> events;

    void Awake()
    {
        events = new Dictionary<string, UnityEvent>();
        foreach (string n in event_names)
        {
            events.Add(n, new UnityEvent());
        }
    }

    void AddListener(string name, UnityAction call)
    {
        if (!events.ContainsKey(name)) { return; }
        events[name].AddListener(call);
    }   

    void RemoveListener(string name, UnityAction call)
    {
        if (!events.ContainsKey(name)) { return; }
        events[name].RemoveListener(call);
    }

    void Invoke(string name)
    {
        events[name].Invoke();
    }

}