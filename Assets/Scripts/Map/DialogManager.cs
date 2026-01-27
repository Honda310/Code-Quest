using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    Queue<string> currentLines;
    TalkEventList talkEventList;
    public void StartDialogue(int npcId)
    {
        currentLines = new Queue<string>(talkEventList.TalkEventTable[npcId]);
        ShowNext();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowNext();
        }
    }

    void ShowNext()
    {
        if (currentLines.Count == 0)
        {
            UIManager.Active?.TalkingEventEnd();
            return;
        }
        string line = currentLines.Dequeue();
        UIManager.Active?.TalkingFowarded(line);
    }
}
