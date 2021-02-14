using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SageAPI.Models
{
    public class PartClass
    {
        public int ObjectID { get; set; }
        public string Name { get; set; }
        public int IndentLevel { get; set; }
        public int ClassParentID { get; set; }
        public int HasChildren { get; set; }

    }
}