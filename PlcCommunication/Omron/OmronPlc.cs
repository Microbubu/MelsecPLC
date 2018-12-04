using System;
using System.IO.Ports;
using System.Threading;

namespace PlcCommunication.Omron
{
    public class OmronPlc : IDisposable
    {
        private SerialPort serialPort;

        public bool IsOpen
        {
            get => serialPort.IsOpen;
        }

        public OmronPlc() : this("COM1")
        {

        }

        public OmronPlc(string portName)
        {
            serialPort = new SerialPort
            {
                PortName = portName,
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.Even,
                StopBits = StopBits.One
            };
        }

        private string EnablePLC(string station)
        {
            lock (this)
            {
                string commStr, result;
                var commHead = $"@{station}SC02";
                var mFCS = GetFSCParity(commHead);
                commStr = commHead + mFCS;

                try
                {
                    serialPort.WriteLine(commStr);
                    Thread.Sleep(50);
                    var sRead = serialPort.ReadLine();
                    result = sRead.Substring(6, 2);
                }
                catch
                {
                    result = string.Empty;
                }

                return result;
            }
        }

        public void Close()
        {
            if (IsOpen)
                serialPort.Close();
        }

        public void Open()
        {
            if (!IsOpen)
                serialPort.Open();
        }

        public byte[] Read(int station, int startAddress, int count, PLCDataType[] dataType)
        {
            var commHead = $"@{ConvertStation(station)}RD" +
                ConvertStartAddress(startAddress) + Format00Str(4, count);
            var mFCS = GetFSCParity(commHead);  //校验位
            var commStr = commHead + mFCS;      //命令串

            var data = string.Empty;
            byte[] buffer = null;
            try
            {
                serialPort.WriteLine(commStr);
                Thread.Sleep(50);
                data = serialPort.ReadLine();
                data = data.Substring(8, data.IndexOf("*") - 10);
                if (!string.IsNullOrEmpty(data))
                {
                    int len = data.Length / 4;
                    buffer = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        buffer[i] = dataType[i] == PLCDataType.HexInt ?
                            byte.Parse("$" + data.Substring(i * 4 + 1, 4), System.Globalization.NumberStyles.HexNumber) :
                            byte.Parse(data.Substring(i * 4 + 1, 4));
                    }
                }
            }
            catch { }
            return buffer;
        }

        private void Write(int station, int startAddress, int count,
            byte[] buffer, PLCDataType dataType)
        {
            this.EnablePLC(Format00Str(2, station));
            var dataInfo = new PLCStruct(buffer);
            var sDataInfo = string.Empty;   //要写入PLC的数据信息

            if (dataType == PLCDataType.HexInt)
            {
                for (int i = 0; i < count; i++)
                    sDataInfo += dataInfo.Integer[i].ToString("x4");
            }
            else if(dataType == PLCDataType.Integer)
            {
                for (int i = 0; i < count; i++)
                    sDataInfo += Format00Str(4, dataInfo.Integer[i]);
            }
            else if(dataType == PLCDataType.Char)
            {
                for (int i = 0; i < count; i++)
                    sDataInfo += Format00Str(4, dataInfo.Char[i]);
            }

            var commHead = "@" + Format00Str(2, station) + "WD" + startAddress + sDataInfo;
            var mFCS = GetFSCParity(commHead);
            var commStr = commHead + mFCS + "*";

            var data = string.Empty;
            try
            {
                serialPort.WriteLine(commStr);
                Thread.Sleep(50);
                data = serialPort.ReadLine();
                data = data.Substring(6, 2);
            }
            catch { }
        }

        public void Dispose()
        {
            this.Close();
            serialPort = null;
        }

        private string ConvertStation(int station) => station.ToString("x2");

        private string ConvertStartAddress(int startAddress) => startAddress.ToString("x2").PadLeft(4, '0');

        private string Format00Str(int length, int num) => num.ToString("x2").PadLeft(length, '0');

        private string GetFSCParity(string commStr)
        {
            int tempFCS = '@';
            for (int i = 2; i < commStr.Length; i++)
                tempFCS ^= commStr[i];
            return tempFCS.ToString("x2");
        }
    }
}
