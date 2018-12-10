using PlcCommunication.Config;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PLCConfigFileGenerator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel();
            this.CreateTree();
            this.DataContext = viewModel;
            this.ConfigTree.ItemsSource = viewModel.LeftTree;
            this.TagsView.ItemsSource = viewModel.TagCollection;
        }

        private void ConfigTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewModel item = (sender as TreeView).SelectedItem as TreeViewModel;
            viewModel.SelectedTreeNode = item;
        }

        #region test code
        private void CreateTree()
        {
            viewModel.Config = new Config()
            {
                ConfigName = "TestConfig",
                Devs = new List<Dev>()
                {
                    new Dev()
                    {
                        DevName="Developer",
                        Groups=new List<Group>()
                        {
                            new Group()
                            {
                                GroupName="Github",
                                Tags=new List<Tag>()
                                {
                                    new Tag()
                                    {
                                        TagName="GithubID",
                                        DeviceAddress="Microbubu"
                                    },
                                    new Tag()
                                    {
                                        TagName="GithubPage",
                                        DeviceAddress="https://microbubu.github.io"
                                    }
                                }
                            },
                            new Group()
                            {
                                GroupName="Weibo",
                                Tags=new List<Tag>()
                                {
                                    new Tag()
                                    {
                                        TagName="WeiboID",
                                        DeviceAddress="Microbubu"
                                    },
                                    new Tag()
                                    {
                                        TagName="BlogPage",
                                        DeviceAddress="https://weibo.com/microbubu"
                                    }
                                }
                            }
                        }
                    },
                    new Dev()
                    {
                        DevName="Microsoft",
                        Groups=new List<Group>()
                        {
                            new Group()
                            {
                                GroupName="WebSite",
                                Tags=new List<Tag>()
                                {
                                    new Tag()
                                    {
                                        TagName="HomePage",
                                        DeviceAddress="https://www.microsoft.com"
                                    },
                                    new Tag()
                                    {
                                        TagName="MSDN",
                                        DeviceAddress="https://msdn.microsoft.com"
                                    }
                                }
                            },
                            new Group()
                            {
                                GroupName="Employees",
                                Tags=new List<Tag>()
                                {
                                    new Tag()
                                    {
                                        TagName="CEO",
                                        DeviceAddress="Satya Nadella"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            viewModel.LeftTree = TreeViewModel.CreateTreeFromConfig(viewModel.Config);
        }
        #endregion

        private void CommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            if (e.Command == ApplicationCommands.Open)
                e.CanExecute = true;
            if (e.Command == ApplicationCommands.Save)
                e.CanExecute = true;
            if (e.Command == ApplicationCommands.Close)
                e.CanExecute = true;

            e.Handled = true;
        }

        private void OpenCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            using (var ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.CheckFileExists = true;
                ofd.Filter = "Xml Files|*.xml|Json Files|*.json";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string file = ofd.FileName;
                    string extension = GetFileExtension(file);
                    try
                    {
                        if (extension == "json")
                            viewModel.Config = Config.DeserializeFromJson(file);
                        else if (extension == "xml")
                            viewModel.Config = Config.DeserializeFromXml(file);

                        if (viewModel.Config == null) return;

                        viewModel.Reset();
                        viewModel.LeftTree = TreeViewModel.CreateTreeFromConfig(viewModel.Config);
                        ConfigTree.ItemsSource = null;
                        ConfigTree.ItemsSource = viewModel.LeftTree;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Unexpected thing happed.\n" + ex.StackTrace);
                    }
                }
            }
        }

        private void SaveCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            using (var sfd = new System.Windows.Forms.SaveFileDialog())
            {
                sfd.Filter = "Xml Files|*.xml|Json Files|*.json";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = sfd.FileName;
                    string extension = GetFileExtension(fileName);
                    viewModel.Config = TreeViewModel.GetConfigObjectFromTree(viewModel.LeftTree);

                    if (viewModel.Config == null)
                        return;

                    if (extension == "json")
                        Config.SerializeJsonToFile(viewModel.Config, fileName);
                    else if (extension == "xml")
                        Config.SerializeXmlToFile(viewModel.Config, fileName);
                }
            }
        }

        private void CloseCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private string GetFileExtension(string fileName)
        {
            string[] splits = fileName?.Split('.');
            if (splits == null || splits.Length == 0)
                return string.Empty;
            return splits[splits.Length - 1].ToLower();
        }

        /// <summary>
        /// 新增树节点(Dev)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodeNewDev_Clicked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTreeNode == null || viewModel.SelectedTreeNode.Deepth == 1)
            {
                var window = new TreeNodeEditWindow(NodeType.Dev, null);
                window.Owner = this;
                var ret = window.ShowDialog();

                if (string.IsNullOrEmpty(ret))
                    return;

                var dev = new Dev() { DevName = ret };

                if (viewModel.Config.Devs == null)
                    viewModel.Config.Devs = new List<Dev>();

                viewModel.Config.Devs.Add(dev);
                viewModel.LeftTree = TreeViewModel.CreateTreeFromConfig(viewModel.Config);
                ConfigTree.ItemsSource = viewModel.LeftTree;
            }
        }

        /// <summary>
        /// 新增树节点(Group)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodeNewGroup_Clicked(object sender, RoutedEventArgs e)
        {
            var window = new TreeNodeEditWindow(NodeType.Group, null);
            window.Owner = this;
            var ret = window.ShowDialog();

            if (string.IsNullOrEmpty(ret))
                return;

            var group = new Group() { GroupName = ret };

            if (viewModel.SelectedTreeNode?.Deepth == 1)
            {
                if ((viewModel.SelectedTreeNode.ConfigItem as Dev).Groups == null)
                    (viewModel.SelectedTreeNode.ConfigItem as Dev).Groups = new List<Group>();
                (viewModel.SelectedTreeNode.ConfigItem as Dev).Groups.Add(group);
            }

            else if (viewModel.SelectedTreeNode?.Deepth == 2)
            {
                if ((viewModel.SelectedTreeNode.Parent.ConfigItem as Dev).Groups == null)
                    (viewModel.SelectedTreeNode.Parent.ConfigItem as Dev).Groups = new List<Group>();
                (viewModel.SelectedTreeNode.Parent.ConfigItem as Dev).Groups.Add(group);
            }

            viewModel.LeftTree = TreeViewModel.CreateTreeFromConfig(viewModel.Config);
            ConfigTree.ItemsSource = viewModel.LeftTree;
        }

        /// <summary>
        /// 删除树结点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodeDelete_Clicked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTreeNode == null) return;

            var messageResult = MessageBox.Show(
                $"Are you sure to delete \"{viewModel.SelectedTreeNode.DisplayName}\"",
                "Delete",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);

            if (messageResult == MessageBoxResult.Cancel) return;

            //删除Dev
            if (viewModel.SelectedTreeNode.Parent == null)
            {
                viewModel.Config.Devs.Remove(viewModel.SelectedTreeNode.ConfigItem as Dev);
                viewModel.SelectedTreeNode = null;
            }
            else
            {
                //删除Group
                if (viewModel.SelectedTreeNode.Deepth == 2)
                {
                    var selected = viewModel.SelectedTreeNode.ConfigItem as Group;
                    (viewModel.SelectedTreeNode.Parent.ConfigItem as Dev).Groups.Remove(selected);
                    viewModel.SelectedTreeNode = null;
                }
            }

            viewModel.LeftTree = TreeViewModel.CreateTreeFromConfig(viewModel.Config);
            ConfigTree.ItemsSource = viewModel.LeftTree;
        }

        /// <summary>
        /// 编辑树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodeEdit_Clicked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTreeNode == null) return;

            TreeNodeEditWindow window = null;
            if (viewModel.SelectedTreeNode.Deepth == 1)
            {
                window = new TreeNodeEditWindow(NodeType.Dev, viewModel.SelectedTreeNode.DisplayName);
            }
            else if (viewModel.SelectedTreeNode.Deepth == 2)
            {
                window = new TreeNodeEditWindow(NodeType.Group, viewModel.SelectedTreeNode.DisplayName);
            }
            if (window != null)
            {
                window.Owner = this;
                var ret = window.ShowDialog();
                if (!string.IsNullOrEmpty(ret))
                    viewModel.SelectedTreeNode.DisplayName = ret;
            }
        }

        /// <summary>
        /// 新增Tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TagsViewNewMenu_Clicked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTreeNode == null) return;

            var window = new TagEditWindow();
            window.Owner = this;
            var ret = window.ShowDialog();

            if (ret == null) return;

            if ((viewModel.SelectedTreeNode.ConfigItem as Group).Tags == null)
                (viewModel.SelectedTreeNode.ConfigItem as Group).Tags = new List<Tag>();

            (viewModel.SelectedTreeNode.ConfigItem as Group).Tags.Add(ret);
            viewModel.TagCollection.Add(ret);
        }

        /// <summary>
        /// 删除Tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TagsViewDeleteMenu_Clicked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTag == null)
                return;

            var messageResult = MessageBox.Show(
                $"Are you sure to delete \"{ viewModel.SelectedTag.TagName}\"",
                "Delete",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);

            if (messageResult == MessageBoxResult.Cancel) return;

            (viewModel.SelectedTreeNode.ConfigItem as Group).Tags.Remove(viewModel.SelectedTag);
            viewModel.TagCollection.Remove(viewModel.SelectedTag);
        }

        /// <summary>
        /// 编辑Tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TagsViewEditMenu_Clicked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTag == null)
                return;

            var window = new TagEditWindow(viewModel.SelectedTag);
            window.Owner = this;
            var ret = window.ShowDialog();

            if (ret == null) return;

            viewModel.SelectedTag.TagName = ret.TagName;
            viewModel.SelectedTag.DeviceAddress = ret.DeviceAddress;
            viewModel.SelectedTag.Size = ret.Size;
        }
    }
}
