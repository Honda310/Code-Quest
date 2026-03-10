using System.Collections.Generic;
using UnityEngine;

public class EventList : MonoBehaviour
{

    public Dictionary<int, bool> TriggeredEventTable = new Dictionary<int, bool>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            TriggeredEventTable[i] = false;
        }
    }
    public void Triggerd(int eventId)
    {
        if (TriggeredEventTable.ContainsKey(eventId))
        {
            TriggeredEventTable[eventId] = true;
        }
    }

    public bool IsTriggerable(int eventId)
    {
        if (TriggeredEventTable.ContainsKey(eventId))
        {
            return TriggeredEventTable[eventId] == false;
        }
        return false;
    }

    public void LoadFromSaveData(int[] eventIds, bool[] triggerd)
    {
        for (int i = 0; i < Mathf.Min(eventIds.Length, triggerd.Length); i++)
        {
            if (TriggeredEventTable.ContainsKey(eventIds[i]))
            {
                TriggeredEventTable[eventIds[i]] = triggerd[i];
            }
        }
    }
}
