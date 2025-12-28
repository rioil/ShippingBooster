using ReceiptSharp;

namespace ShippingBooster.Models.Printer;

public class MPT2Printer : IPrinter
{
    public string Option => "-p generic -c 32 -l ja -m 0,0";

    public string CreateSvg(string markdown)
    {
        return new Receipt(markdown, Option).ToSvg();
    }
    
    public byte[] CreateCommand(string markdown)
    {
        var command = new Receipt(markdown, Option).ToCommand();
        command[4] = byte.MaxValue;
        return command;
    }
}