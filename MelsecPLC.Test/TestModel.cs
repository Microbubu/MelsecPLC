using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlcCommunication.Common;

namespace MelsecPLC.Test
{
    public class TestModel : NotifyPropertyChange
    {
        public TestModel()
        {
            DeviceAddrList = new List<string>();
            DataToWriteList = new List<int>();
        }

        public List<string> DeviceAddrList { get; private set; }
        public List<int> DataToWriteList { get; private set; }

        public PlcCommunication.Melsec.MelsecPLC MelsecPlc;

        private int _stationNumber = 0;
        public int StationNumber
        {
            get => _stationNumber;
            set
            {
                _stationNumber = value;
                Notify(nameof(StationNumber));
            }
        }

        private string _deviceAddress;
        public string DeviceAddress
        {
            get => _deviceAddress;
            set
            {
                _deviceAddress = value;
                Notify(nameof(DeviceAddress));
            }
        }

        private int _dataSize;
        public int DataSize
        {
            get => _dataSize;
            set
            {
                _dataSize = value;
                Notify(nameof(DataSize));
            }
        }

        private string _dataToWrite;
        public string DataToWrite
        {
            get => _dataToWrite;
            set
            {
                _dataToWrite = value;
                Notify(nameof(DataToWrite));
            }
        }

        public ObservableCollection<int> ReadedData = new ObservableCollection<int>();

        public void InitialMelsecPlc() 
            => this.MelsecPlc = new PlcCommunication.Melsec.MelsecPLC(_stationNumber);

        public void SetDeviceAddrList()
        {
            DeviceAddrList.Clear();
            var list = GetSeperateText(_deviceAddress);
            if (list == null) return;
            list.ForEach(v => DeviceAddrList.Add(v));
        }

        public void SetDataToWriteList()
        {
            DataToWriteList.Clear();
            var datas = GetSeperateText(_dataToWrite);
            if (datas == null) return;
            foreach(var v in datas)
            {
                if (int.TryParse(v, out int num))
                    DataToWriteList.Add(num);
                else
                {
                    DataToWriteList.Clear();
                    return;
                }
            }
        }

        public void SetReadedData(int[] data)
        {
            ReadedData.Clear();
            foreach (var v in data)
                ReadedData.Add(v);
        }

        private List<string> GetSeperateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            List<string> retList = new List<string>();
            foreach (var v in text.Split(','))
            {
                retList.Add(v);
            }

            return retList;
        }
    }
}
