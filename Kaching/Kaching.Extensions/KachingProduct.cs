using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kaching.Extensions
{
    public class KachingProduct
    {
        public L10nString Name { get; set; }
        public L10nString Description { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public string Id { get; internal set; }

        public string ImageUrl { get; set; }

        public Variant[] Variants { get; set; }
        public List<Dimension> Dimensions { get; set; }

        public Dictionary<string, string> Attributes { get; set; }
        public Dictionary<string, bool> Tags { get; set; }
    }

    public class Variant
    {
        public L10nString Name { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public string Id { get; internal set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, string> DimensionValues { get; set; }

        public Dictionary<string, string> Attributes { get; set; }
    }

    public class Dimension
    {
        public L10nString Name { get; set; }
        public string Id { get; set; }
        public List<DimensionValue> Values { get; set; }
    }

    public class DimensionValue
    {
        public string Id { get; set; }
        public L10nString Name { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
    }
}
