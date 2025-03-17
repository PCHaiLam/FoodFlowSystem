using FoodFlowSystem.DTOs.Requests.Table;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Table
{
    public interface ITableService
    {
        Task<TableResponse> GetTableByIdAsync(int id);
        Task<ICollection<TableResponse>> GetTablesAsync();
        Task<TableResponse> CreateTableAsync(CreateTableRequest request);
        Task<TableResponse> UpdateTableAsync(UpdateTableRequest request);
        Task DeleteTableAsync(int id);
    }
}
