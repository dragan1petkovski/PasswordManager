using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ItemTimeInfo : ItemId
    {
        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }
    }
}
