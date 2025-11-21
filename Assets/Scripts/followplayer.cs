using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    /*Vector3 prePlayerPos;:*/   // 前フレームでのプレイヤーの座標位置

    void LateUpdate()
    {
        /*if (player.transform.position != prePlayerPos)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            prePlayerPos = player.transform.position;
        }*/
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}