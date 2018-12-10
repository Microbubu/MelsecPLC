using PlcCommunication.Config;
using System.Windows;

namespace PLCConfigFileGenerator
{
    /// <summary>
    /// TagEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TagEditWindow : Window
    {
        private Tag newTag;

        public TagEditWindow()
        {
            InitializeComponent();
            newTag = new Tag();
            this.DataContext = newTag;
        }

        public TagEditWindow(Tag tag)
        {
            InitializeComponent();
            newTag = new Tag
            {
                TagName = tag.TagName,
                DeviceAddress = tag.DeviceAddress,
                Size = tag.Size
            };

            this.DataContext = newTag;
        }

        public new Tag ShowDialog()
        {
            base.ShowDialog();
            return newTag;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(newTag.TagName) || string.IsNullOrWhiteSpace(newTag.DeviceAddress))
            {
                MessageBox.Show("TagName and DeviceAddress can not be empty.");
                return;
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            newTag = null;
            this.Close();
        }
    }
}
