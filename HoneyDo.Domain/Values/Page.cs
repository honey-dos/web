using System;

namespace HoneyDo.Domain.Values
{
    public class Page
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public Page(int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentException("Must be greater than 0", nameof(pageIndex));
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                throw new ArgumentException("Must be greater than 0 & less than 100", nameof(pageSize));
            }

            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
