using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using ShippingBooster.Models.Printer;

namespace ShippingBooster.Models.PrintDispatcher;

public class EscPosPrintDispatcher : IPrintDispatcher
{
    public void Dispatch(IPrinter printer, string markdown)
    {
        try
        {
            using var comPort = new SerialPort("COM10", 9600, Parity.None, 8, StopBits.One);
            comPort.Handshake = Handshake.RequestToSend;
            comPort.WriteTimeout = 5000;
            Debug.WriteLine("Opening COM port...");
            comPort.Open();
            comPort.DiscardInBuffer();
            comPort.DataReceived += (sender, args) => Debug.WriteLine(args.EventType);
            Debug.WriteLine($"COM port opened. Status: {comPort.IsOpen}");
            var command = printer.CreateCommand(markdown);
            Debug.WriteLine(
                $"Writing command to COM port: bytes[{command.Length}] {Encoding.GetEncoding("iso-8859-1").GetString(command)}");
            comPort.Write(command, 0, command.Length);
            comPort.ReadExisting();
            Thread.Sleep(10000);
            comPort.Close();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }
    }
}