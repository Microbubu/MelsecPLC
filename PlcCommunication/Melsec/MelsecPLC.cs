using System;
using ACTMULTILib;
using System.Collections.Generic;
using System.Linq;
using PlcCommunication.Config;

namespace PlcCommunication.Melsec
{
    public class MelsecPLC : IDisposable
    {
        private const string PortNotOpen = "串口未打开";
        private const string ReadFailed = "读取失败";

        private ActEasyIFClass com = null;

        public bool IsOpen { get; private set; }

        private Config.Config config;

        private string configFile;
        public string ConfigFile
        {
            get => configFile;
            set
            {
                configFile = value;
                config = Config.Config.DeserializeFromXml(value);
            }
        }

        public MelsecPLC()
        {
            com = new ActEasyIFClass();
        }

        public MelsecPLC(int station) : this(station, string.Empty)
        {

        }

        public MelsecPLC(int station,string password)
        {
            com = new ActEasyIFClass();
            com.ActLogicalStationNumber = station;
            if (!string.IsNullOrEmpty(password))
                com.ActPassword = password;
        }

        public bool Open()
        {
            lock (this)
            {
                if (!IsOpen)
                {
                    com.Open();
                    IsOpen = true;
                    return true;
                }
                else return false;
            }
        }

        public bool Close()
        {
            lock (this)
            {
                if (IsOpen)
                {
                    com.Close();
                    IsOpen = false;
                }
                return true;
            }
        }

        public void Dispose()
        {
            if (IsOpen) com.Close();
            com = null;
            IsOpen = false;
        }

        public int Read(string device, int size)
        {
            if (!IsOpen) throw new Exception(PortNotOpen);

            int ret = com.ReadDeviceBlock(device, size, out int data);
            if (ret == 0) return data;
            throw new Exception(ReadFailed);
        }

        public int Read(string devName, string groupName, string tagName)
        {
            var tag = FindTag(devName, groupName, tagName);
            if (tag == null) throw new Exception("Tag not found.");
            return Read(tag.DeviceAddress, tag.Size);
        }

        public bool Write(string device, int size, int data)
        {
            if (!IsOpen) throw new Exception(PortNotOpen);

            int ret = com.WriteDeviceBlock(device, size, ref data);
            return ret == 0;
        }

        public bool Write(string devName, string groupName, string tagName, int data)
        {
            var tag = FindTag(devName, groupName, tagName);
            if (tag == null) throw new Exception("Tag noe found.");
            return Write(tag.DeviceAddress, tag.Size, data);
        }

        public int[] ReadRandom(List<string> deviceList)
        {
            if (!IsOpen) throw new Exception(PortNotOpen);

            var devices = string.Empty;
            deviceList.ForEach(v => devices += $"{v}\n");

            var data = new int[deviceList.Count];
            int ret = com.ReadDeviceRandom(devices, deviceList.Count, out data[0]);

            if (ret == 0) return data;
            throw new Exception(ReadFailed);
        }

        public bool WriteRandom(List<string> deviceList, int[] data)
        {
            if (!IsOpen) throw new Exception(PortNotOpen);

            if (deviceList?.Count == 0 || deviceList.Count != data.Length)
                throw new ArgumentException(
                    $"{nameof(deviceList)}和{nameof(data)}元素数量不匹配");

            var devices = string.Empty;
            deviceList.ForEach(v => devices += $"{v}\n");

            int ret = com.WriteDeviceRandom(devices, data.Length, ref data[0]);
            return ret == 0;
        }

        private Tag FindTag(string devName, string groupName, string tagName)
            => config?.Devs?
                .FirstOrDefault(v => v.DevName == devName)?.Groups?
                .FirstOrDefault(v => v.GroupName == groupName)?.Tags?
                .FirstOrDefault(v => v.TagName == tagName);
    }
}
