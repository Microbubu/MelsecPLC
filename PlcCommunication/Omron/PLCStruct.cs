using System;

namespace PlcCommunication.Omron
{
    public struct PLCStruct
    {
        public int[] Integer;
        public char[] Char;

        public PLCStruct(byte[] data)
        {
            this.Integer = new int[data.Length];
            this.Char = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                this.Integer[i] = Convert.ToInt32(data[i]);
                this.Char[i] = Convert.ToChar(data[i]);
            }
        }
    }
}
