namespace ShippingBooster.Models.Markdown.Booth;

public class BoothShippingLabelCreator
{
    public string Create(string orderNo, DateTime orderDate, string shippingCode, string shippingReceiptNo,
        string shippingPassword)
    {
        var content = $$"""
                        貼り付け
                        =

                        {width:10,20}
                        -
                        注文番号 | {{orderNo}}
                        注文日 | {{orderDate:yyyy/MM/dd}}
                        発送期限 | "{{orderDate + TimeSpan.FromDays(10):yyyy/MM/dd}}"
                        -

                        {width:auto}
                        { code: {{shippingCode}}; option: qrcode,8 }

                        -
                        {width:10,20}
                        受付番号 | {{shippingReceiptNo}}
                        パスワード | {{shippingPassword}}
                        =
                        {width:auto}
                        貼り付け
                        """;

        return content;
    }
}