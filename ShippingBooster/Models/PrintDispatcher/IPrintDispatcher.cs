using ShippingBooster.Models.Printer;

namespace ShippingBooster.Models.PrintDispatcher;

public interface IPrintDispatcher
{
    void Dispatch(IPrinter printer, string markdown);
}