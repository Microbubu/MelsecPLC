using System;
using System.Collections.Generic;
using System.Windows;

namespace MelsecPLC.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestModel model;

        public MainWindow()
        {
            InitializeComponent();
            model = new TestModel();
            this.DataContext = model;
            this.ReadResultBox.ItemsSource = model.ReadedData;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)OpenButton.Content == "Open")
            {
                model.InitialMelsecPlc();
                if (model.MelsecPlc.Open())
                {
                    OpenButton.Content = "Close";
                    MessageBox.Show("Open port successfully.");
                }
                else MessageBox.Show("Open port failed.");
            }
            else
            {
                if(model.MelsecPlc!=null)
                {
                    if (model.MelsecPlc.Close())
                        OpenButton.Content = "Open";
                }
            }
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if(model.MelsecPlc==null || !model.MelsecPlc.IsOpen)
            {
                MessageBox.Show("Make sure plc port is open.");
                return;
            }

            try
            {
                model.SetDeviceAddrList();
                if (model.DeviceAddrList.Count == 1)
                {
                    int ret = model.MelsecPlc.Read(model.DeviceAddrList[0], model.DataSize);
                    model.SetReadedData(new int[] { ret });
                }
                else
                {
                    int[] ret = model.MelsecPlc.ReadRandom(model.DeviceAddrList);
                    model.SetReadedData(ret);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (model.MelsecPlc == null || !model.MelsecPlc.IsOpen)
            {
                MessageBox.Show("Make sure plc port is open.");
                return;
            }

            try
            {
                model.SetDeviceAddrList();
                model.SetDataToWriteList();
                if (model.DeviceAddrList.Count == 1)
                {
                    bool success = model.MelsecPlc.Write(
                        model.DeviceAddrList[0],
                        model.DataSize, 
                        model.DataToWriteList[0]);
                    MessageBox.Show(success ? "Write successfully." : "Write failed.");
                }
                else
                {
                    if (model.DeviceAddrList.Count != model.DataToWriteList.Count)
                    {
                        MessageBox.Show("Make sure device address's count equals to data's count.");
                        return;
                    }

                    bool success = model.MelsecPlc.WriteRandom(
                        model.DeviceAddrList,
                        model.DataToWriteList.ToArray());
                    MessageBox.Show(success ? "Write successfully." : "Write failed.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
