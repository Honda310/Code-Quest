using UnityEngine;

public class FloorAllHierarchy : MonoBehaviour
{
    void Start()
    {
        // 1. スクリプトがアタッチされている親オブジェクト自身を処理
        ApplyFloorTransformRecursively(this.transform);

        Debug.Log(gameObject.name + " とそのすべての子孫の位置とスケールを切り捨てました。");
    }

    /// <summary>
    /// 指定されたTransformとそのすべての子孫のTransformに切り捨て処理を適用します。
    /// </summary>
    /// <param name="targetTransform">処理を適用するTransform</param>
    private void ApplyFloorTransformRecursively(Transform targetTransform)
    {
        // -----------------------------------
        // 現在のオブジェクト（親、子、孫...）の処理
        // -----------------------------------

        // 1. ポジションの切り捨て処理
        Vector3 currentPosition = targetTransform.position;
        Vector3 flooredPosition = new Vector3(
            Mathf.Floor(currentPosition.x),
            Mathf.Floor(currentPosition.y),
            Mathf.Floor(currentPosition.z)
        );
        targetTransform.position = flooredPosition;

        // 2. スケールの切り捨て処理
        Vector3 currentScale = targetTransform.localScale;
        Vector3 flooredScale = new Vector3(
            Mathf.Floor(currentScale.x),
            Mathf.Floor(currentScale.y),
            Mathf.Floor(currentScale.z)
        );
        targetTransform.localScale = flooredScale;

        // -----------------------------------
        // 3. 再帰処理：さらにその子オブジェクトを処理する
        // -----------------------------------
        foreach (Transform child in targetTransform)
        {
            // 関数自身を呼び出し、下の階層へ移動
            ApplyFloorTransformRecursively(child);
        }
    }
}