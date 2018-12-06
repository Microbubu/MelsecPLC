using System.Windows;
using System.Windows.Controls;

namespace PLCConfigFileGenerator
{
    /// <summary>
    /// TreeNodeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TreeNodeEditWindow : Window
    {
        private string newValue;

        public TreeNodeEditWindow(NodeType type, string oldText)
        {
            InitializeComponent();
            SetLabel(type,oldText);
        }

        private void SetLabel(NodeType type,string oldNameText)
        {
            TextBox.Text = oldNameText;
            if(type == NodeType.Dev)
            {
                Label.Content = "Dev Name:";
            }
            else if (type == NodeType.Group)
            {
                Label.Content = "Group Name:";
            }
        }

        public new string ShowDialog()
        {
            base.ShowDialog();
            return newValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = (sender as Button).Content as string;
            if (content == "OK")
            {
                if (string.IsNullOrWhiteSpace(TextBox.Text))
                {
                    MessageBox.Show("Content can not be empty.");
                    return;
                }
                newValue = TextBox.Text;
                this.Close();
            }
            if (content == "Cancel")
            {
                newValue = null;
                this.Close();
            }
        }
    }

    public enum NodeType
    {
        Dev,
        Group
    }
}
