// ==================================================
// File: C:\Users\34007\CodeQuest\Assets\Scripts\Map\EnemySymbol.cs
// ==================================================
using UnityEngine;

/// <summary>
/// 【敵シンボルクラス】
/// マップ上に配置する敵オブジェクトにアタッチします。
/// プレイヤーとの接触でバトルを開始します。
/// </summary>
public class EnemySymbol : MonoBehaviour
{
    // CSVに登録されている敵ID
    public int EnemyID = 1;

    private bool isDefeated = false;

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("colid");
        // プレイヤーと接触し、まだ倒されていない場合
        if (col.gameObject.CompareTag("Player") && !isDefeated)
        {
            Debug.Log("battle");
            // バトル開始
            GameManager.Instance.GetComponent<BattleManager>()
                .StartBattle(GameManager.Instance.player, GameManager.Instance.neto, EnemyID);

            // 倒されたことにする (バトル終了処理で設定を戻すか、オブジェクトを非表示にする)
            // isDefeated = true; 
        }
    }
}