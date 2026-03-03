using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rb;
    int roop_frame=0;
    float moveSpeed = 85.0f;
    int moving=-1;
    private Dictionary<string, Sprite> PromdSprites;
    private Dictionary<string, Sprite> GabettaSprites;
    private Dictionary<string, Sprite> PackettonSprites;
    //private Dictionary<string, Sprite> [“G‚Ì–¼‘O]Sprites;
    private Dictionary<string, Sprite> TrojarSprites;
    private Dictionary<string, Sprite> adSprites;
    private Dictionary<string, Sprite> MatranSprites;
    private Dictionary<string, Sprite> Modu1;
    private Dictionary<string, Sprite> Modu2;
    private Dictionary<string, Sprite> Modu3;
    private Dictionary<string, Sprite> MulsleddaSprites;
    [SerializeField] private Enemy enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        //PackettonSprites = new Dictionary<string, Sprite>()
        //{
        //    { "-1_-1", Load("Packetton/1_1paketton_left") },
        //    { "-1_1", Load("Packetton/1_2paketton_battleMotion_left") },

        //    { "1_-1", Load("Packetton/2_1paketton_right") },
        //    { "1_1", Load("Packetton/2_2paketton_battleMotion_right") },
        //};
        TrojarSprites = new Dictionary<string, Sprite>()
        {
            { "-1_0", Load("Trojar/left_walk1") },
            { "-1_1", Load("Trojar/left_walk2") },
            { "-1_2", Load("Trojar/left_walk3") },

            { "1_0", Load("Trojar/right_walk1") },
            { "1_1", Load("Trojar/right_walk2") },
            { "1_2", Load("Trojar/right_walk3") },
        };
        MulsleddaSprites = new Dictionary<string, Sprite>()
        {
            { "1_1", Load("Mulsledda/") },
            { "-1_1", Load("Mulsledda/") },
        };
        adSprites = new Dictionary<string, Sprite>()
        {
            { "-1_-1", Load("2ad/2walk") },
            { "-1_1", Load("2ad_2walk") },

            { "1_-1", Load("2ad_3walk") },
            { "1_1", Load("2ad_3walk") },
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
            string spriteKey = $"{direction.x}_{MovePreset(roop_frame)}";
            if (enemy.EnemyName == "Promd")
            {
                if (PromdSprites.ContainsKey(spriteKey))
                {
                    GetComponent<SpriteRenderer>().sprite = PromdSprites[spriteKey];
                }
            }
            else if (enemy.EnemyName == "Gabetta")
            {
                if (GabettaSprites.ContainsKey(spriteKey))
                {
                    GetComponent<SpriteRenderer>().sprite = GabettaSprites[spriteKey];
                }
            }
            else if (enemy.EnemyName == "Packetton")
            {
                if (PackettonSprites.ContainsKey(spriteKey))
                {
                    GetComponent<SpriteRenderer>().sprite = PackettonSprites[spriteKey];
                }
            }
            else if (enemy.EnemyName == "Trojar")
            {
                spriteKey = $"{direction.x}_{MovePresetTrojar(roop_frame)}";
                if (TrojarSprites.ContainsKey(spriteKey))
                {
                    GetComponent<SpriteRenderer>().sprite = TrojarSprites[spriteKey];
                }
            }else if (enemy.EnemyName == "Mulsledda")
            {
                spriteKey = $"{direction.x}_1";
                if (MulsleddaSprites.ContainsKey(spriteKey))
                {
                    GetComponent<SpriteRenderer>().sprite = TrojarSprites[spriteKey];
                }
            }
            //else if(enemy.EnemyName == "")
            //{
            //    if (PackettonSprites.ContainsKey(spriteKey))
            //    {
            //        GetComponent<SpriteRenderer>().sprite = PackettonSprites[spriteKey];
            //    }
            //}

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
    private int MovePreset(int roop_frame)
    {
        if (roop_frame % 20 == 0)
        {
            return moving = moving * -1;
        }
        else
        {
            return moving;
        }
    }
    private int MovePresetTrojar(int roop_frame)
    {
        if (roop_frame % 15 == 0)
        {
            switch (moving)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                case 2:
                    return 0;
                default:
                    return moving;
            }
        }
        else
        {
            return moving;
        }
    }
}
