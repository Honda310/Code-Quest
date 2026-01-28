using System.Collections;
using UnityEngine;
using static GameManager;

public class TreasureEvent : MonoBehaviour
{
    [SerializeField] private int TreasureId;
    //ê‘=1,ê¬=2,ì¡éÍ3Ç∆Ç∑ÇÈÇÊ
    [SerializeField] private int ColorId;
    [SerializeField] private UIManager uiManager;
    private TreasureBoxList treasureBoxList;
    public Rigidbody2D rb;
    private Transform target;
    private bool Skipable = false;
    private bool OpenAble = false;
    private void Start()
    {
        treasureBoxList = GameManager.Instance.treasureBoxList;
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
            float dist = toPlayer.magnitude;
            if (dist < 40)
            {
                OpenAble = true;
            }
            else
            {
                OpenAble = false;
            }
        }
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && GameManager.Instance.CurrentMode == GameMode.Talk && OpenAble && Skipable)
        {
            uiManager.TreasureTakeEventEnd();
            Skipable = false;
        }
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && GameManager.Instance.CurrentMode == GameMode.Field && OpenAble)
        {
            if (treasureBoxList.TreasureBoxTable[TreasureId].accessAble)
            {
                string itemname = treasureBoxList.OpenTreasureBox(TreasureId, ColorId);
                uiManager.TreasureTakeEventStart(itemname);
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Image/OnMapObject/{ColorId}_2treasureBox");
                StartCoroutine(SkipCooldown());
            }
        }
        IEnumerator SkipCooldown()
        {

            yield return new WaitForSeconds(0.15f);
            Skipable = true;
        }
    }
}
