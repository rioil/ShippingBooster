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
    public static string[] AvailableItems { get; } = [ "🧈1", "🧈1 + 🔨片面", "🧈1 + 🔨両面" ];
    
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

    public string ShippingCode
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = string.Empty;

    public string ShippingReceiptNo
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = string.Empty;

    public string ShippingPassword
    {
        get;
        set => RaisePropertyChangedIfSet(ref field, value);
    } = string.Empty;

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
        var labelMarkdown = labelCreator.Create(OrderNo, OrderDate, ShippingCode, ShippingReceiptNo, ShippingPassword);
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
        var markdown = creator.Create(OrderNo, OrderItem);
        var dispatcher = new XpsDocumentPrintDispatcher("POS58 Printer BT");
        var printer = new MPT2Printer();
        dispatcher.Dispatch(printer, markdown);
        SetStatus("Printed receipt.");
    }

    public ViewModelCommand PrintReceiptCommand => field ??= new ViewModelCommand(PrintReceipt);

    private async void Load()
    {
        if (!Clipboard.ContainsText()) return;
        var url = Clipboard.GetText();
        if (string.IsNullOrEmpty(url)) return;
        Uri uri;
        try
        {
            uri = new Uri(url);
        }
        catch (UriFormatException)
        {
            Status = "Clipboard does not contain a valid URL.";
            return;
        }
        var image = new BitmapImage(uri);
        
        // Wait for the image to load
        await Task.Delay(TimeSpan.FromMilliseconds(500));

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
        var splits = ShippingCode.Split().Where(s => !string.IsNullOrEmpty(s)).ToArray();
        if (splits.Length >= 3)
        {
            ShippingReceiptNo = splits[1];
            ShippingPassword = splits[2];
        }
        SetStatus("Loaded shipping code from clipboard.");
    }

    public ViewModelCommand LoadCommand => field ??= new ViewModelCommand(Load);

    private void SetStatus(string status)
    {
        Status = $"{DateTime.Now:HH:mm:ss} {status}";
    }
}