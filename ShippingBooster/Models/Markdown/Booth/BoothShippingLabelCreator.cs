namespace ShippingBooster.Models.Markdown.Booth;

public class BoothShippingLabelCreator
{
    public string Create(string orderNo, DateTime orderDate, DateTime dataReceivedDate)
    {
        var content = $$"""
                        貼り付け
                        =

                        {width:15,20}
                        -
                        注文番号 | {{orderNo}}
                        注文日 | {{orderDate:yyyy/MM/dd}}
                        データ受付日 | {{dataReceivedDate:yyyy/MM/dd}}
                        発送期限 | "{{dataReceivedDate + TimeSpan.FromDays(14):yyyy/MM/dd}}"
                        -

                        =
                        {width:auto}
                        貼り付け
                        """;

        return content;
    }
}