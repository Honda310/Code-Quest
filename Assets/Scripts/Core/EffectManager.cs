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
        switch (effectId)
        {
            case 1: // HP回復
                // 最大HPを超えないように回復量を計算
                int heal = Math.Min(value, player.MaxHP - player.CurrentHP);
                player.CurrentHP += heal;
                break;

            case 2: // 攻撃力アップ
                player.ApplyTemporaryAtk(value);
                break;

            case 3: // 防御力アップ
                player.ApplyTemporaryDef(value);
                break;

            case 99: // バフ解除（デバッグ完了など）
                player.ClearBuffs();
                break;
        }
    }
    public void ApplyEffect(int effectId, Neto neto, int value)
    {
        switch (effectId)
        {
            case 1: 
                int heal = Math.Min(value, neto.MaxHP - neto.CurrentHP);
                neto.CurrentHP += heal;
                break;

            case 2:
                break;

            case 3:
                neto.ApplyTemporaryDef(value);
                break;

            case 99:
                neto.ClearBuffs();
                break;
        }

    }
}