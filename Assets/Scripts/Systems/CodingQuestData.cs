using UnityEngine;
[System.Serializable]
public class CodingQuestData
{
    public string ID;
    public string QuestionText;
    public string CorrectAnswer;
    public string QuestionHint;
    public string SampleCode;
    public CodingQuestData(string id, string q, string a, string h,string s)
    {
        ID = id;
        QuestionText = q;
        CorrectAnswer = a;
        QuestionHint = h;
        SampleCode = s;
    }
}
