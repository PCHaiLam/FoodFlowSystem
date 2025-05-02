using FoodFlowSystem.DTOs;

namespace FoodFlowSystem.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetPaginationInfo(this HttpContext context,
                                        int totalRecords, int currentPage,
                                        int pageSize, int currentPageSize)
        {
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var paginationInfo = new PaginationInfo
            {
                TotalRecord = totalRecords,
                CurrentPage = currentPage,
                TotalPages = totalPages,
                NextPage = currentPage < totalPages ? currentPage + 1 : null,
                PrevPage = currentPage > 1 ? currentPage - 1 : null,
                CurrentPageSize = currentPageSize
            };

            context.Items["PaginationInfo"] = paginationInfo;
            context.Items["RequiresPagination"] = true;
        }
    }
}
