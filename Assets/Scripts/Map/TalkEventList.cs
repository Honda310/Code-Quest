using System.Collections.Generic;
using UnityEngine;

public class TalkEventList: MonoBehaviour
{
    public Dictionary<int, Queue<string>> TalkEventTable = new Dictionary<int, Queue<string>>();
    void Start()
    {
        TalkEventTable[0] = new Queue<string>(new[]
        {
            "±°İÌ§²Ì§²Ì§²Ì§²",
            "‚¨‘O‚ç‚ÌéŒ¾‚µ‚½•Ï”‚ğ–¢’è‹`‚µ‚Ä‚â‚éÌ§²‚Ë‚¥I",
        });
    }
    void Update()
    {
        
    }
}
