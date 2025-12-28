using Livet;
using Livet.Commands;
using ShippingBooster.Models.Booth;
using ShippingBooster.Models.Print;

namespace ShippingBooster.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public string OrderNo
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = "Order-0123456789";

    public DateTime OrderDate
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = DateTime.Today;

    public string ShippingCode
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = "SC-9876543210";

    public string ShippingReceiptNo
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = "SRN-1234567890";

    public string ShippingPassword
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = "password123";

    public string? ShippingLabelSvg
    {
        get;
        private set => RaisePropertyChangedIfSet(ref field, value);
    }

    public string? ReceiptSvg
    {
        get;
        private set => RaisePropertyChangedIfSet(ref field, value);
    }


    private void Preview()
    {
        var labelCreator = new BoothShippingLabelCreator();
        ShippingLabelSvg = labelCreator.Create(OrderNo, OrderDate, ShippingCode, ShippingReceiptNo, ShippingPassword);

        var receiptCreator = new BoothReceiptCreator();
        ReceiptSvg = receiptCreator.Create(OrderNo, "🧈1");
    }

    public ViewModelCommand PreviewCommand => field ??= new ViewModelCommand(Preview);

    private void PrintShippingLabel()
    {
        if (ShippingLabelSvg == null) return;

        // "HPRT MPT-II BT"
        var printer = new Printer("POS58 Printer BT");
        printer.Print(ShippingLabelSvg);
    }

    public ViewModelCommand PrintShippingLabelCommand => field ??= new ViewModelCommand(PrintShippingLabel);

    private void PrintReceipt()
    {
        if (ReceiptSvg == null) return;
        var printer = new Printer("POS58 Printer BT");
        printer.Print(ReceiptSvg);
    }

    public ViewModelCommand PrintReceiptCommand => field ??= new ViewModelCommand(PrintReceipt);
}