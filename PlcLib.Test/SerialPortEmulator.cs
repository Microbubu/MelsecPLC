using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlcLib.Test
{
    public class SerialPortEmulator
    {
        private static readonly object obj = 0;

        public static void Create()
        {
            var serialPort = new SerialPort
            {
                PortName = "COM2",
                Parity = Parity.Even,
                BaudRate = 9600,
                StopBits = StopBits.One,
                DataBits = 7
            };

            if (!serialPort.IsOpen)
                serialPort.Open();

            List<byte> buffer = null;
            byte[] toWrite = { 0x02, 0x30, 0x32, 0x03, 0x36, 0x35 };

            serialPort.DataReceived += (s, e) =>
            {
                lock (obj)
                {
                    buffer = null;
                    buffer = new List<byte>();
                    buffer.AddRange(Encoding.ASCII.GetBytes(serialPort.ReadExisting()));

                    Console.WriteLine("COM2接收到：");
                    buffer.ForEach(v => Console.Write(v.ToString("x2") + " "));
                    Console.WriteLine();

                    if (buffer[1] == 0x31)      //模拟收到的是写指令
                        serialPort.Write(new byte[] { 0x06 }, 0, 1);
                    else
                        serialPort.Write(toWrite, 0, toWrite.Length);
                }
            };
        }
    }
}
