using System.Collections.Generic;
using PlcCommunication.Melsec;
using static System.Console;

namespace PlcLib.Test
{
    class MelsecPLCTest
    {
        static string SucceedString = "Successfully";
        static string FailedString = "Failed";

        static void Main()
        {
            var plc = new MelsecPLC(1);
            var device = "D400";
            plc.Open();

            bool success = false;

            success = plc.Write(device, 8, 30);
            Output(nameof(plc.Write), success);

            WriteLine(nameof(plc.Read)+":");
            WriteLine(plc.Read(device, 8));

            List<string> deviceList = new List<string>()
            {
                "D400","D401","D402","D403"
            };

            WriteLine(nameof(plc.WriteRandom) + ":");
            success = plc.WriteRandom(deviceList, new int[4] { 1, 2, 3, 4 });
            Output(nameof(plc.WriteRandom), success);

            WriteLine(nameof(plc.ReadRandom) + ":");
            foreach (var v in plc.ReadRandom(deviceList))
                Write(v + " ");

            plc.Close();
            ReadKey();
        }

        static void Output(string name, bool success)
            => WriteLine($"Method {name} Execute {(success ? SucceedString : FailedString)}.");
    }
}
