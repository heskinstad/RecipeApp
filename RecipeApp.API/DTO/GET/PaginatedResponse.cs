namespace RecipeApp.API.DTO.GET
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public PaginatedResponse(List<T> items, int totalCount, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
        }
    }

}
