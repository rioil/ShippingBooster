using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;
using Livet;
using Livet.Commands;
using ShippingBooster.Models.Markdown.Booth;
using ShippingBooster.Models.PrintDispatcher;
using ShippingBooster.Models.Printer;
using ZXing;
using ZXing.Windows.Compatibility;

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

    public string Status
    {
        get;
        private set => RaisePropertyChangedIfSet(ref field, value);
    } = string.Empty;


    private void Preview()
    {
        var printer = new MPT2Printer();
        
        var labelCreator = new BoothShippingLabelCreator();
        var labelMarkdown = labelCreator.Create(OrderNo, OrderDate, ShippingCode, ShippingReceiptNo, ShippingPassword);
        ShippingLabelSvg = printer.CreateSvg(labelMarkdown);

        var receiptCreator = new BoothReceiptCreator();
        var receiptMarkdown = receiptCreator.Create(OrderNo, "🧈1");
        ReceiptSvg = printer.CreateSvg(receiptMarkdown);
    }

    public ViewModelCommand PreviewCommand => field ??= new ViewModelCommand(Preview);

    private void PrintShippingLabel()
    {
        SetStatus("Printing shipping label...");
        var creator = new BoothShippingLabelCreator();
        var markdown = creator.Create(OrderNo, OrderDate, ShippingCode, ShippingReceiptNo, ShippingPassword);
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
        var markdown = creator.Create(OrderNo, "🧈1");
        var dispatcher = new XpsDocumentPrintDispatcher("POS58 Printer BT");
        var printer = new MPT2Printer();
        dispatcher.Dispatch(printer, markdown);
        SetStatus("Printed receipt.");
    }

    public ViewModelCommand PrintReceiptCommand => field ??= new ViewModelCommand(PrintReceipt);

    private void Load()
    {
        if (!Clipboard.ContainsText()) return;
        var url = Clipboard.GetText();
        if (string.IsNullOrEmpty(url)) return;
        var image = new BitmapImage(new Uri(url));

        var reader = new BarcodeReader()
        {
            Options = { PossibleFormats = [BarcodeFormat.QR_CODE], TryInverted = true, TryHarder = true }
        };
        var result = reader.Decode(image);
        if (result == null)
        {
            SetStatus("Failed to read QR code.");
            return;
        }
        ShippingCode = result.Text;
        SetStatus("Loaded shipping code from clipboard.");
    }

    public ViewModelCommand LoadCommand => field ??= new ViewModelCommand(Load);

    private void SetStatus(string status)
    {
        Status = $"{DateTime.Now:HH:mm:ss} {status}";
    }
}