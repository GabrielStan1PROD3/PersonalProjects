using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class PagedResult<TDto> where TDto : IDto, new()
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<TDto> Items { get; set; }
    }
}