using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SharpVectors.Converters;
using ShippingBooster.Models.Printer;

namespace ShippingBooster.Models.PrintDispatcher;

public class XpsDocumentPrintDispatcher(string printQueueName) : IPrintDispatcher
{
    public void Dispatch(IPrinter printer, string markdown)
    {
        var printServer = new LocalPrintServer();
        var queue = printServer.GetPrintQueue(printQueueName);
        var writer = PrintQueue.CreateXpsDocumentWriter(queue);

        const double lengthInMm = 200;
        const double widthInMm = 58;
        const double printableWidthInMm = 48;
        var ticket = queue.DefaultPrintTicket;
        ticket.PageMediaSize = new PageMediaSize(ConvertMmToPx(widthInMm), ConvertMmToPx(lengthInMm));
        ticket.PageOrientation = PageOrientation.Portrait;

        var page = new FixedPage()
        {
            Width = ConvertMmToPx(widthInMm),
            Height = ConvertMmToPx(lengthInMm)
        };
        var grid = new Grid()
        {
            Width = ConvertMmToPx(printableWidthInMm),
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var svg = printer.CreateSvg(markdown);
        AddSvg(svg, grid);
        page.Children.Add(grid);

        writer.Write(page, ticket);
    }

    private static double ConvertMmToPx(double mm)
    {
        return mm * 3.7795275591;
    }

    private static void AddSvg(string svgContent, Grid grid)
    {
        var svg = new SvgBitmap()
        {
            SvgSource = svgContent
        };
        grid.Children.Add(svg);
    }
}