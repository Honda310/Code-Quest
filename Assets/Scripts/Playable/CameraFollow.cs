using UnityEngine;

/// <summary>
/// カメラがターゲット（プレイヤー）を滑らかに追従するクラス。
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    public float maxY = 45f;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position;
            targetPos.z = -15f; // 2DカメラのZ位置
            Vector3 newPos = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
            // 現在位置から目標位置へ少しずつ移動させる（線形補間）

            newPos.y = Mathf.Min(newPos.y, maxY);
            newPos.x = Mathf.Round(newPos.x);
            newPos.y = Mathf.Round(newPos.y);
            transform.position = newPos;

        }
    }
}