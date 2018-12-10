using System.Collections.Generic;
using PlcCommunication.Config;
using PlcCommunication.Common;

namespace PLCConfigFileGenerator
{
    public class TreeViewModel : NotifyPropertyChange
    {
        public string Icon { get; set; }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                Name = value;
                DisplayNameChanged();
                Notify(nameof(DisplayName));
            }
        }

        public string Name { get; set; }

        public TreeViewModel Parent { get; set; }

        public List<TreeViewModel> Children { get; set; }

        public int Deepth { get; set; }

        public object ConfigItem { get; private set; }

        public TreeViewModel()
        {
            Children = new List<TreeViewModel>();
        }

        public TreeViewModel(object obj)
        {
            Children = new List<TreeViewModel>();
            ConfigItem = obj;
        }

        private void DisplayNameChanged()
        {
            if(this.ConfigItem is Dev)
                ((Dev)ConfigItem).DevName = _displayName;

            if (this.ConfigItem is Group)
                ((Group)ConfigItem).GroupName = _displayName;

            if (this.ConfigItem is Tag)
                ((Tag)ConfigItem).TagName = _displayName;
        }

        public static List<TreeViewModel> CreateTreeFromConfig(Config config)
        {
            if (config == null) return null;
            if (config.Devs == null) return null;
            if (config?.Devs?.Count == 0) return null;

            var tree = new List<TreeViewModel>();
            foreach(var dev in config.Devs)
            {
                var item1 = new TreeViewModel(dev)
                {
                    Name = dev.DevName,
                    DisplayName = dev.DevName,
                    Icon = "Assets/root.png",
                    Deepth = 1
                };

                if (dev.Groups?.Count > 0)
                {
                    item1.Children = new List<TreeViewModel>();
                    foreach(var group in dev.Groups)
                    {
                        var item2 = new TreeViewModel(group)
                        {
                            Name = group.GroupName,
                            DisplayName = group.GroupName,
                            Icon = "Assets/group.png",
                            Deepth = 2,
                            Parent = item1
                        };
                        item1.Children.Add(item2);
                    }
                }
                tree.Add(item1);
            }

            return tree;
        }

        public static Config GetConfigObjectFromTree(List<TreeViewModel> tree)
        {
            if (tree == null || tree.Count == 0) return null;

            var config = new Config();
            config.Devs = new List<Dev>();
            foreach(var item in tree)
            {
                config.Devs.Add(item.ConfigItem as Dev);
            }
            return config;
        }
    }
}
