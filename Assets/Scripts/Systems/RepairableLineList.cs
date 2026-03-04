using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RepairableLineList : MonoBehaviour
{
    public Dictionary<int, bool> RepairableLineTable = new Dictionary<int, bool>();

    public void CreateRepairableLine(int lineId)
    {
        RepairableLineTable[lineId] = false;
    }

    public void RepairLine(int lineId)
    {
        if (RepairableLineTable.ContainsKey(lineId))
        {
            RepairableLineTable[lineId] = true;
        }
    }

    public bool IsRepairable(int lineId)
    {
        if (RepairableLineTable.ContainsKey(lineId))
        {
            return RepairableLineTable[lineId] == false;
        }
        return false;
    }

    public void LoadFromSaveData(int[] lineIds, bool[] repaired)
    {
        for (int i = 0; i < Mathf.Min(lineIds.Length, repaired.Length); i++)
        {
            if (RepairableLineTable.ContainsKey(lineIds[i]))
            {
                RepairableLineTable[lineIds[i]] = repaired[i];
            }
        }
    }
}
