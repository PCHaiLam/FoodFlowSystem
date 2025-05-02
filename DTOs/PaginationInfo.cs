namespace FoodFlowSystem.DTOs
{
    public class PaginationInfo
    {
        public int TotalRecord { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int CurrentPageSize { get; set; }
    }
}
