using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcCommunication.Common
{
    public static class Utils
    {
        public static byte[] GetParity(this byte[] bytes)
        {
            int sum = 0;
            foreach (var item in bytes)
                sum += item;

            var check = sum.ToString("x2");
            check = check.Length < 2 ?
                check.PadLeft(2, '0') :
                check.Substring(check.Length - 2, 2);

            return Encoding.ASCII.GetBytes(check);
        }

        public static byte[] HexStringToBytes(string hexStr)
        {
            hexStr = hexStr.PadLeft(((hexStr.Length + 1) / 2) * 2, '0');

            byte[] bytes = new byte[hexStr.Length / 2];
            for(int i = 0; i < hexStr.Length; i += 2)
            {
                byte val = byte.Parse(hexStr.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                bytes[i / 2] = val;
            }
            return bytes;
        }
    }
}
