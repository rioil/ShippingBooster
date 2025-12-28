namespace ShippingBooster.Models.Markdown.Booth;

public class BoothReceiptCreator
{
    public string Create(string orderNo, string orderItem)
    {
        var content = $$"""
                        貼り付け
                        =

                        Thank you for your purchase!

                        {width:10,20}
                        -
                        注文番号 | {{orderNo}}
                        注文内容 | {{orderItem}}
                        -
                        {width:auto}
                        📜"「Butter」について"📜
                        
                        |アバター「Butter」の著作権は
                        |Polygonal Mind が保有しています
                        |オリジナルデータは CC BY 4.0
                        |ライセンスの下で配布されています

                        |このストラップは、オリジナルデータを参考に3Dプリント用データを独自に作成して印刷したものです

                        オリジナルデータ
                        { code: https://github.com/PolygonalMind/100Avatars/tree/master/100Avatars\_083; option: qrcode,4 }

                        ライセンス（CC BY 4.0）
                        { code: https://github.com/PolygonalMind/100Avatars/blob/master/CCLicense.md; option: qrcode,4 }
                        -
                        {width:auto}
                        📋"アンケート"📋
                        
                        ご協力ありがとうございます！

                        { code: https://docs.google.com/forms/d/e/1FAIpQLSfGELByLsdeFD89autRA0UB6BjMA-fnftzVOEdLWfSs9l1USA/viewform?usp=pp_url&entry.35904481={{orderNo}}; option: qrcode,6 }

                        https://forms.gle/Zk2i4aMT6wDegBpf9
                        """;

        return content;
    }
}