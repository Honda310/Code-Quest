using System.Collections.Generic;
using UnityEngine;

public class TalkEventList: MonoBehaviour
{
    public Dictionary<int, Queue<string>> TalkEventTable = new Dictionary<int, Queue<string>>();
    void Start()
    {
        TalkEventTable[0] = new Queue<string>(new[]
        {
           "「昔々、ある所にコード達の国がありました。",
           "徳高い主によって統治されていたその国は、かつて類を見ないほどの繁栄を極めていました。",
           "しかし、平穏は突如として崩れ去ります。",
           "善良な主は囚われ、代わって王座に就いたのは非道なる暴君でした。",
           "秩序は崩壊し、コード達は狂い始めます。",
           " 正気を保つ者たちもまた、深い絶望の淵で、ただ静かに終焉の時を待ち続けるのでした。」",
           "「朝から暗い気持ちになった。最悪だ。」",
           "本を閉じた",
           "お母さん「起きて…起きて！！学校遅刻するわよ！」",
        });

        TalkEventTable[1] = new Queue<string>(new[]
        {
            "キーンコーンカーンコーン",
            "ムナタカ「始めます」",
            "生徒「よろしくお願いします」",
            "主人公（パソコンをつける）（画面が光だし目の前が真っ白になる）"
        });

        TalkEventTable[2] = new Queue<string>(new[]
        {
           "ネト「みんな…みんなおかしくなっちゃったネト…」",
           "「人間ならきっとみんなのこと元に戻せるネト…」",
           "「でもこんなとこにいるわけないネト…」",
           "ネト「!?」",
           "ネト「助けてほしいネト！！」",
           "主人公「?」or「どうしたの?」",
           "ネト「この世界はいま、色々な物がバグって大変なことになってるネト」\r\n「君の…プログラミングの力でこの世界を救ってほしいネト！」",
           "主人公「わかった」",
           "ネト「ありがとうネト！！ほんと助かるネト！」\r\n「そういえば何て呼べばいいネトー？」",
           "主人公「名前」",
           "ネト「これからよろしくネト！！！」",
        });

        TalkEventTable[2] = new Queue<string>(new[]
        {
            "エネレス「!?」",
            "「ここは通行止めだぞ！」",
            "「通りたいなら俺と戦え！」"
        });
        
        TalkEventTable[3] = new Queue<string>(new[]
        {
            "青いモブ:「なんかようか？」",
        });

        TalkEventTable[4] = new Queue<string>(new[]
        {
            "赤いやつ:「メラメラ」"
        });

        TalkEventTable[5] = new Queue<string>(new[]
        {
            "おかえりなさいませ　ご主人様"
        });

        TalkEventTable[6] = new Queue<string>(new[]
        {
            "ジョン=ゼロ「よく来たね」",
            "「君たちは王を倒そうとか考えているのかな…」",
            "「そうか…ふーむ」",
            "「このまま通してあげたいんだが…それはできないんだ」",
            "「すまないが、君たちに王を倒す資格があるか見定めさせてもらう」",
        });

        TalkEventTable[7] = new Queue<string>(new[]
        {
            "「どうやら君には資格があるみたいだ」",
            "「右に進むといい」",
            "「きっと役に立つものが手に入るよ」"
        });

        TalkEventTable[8] = new Queue<string>(new[]
        {
            "エネレス「本当にこのまま進むのか？」",
            "「そっか…ごめんね」",
            "「わかってもらうまでここは通さない！！」"
        });

        TalkEventTable[9] = new Queue<string>(new[]
        {
            "「そっか…どうしても進むんだね」",
            "「もしできるのならこの先は話し合いで解決してほしい…お願いだよ」"
        });

        TalkEventTable[10] = new Queue<string>(new[]
        {
            "エラー王「お前たちは何者だ…?」",
            "「まぁ…いいか」",
            "「私の邪魔をする者はたとえ神であっても私の物語の『端役』として死んでもらう」"
        });
    }
}
