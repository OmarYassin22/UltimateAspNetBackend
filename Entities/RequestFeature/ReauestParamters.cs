using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ReauestFeature
{
    public class ReauestParamters
    {
        const int maxPageSize = 10;
        public int PageNumber { get; set; } = 1;
        private int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
        public string Orderby { get; set; }
        public string? Fields { get; set; }
    }
}
