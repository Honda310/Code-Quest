using UnityEngine;
using System.Collections.Generic;

public class SavePointManager : MonoBehaviour
{
    public Rigidbody2D rb;
    private Transform target;
    private bool Savable=false;
    UIManager uiManager;
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
            if (dist < 20)
            {
                Savable = true;
            }
            else
            {
                Savable = false;
            }
        }
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && GameManager.Instance.CurrentMode == GameManager.GameMode.Field && Savable)
        {
            GameManager.Instance.SetMode(GameManager.GameMode.SaveLoad);
            uiManager.SavePanelEnable();
        }
        else if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)) && GameManager.Instance.CurrentMode == GameManager.GameMode.SaveLoad && Savable)
        {
            GameManager.Instance.SetMode(GameManager.GameMode.Field);
            uiManager.SavePanelDisable();
        }
    }
}
