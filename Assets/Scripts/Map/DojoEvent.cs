using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class DojoEvent : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform target;
    public UIManager uiManager;
    private bool Talkable=false;
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Dojo")
        {
            Player player = Object.FindFirstObjectByType<Player>();
            if (player != null)
            {
                target = player.transform;
                Vector2 TargetPos = target.position;
                Vector2 pos = rb.position;
                Vector2 toPlayer = TargetPos - pos;
                float dist = toPlayer.magnitude;
                if (dist < 20)
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

            }
        }
    }
}
