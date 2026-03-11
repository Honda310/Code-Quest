using UnityEngine;
using static GameManager;

/// <summary>
/// 壊れた道（コード）のギミッククラスです。
/// </summary>
public class RepairableLine : MonoBehaviour
{
    [SerializeField] private int lineId;
    private RepairableLineList repairableLineList;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject BugMapTile;
    public Rigidbody2D rb;
    public Transform target;
    public float dist;
    private bool Repairable = false;
    
    void Start()
    {
        repairableLineList = GameManager.Instance.repairableLineList;
        if (repairableLineList.RepairableLineTable[lineId])
        {
            Repair();
        }
    }
    void Update()
    {
        Player player = Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            target = player.transform;
            Vector2 TargetPos = target.position;
            Vector2 pos = rb.position;
            Vector2 toPlayer = TargetPos - pos;
            dist = toPlayer.magnitude;
            if (dist < 80)
            {
                Repairable = true;
            }
            else
            {
                Repairable = false;
            }
        }
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && GameManager.Instance.CurrentMode == GameMode.Field && Repairable)
        {
            uiManager.DebugStart();
        }
    }
    public void Repair()
    {
        BugMapTile.SetActive(false);
    }
}