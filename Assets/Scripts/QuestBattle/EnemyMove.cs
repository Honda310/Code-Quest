using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rb;
    int roop_frame=0;
    float moveSpeed = 85.0f;
    int tale_moving=-1;
    private Dictionary<string, Sprite> PromdSprites;
    private Dictionary<string, Sprite> GabettaSprites;
    private Dictionary<string, Sprite> PackettonSprites;
    [SerializeField] private Enemy enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 pos = rb.position;
        PromdSprites = new Dictionary<string, Sprite>()
        {
            { "-1_-1", Load("promd/3Enemy_06EasyEn_1normal_3left") },
            { "-1_1", Load("promd/3Enemy_06EasyEn_1normal_3left_tail") },

            { "1_-1", Load("promd/3Enemy_06EasyEn_1normal_4right") },
            { "1_1", Load("promd/3Enemy_06EasyEn_1normal_4right_tail") },
        };
        GabettaSprites = new Dictionary<string, Sprite>()
        {
            { "-1_-1", Load("Gabetta/1_1Gabetta_left") },
            { "-1_1", Load("Gabetta/1_1Gabetta_left") },

            { "1_-1", Load("Gabetta/2_1Gabetta_right") },
            { "1_1", Load("Gabetta/2_1Gabetta_right") },
        };
        PackettonSprites = new Dictionary<string, Sprite>()
        {
            { "-1_-1", Load("Packetton/1_1paketton_left") },
            { "-1_1", Load("Packetton/1_2paketton_battleMotion_left") },

            { "1_-1", Load("Packetton/2_1paketton_right") },
            { "1_1", Load("Packetton/2_2paketton_battleMotion_right") },
        };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.GameMode.Field == GameManager.Instance.CurrentMode)
        {
            Vector2 direction = Vector2.zero;
            roop_frame++;
            if (roop_frame <= 200)
            {
                direction.x = 1;
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
            else if (roop_frame <= 600)
            {
                direction.x = -1;
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
            else if (roop_frame < 800)
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
            if (enemy.EnemyName == "Promd")
            {
                if (PromdSprites.ContainsKey(spriteKey))
                {
                    GetComponent<SpriteRenderer>().sprite = PromdSprites[spriteKey];
                }
            }
            else if(enemy.EnemyName == "Gabetta")
            {
                if (GabettaSprites.ContainsKey(spriteKey))
                {
                    GetComponent<SpriteRenderer>().sprite = GabettaSprites[spriteKey];
                }
            }
            else if(enemy.EnemyName == "Packetton")
            {
                if (PackettonSprites.ContainsKey(spriteKey))
                {
                    GetComponent<SpriteRenderer>().sprite = PackettonSprites[spriteKey];
                }
            }
            else
            {
                Debug.Log(enemy.EnemyName);
            }
        }
    }
    Sprite Load(string name)
    {
        return Resources.Load<Sprite>($"Image/Enemy/{name}");
    }
}
