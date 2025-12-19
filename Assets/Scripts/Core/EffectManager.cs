using UnityEngine;
using System;

/// <summary>
/// 【効果処理】
/// アイテムを使ったときの具体的な効果（回復やバフ）を実行するクラスです。
/// </summary>
public class EffectManager : MonoBehaviour
{
    public void ApplyEffect(int effectId, Player player, int value)
    {
        // 設計書の「効果ID」に応じた処理
        switch (effectId)
        {
            case 1: // HP回復
                // 最大HPを超えないように回復量を計算
                int heal = Math.Min(value, player.MaxHP - player.CurrentHP);
                player.CurrentHP += heal;
                //GameManager.Instance.uiManager.ShowLog($"HPが {heal} 回復しました。");
                break;

            case 2: // 攻撃力アップ
                player.ApplyTemporaryAtk(value);
                //GameManager.Instance.uiManager.ShowLog($"攻撃力が一時的に {value} 上がりました！");
                break;

            case 3: // 防御力アップ
                player.ApplyTemporaryDef(value);
                //GameManager.Instance.uiManager.ShowLog($"防御力が一時的に {value} 上がりました！");
                break;

            case 99: // バフ解除（デバッグ完了など）
                player.ClearBuffs();
                //GameManager.Instance.uiManager.ShowLog("ステータス変化が元に戻りました。");
                break;
        }
    }
    public void ApplyEffect(int effectId, Neto neto, int value)
    {
        // 設計書の「効果ID」に応じた処理
        switch (effectId)
        {
            case 1: // HP回復
                // 最大HPを超えないように回復量を計算
                int heal = Math.Min(value, neto.MaxHP - neto.CurrentHP);
                neto.CurrentHP += heal;
                //GameManager.Instance.uiManager.ShowLog($"HPが {heal} 回復しました。");
                break;

            case 2: // 攻撃力アップ
                //GameManager.Instance.uiManager.ShowLog($"攻撃力が上昇するアイテムは、このキャラクターには無効です！");
                break;

            case 3: // 防御力アップ
                neto.ApplyTemporaryDef(value);
                //GameManager.Instance.uiManager.ShowLog($"防御力が一時的に {value} 上がりました！");
                break;

            case 99: // バフ解除（デバッグ完了など）
                neto.ClearBuffs();
                //GameManager.Instance.uiManager.ShowLog("ステータス変化が元に戻りました。");
                break;
        }

    }
}