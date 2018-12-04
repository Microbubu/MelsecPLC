using System;
using PlcCommunication.Common;

namespace PlcLib.Test
{
    class UtilsTest
    {
        static void Main(string[] args)
        {
            HexStringToBytes();
            Console.ReadKey();
        }

        static void HexStringToBytes()
        {
            var hexString = "0200ff";
            var hexBytes = Utils.HexStringToBytes(hexString);
            foreach (var v in hexBytes)
                Console.Write(v.ToString("x2") + " ");
            Console.WriteLine();
        }
    }
}
