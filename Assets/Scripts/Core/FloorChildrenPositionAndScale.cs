using UnityEngine;

public class FloorChildrenPositionAndScale : MonoBehaviour
{
    // Start() で一度だけ実行
    void Start()
    {
        // アタッチされたオブジェクト自身（親）の子オブジェクトをすべて反復処理
        foreach (Transform child in transform)
        {
            // ===================================
            // 1. ポジションの切り捨て処理
            // ===================================
            Vector3 currentPosition = child.position;
            Vector3 flooredPosition = new Vector3(
                Mathf.Floor(currentPosition.x),
                Mathf.Floor(currentPosition.y),
                Mathf.Floor(currentPosition.z)
            );
            child.position = flooredPosition;

            // ===================================
            // 2. スケールの切り捨て処理
            // ===================================
            Vector3 currentScale = child.localScale;
            Vector3 flooredScale = new Vector3(
                Mathf.Floor(currentScale.x),
                Mathf.Floor(currentScale.y),
                Mathf.Floor(currentScale.z)
            );
            child.localScale = flooredScale;

            Debug.Log(child.name + " の位置とスケールを切り捨てました。");
        }

        // 注意：このスクリプトは親オブジェクト（自身）のTransformは変更しません。
    }
}