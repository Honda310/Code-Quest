using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyList : MonoBehaviour
{
    public Dictionary<int, bool> enemyDefeated = new Dictionary<int, bool>();
    public void EnemyRegist(int EnemyId)
    {
        enemyDefeated[EnemyId] = false;
    }
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
    ///<summary>
    ///撃破済に登録されたエネミーをセーブデータから復元するメソッド。
    ///宝箱のリスト(TreasureBoxListと違い、これはクリアしたリストに要素を追加する形になっている。
    ///なお、シーン開始時に対応するEnemyをDestoryする担当は、EnemySymbol.cs参照のこと
    ///</summary>
    public void LoadFromSaveData(int[] id, bool[]defeat)
    {
        enemyDefeated.Clear();
        for (int i=0;i<Mathf.Min(id.Length,defeat.Length);i++)
        {
            enemyDefeated.Add(id[i], defeat[i]);
        }
    }
}
