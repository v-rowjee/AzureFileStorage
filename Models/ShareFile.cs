using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileStorage.Models
{
    public class ShareFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public long Size { get; set; }
    }
}