using UnityEngine;

/// <summary>
/// 壊れた道（コード）のギミッククラスです。
/// </summary>
public class RepairableLine : MonoBehaviour
{
    public bool Repaired = false;

    public void Repair()
    {
        Repaired = true;
        gameObject.SetActive(false); // 通れるようにオブジェクトを消す
    }
}