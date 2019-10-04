using System;
using System.Collections.Generic;
using UCommerce;
using UCommerce.Tree;
using UCommerce.Tree.Impl;

namespace Kaching.Extensions.FolderProviders
{
    public class KachingConfigurationFolderProvider: ITreeContentProvider
    {
        public KachingConfigurationFolderProvider()
        {
        }

        public IList<ITreeNodeContent> GetChildren(string nodeType, string id)
        {
            return new List<ITreeNodeContent>()
            {
                new TreeNodeContent("kaching", -1)
                {
                    Name = "Ka-ching",
                    HasChildren = false,
                    Action = "/Apps/Ka-ching/Configuration.aspx",
                    Icon = "icon icon-coins"
                }
            };
        }

        public bool Supports(string nodeType)
        {
            return nodeType == Constants.DataProvider.NodeType.Settings;
        }
    }
}
