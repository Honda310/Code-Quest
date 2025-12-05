using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rb;
    int roop_frame=0;
    float moveSpeed = 85.0f;
    int tale_moving=-1;
    private Dictionary<string, Sprite> sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 pos = rb.position;
        sprites = new Dictionary<string, Sprite>()
        {
            { "-1_-1", Load("3left") },
            { "-1_1", Load("3left_tail") },

            { "1_-1", Load("4right") },
            { "1_1", Load("4right_tail") },
        };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = Vector2.zero;
        roop_frame++;
        if (roop_frame<=200)
        {
            direction.x = 1;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }else if (roop_frame <= 600)
        {
            direction.x = -1;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }else if(roop_frame < 800)
        {
            direction.x = 1;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            roop_frame = 0;
        }
        if (roop_frame % 20 == 0)
        {
            tale_moving = tale_moving * -1;
        }
        string spriteKey = $"{direction.x}_{tale_moving}";
        if (sprites.ContainsKey(spriteKey))
        {
            GetComponent<SpriteRenderer>().sprite = sprites[spriteKey];
        }
    }
    Sprite Load(string name)
    {
        return Resources.Load<Sprite>($"Image/Enemy/promd/3Enemy_06EasyEn_1normal_{name}");
    }
}
