using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ReauestFeature
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int count, int pagenumer, int pagesize)
        {

            MetaData = new MetaData()
            {

                PageSize = pagesize,
                TotalCount = count,
                CurrentPage = pagenumer,
                TotalPages = (int)Math.Ceiling(count / (decimal)pagesize),
            };
            AddRange(items);

        }
        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 2;
            }
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var res = new PagedList<T>(items, count, pageSize, pageNumber);
            return res;
        }
    }
}
