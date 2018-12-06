using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcCommunication.Common;
using PlcCommunication.Config;

namespace PLCConfigFileGenerator
{
    public class ViewModel:NotifyPropertyChange
    {
        private bool _treeNewDevMenuEnable = true;
        public bool TreeNewDevMenuEnable
        {
            get => _treeNewDevMenuEnable;
            set
            {
                _treeNewDevMenuEnable = value;
                Notify(nameof(TreeNewDevMenuEnable));
            }
        }

        private bool _treeNewGroupMenuEnable = false;
        public bool TreeNewGroupMenuEnable
        {
            get => _treeNewGroupMenuEnable;
            set
            {
                _treeNewGroupMenuEnable = value;
                Notify(nameof(TreeNewGroupMenuEnable));
            }
        }

        private bool _treeDeleteMenuEnable = false;
        public bool TreeDeleteMenuEnable
        {
            get => _treeDeleteMenuEnable;
            set
            {
                _treeDeleteMenuEnable = value;
                Notify(nameof(TreeDeleteMenuEnable));
            }
        }

        private bool _treeEditMenuEnable = false;
        public bool TreeEditMenuEnable
        {
            get => _treeEditMenuEnable;
            set
            {
                _treeEditMenuEnable = value;
                Notify(nameof(TreeEditMenuEnable));
            }
        }

        private TreeViewModel _selectedTreeNode;
        public TreeViewModel SelectedTreeNode
        {
            get => _selectedTreeNode;
            set
            {
                _selectedTreeNode = value;
                TagCollection.Clear();

                if (_selectedTreeNode == null)
                {
                    TreeNewDevMenuEnable = true;
                    TreeNewGroupMenuEnable = false;
                    TreeDeleteMenuEnable = false;
                    TreeEditMenuEnable = false;
                }
                else if (_selectedTreeNode.Deepth == 1)
                {
                    TreeNewDevMenuEnable = true;
                    TreeNewGroupMenuEnable = true;
                    TreeDeleteMenuEnable = true;
                    TreeEditMenuEnable = true;
                }
                else if (_selectedTreeNode.Deepth == 2)
                {
                    TreeNewDevMenuEnable = false;
                    TreeNewGroupMenuEnable = true;
                    TreeDeleteMenuEnable = true;
                    TreeEditMenuEnable = true;

                    var group = _selectedTreeNode.ConfigItem as Group;
                    group?.Tags?.ForEach(v => TagCollection.Add(v));
                }
            }
        }

        public Config Config { get; set; } = new Config();

        private List<TreeViewModel> _leftTree;
        public List<TreeViewModel> LeftTree
        {
            get => _leftTree;
            set
            {
                _leftTree = value;
                Notify(nameof(LeftTree));
            }
        }

        public ObservableCollection<Tag> TagCollection = new ObservableCollection<Tag>();

        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get => _selectedTag;
            set
            {
                _selectedTag = value;
                Notify(nameof(SelectedTag));
            }
        }

        public void Reset()
        {
            LeftTree?.Clear();
            TagCollection.Clear();
        }
    }
}
