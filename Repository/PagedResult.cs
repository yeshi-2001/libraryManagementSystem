using System.Collections.Generic;

namespace libraryManagementSystem.Repository
{
  
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => PageSize == 0 ? 0 : (int)System.Math.Ceiling((double)TotalCount / PageSize);

        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}