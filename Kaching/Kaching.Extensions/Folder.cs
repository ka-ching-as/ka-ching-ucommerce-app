using System;
using System.Collections.Generic;

namespace Kaching.Extensions
{
    public class Folder
    {
        public Filter Filter { get; set; }
        public List<Folder> Children { get; set; }

        public Folder(string tag)
        {
            this.Filter = new Filter();
            this.Filter.Tag = tag;
        }
    }

    public class Filter
    {
        public string Tag { get; set; }
    }
}
