namespace FoodFlowSystem.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public PaginationInfo pagination { get; set; }

        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(T data, int totalRecords, int currentPage, int pageSize)
        {
            this.Data = data;

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            pagination = new PaginationInfo
            {
                TotalRecord = totalRecords,
                CurrentPage = currentPage,
                TotalPages = totalPages,
                NextPage = currentPage < totalPages ? currentPage + 1 : null,
                PrevPage = currentPage > 1 ? currentPage - 1 : null
            };
        }
    }
}