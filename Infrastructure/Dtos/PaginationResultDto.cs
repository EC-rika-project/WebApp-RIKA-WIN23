using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class PaginationResult<T>
    {
        public List<T>? Data { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
    }
}
