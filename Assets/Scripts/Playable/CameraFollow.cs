using UnityEngine;

/// <summary>
/// カメラがターゲット（プレイヤー）を滑らかに追従するクラス。
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position;
            targetPos.z = -10f; // 2DカメラのZ位置

            // 現在位置から目標位置へ少しずつ移動させる（線形補間）
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
        }
    }
}