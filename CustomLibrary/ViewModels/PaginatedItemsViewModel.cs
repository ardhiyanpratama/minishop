using System.Collections.Generic;

namespace CustomLibrary.ViewModels
{
    public class PaginatedItemsViewModel<TEntity> where TEntity : class
    {
        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public long Count { get; private set; }

        public IEnumerable<TEntity> Data { get; private set; }

        public PaginatedItemsViewModel(int pageNumber, int pageSize, long count, IEnumerable<TEntity> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
