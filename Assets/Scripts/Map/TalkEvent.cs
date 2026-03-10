using UnityEngine;
using static GameManager;

public class TalkEvent : MonoBehaviour
{
    [SerializeField] private int NpcId;
    private DialogueManager dialogueManager;
    public Rigidbody2D rb;
    public Transform target;
    public bool Talkable=false;
    public float dist;
    private void Start()
    {
        dialogueManager = GameManager.Instance.dialogueManager;
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
            if (dist < 40)
            {
                Talkable = true;
            }
            else
            {
                Talkable = false;
            }
        }
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && GameManager.Instance.CurrentMode == GameMode.Field && Talkable)
        {
            UIManager.Active?.TalkingEventStart();
            dialogueManager.StartDialogue(NpcId);
        }
    }
}
