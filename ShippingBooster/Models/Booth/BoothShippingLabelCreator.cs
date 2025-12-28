namespace ShippingBooster.Models.Booth;

using ReceiptSharp;

public class BoothShippingLabelCreator
{
    public string Create(string orderNo, DateTime orderDate, string shippingCode, string shippingReceiptNo, string shippingPassword)
    {
        var content = $$"""
                      -
                      *
                      貼り付け
                      *
                      -
                      
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
                      -
                      {width:auto}
                      *
                      貼り付け
                      *
                      -
                      """;

        // try
        // {
        //     using var comPort = new SerialPort("COM10", 9600, Parity.None, 8, StopBits.One);
        //     comPort.Handshake = Handshake.RequestToSend;
        //     comPort.WriteTimeout = 5000;
        //     Debug.WriteLine("Opening COM port...");
        //     comPort.Open();
        //     comPort.DiscardInBuffer();
        //     comPort.DataReceived += (sender, args) => Debug.WriteLine(args.EventType);
        //     Debug.WriteLine($"COM port opened. Status: {comPort.IsOpen}");
        //     var command = new Receipt(content, "-p generic -c 32 -l en -s -m 0,0").ToCommand();
        //     command[4] = byte.MaxValue;
        //     Debug.WriteLine(
        //         $"Writing command to COM port: bytes[{command.Length}] {Encoding.GetEncoding("iso-8859-1").GetString(command)}");
        //     comPort.Write(command, 0, command.Length);
        //     comPort.ReadExisting();
        //     Thread.Sleep(10000);
        //     comPort.Close();
        // }
        // catch (Exception ex)
        // {
        //     Debug.WriteLine($"Error: {ex.Message}");
        // }

        return new Receipt(content, "-p generic -c 32 -l ja -m 0,0").ToSvg();
    }
}