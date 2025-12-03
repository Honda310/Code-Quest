using UnityEngine;

/// <summary>
/// キャラクターのアニメーターに移動方向パラメータを渡すクラス。
/// </summary>
public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(Vector2 dir)
    {
        if (animator == null) return;

        // 移動している場合
        if (dir.magnitude > 0)
        {
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}