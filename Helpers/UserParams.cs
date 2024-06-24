using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class UserParams
    {
        private const int MaxPagSize = 50;
        public int PageNumber {get;set;}=1;
        private int _pageSize = 10;

        public int PageSize
        {
            get =>_pageSize;
            set => _pageSize = (value> MaxPagSize ) ? MaxPagSize : value;
        }

        public string CurrentUsername { get; set; }
        public string Gender { get; set; }

        public int MinAge { get; set; }=18;
        public int MaxAge { get; set; }=150;
    }
}