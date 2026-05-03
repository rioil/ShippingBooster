using Livet;
using Livet.Commands;
using ShippingBooster.Models.Markdown.Booth;
using ShippingBooster.Models.PrintDispatcher;
using ShippingBooster.Models.Printer;

namespace ShippingBooster.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public static string[] AvailableItems { get; } =
        ["🧈1", "🧈1 + 🔨片面", "🧈1 + 🔨両面", "🧈2", "🧈2 + 🔨片面", "🧈2 + 🔨両面"];

    public string OrderNo
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = string.Empty;

    public DateTime OrderDate
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = DateTime.Today;

    public DateTime DataReceivedDate
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = DateTime.Today;

    public string OrderItem
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = AvailableItems[0];

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

    public string Status
    {
        get;
        private set => RaisePropertyChangedIfSet(ref field, value);
    } = string.Empty;


    private void Preview()
    {
        var printer = new MPT2Printer();

        var labelCreator = new BoothShippingLabelCreator();
        var labelMarkdown = labelCreator.Create(OrderNo, OrderDate, DataReceivedDate);
        ShippingLabelSvg = printer.CreateSvg(labelMarkdown);

        var receiptCreator = new BoothReceiptCreator();
        var receiptMarkdown = receiptCreator.Create(OrderNo, OrderItem);
        ReceiptSvg = printer.CreateSvg(receiptMarkdown);
    }

    public ViewModelCommand PreviewCommand => field ??= new ViewModelCommand(Preview);

    private void PrintShippingLabel()
    {
        if (ShippingLabelSvg == null) return;

        SetStatus("Printing shipping label...");
        var creator = new BoothShippingLabelCreator();
        var markdown = creator.Create(OrderNo, OrderDate, DataReceivedDate);
        var dispatcher = new XpsDocumentPrintDispatcher("POS58 Printer BT");
        var printer = new MPT2Printer();
        dispatcher.Dispatch(printer, markdown);
        SetStatus("Printed shipping label.");
    }

    public ViewModelCommand PrintShippingLabelCommand => field ??= new ViewModelCommand(PrintShippingLabel);

    private void PrintReceipt()
    {
        if (ReceiptSvg == null) return;

        SetStatus("Printing receipt...");
        var creator = new BoothReceiptCreator();
        var markdown = creator.Create(OrderNo, OrderItem);
        var dispatcher = new XpsDocumentPrintDispatcher("POS58 Printer BT");
        var printer = new MPT2Printer();
        dispatcher.Dispatch(printer, markdown);
        SetStatus("Printed receipt.");
    }

    public ViewModelCommand PrintReceiptCommand => field ??= new ViewModelCommand(PrintReceipt);

    private void SetStatus(string status)
    {
        Status = $"{DateTime.Now:HH:mm:ss} {status}";
    }
}