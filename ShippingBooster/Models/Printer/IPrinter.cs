namespace ShippingBooster.Models.Printer;

public interface IPrinter
{
    string Option { get; }

    string CreateSvg(string markdown);
    
    byte[] CreateCommand(string markdown);
}