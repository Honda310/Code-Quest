using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public Dictionary<int, bool> enemyDefeated = new Dictionary<int, bool>();
    public void EnemyListTake(Dictionary<int, EnemyData> dictionary)
    {
        foreach (int key in dictionary.Keys)
        {
            enemyDefeated[key] = false;
        }
    }
    public void EnemyDefeat(int key)
    {
        enemyDefeated[key] = true;
    }
    public bool isDefeated(int key)
    {
        if (enemyDefeated[key] == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void LoadFromSaveData(Dictionary<int, bool> dictionary)
    {
        foreach (int key in dictionary.Keys)
        {
            enemyDefeated[key] = dictionary[key];
        }
    }
}
