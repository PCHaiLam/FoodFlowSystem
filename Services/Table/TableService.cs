using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Table;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Table;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Table;
using System.Net.WebSockets;

namespace FoodFlowSystem.Services.Table
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly ILogger<TableService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTableRequest> _createTableRequestValidator;
        private readonly IValidator<UpdateTableRequest> _updateTableRequestValidator;

        public TableService(
            ITableRepository tableRepository,
            ILogger<TableService> logger,
            IMapper mapper,
            IValidator<CreateTableRequest> createTableRequestValidator,
            IValidator<UpdateTableRequest> updateTableRequestValidator
            )
        {
            _tableRepository = tableRepository;
            _logger = logger;
            _mapper = mapper;
            _createTableRequestValidator = createTableRequestValidator;
            _updateTableRequestValidator = updateTableRequestValidator;
        }

        public async Task<TableResponse> CreateTableAsync(CreateTableRequest request)
        {  
            var validationResult = await _createTableRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid input", 400, errors);
            }

            var tableEntity = _mapper.Map<TableEntity>(request);
            var createdTable = await _tableRepository.AddAsync(tableEntity);

            var result = _mapper.Map<TableResponse>(createdTable);
            return result;
        }

        public async Task DeleteTableAsync(int id)
        {
            await _tableRepository.DeleteAsync(id);
        }

        public async Task<TableResponse> GetTableByIdAsync(int id)
        {
            var tableEntity = await _tableRepository.GetByIdAsync(id);
            if (tableEntity == null)
            {
                throw new ApiException($"Table with id {id} not found", 404);
                
            }

            var result = _mapper.Map<TableResponse>(tableEntity);
            return result;
        }

        public async Task<ICollection<TableResponse>> GetTablesAsync()
        {
            var tableEntities = await _tableRepository.GetAllAsync();
            var result = _mapper.Map<ICollection<TableResponse>>(tableEntities);
            return result;
        }

        public async Task<TableResponse> UpdateTableAsync(UpdateTableRequest request)
        {
            var validationResult = await _updateTableRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid input", 400, errors);
            }

            var tableEntity = _mapper.Map<TableEntity>(request);
            var updatedTable = await _tableRepository.UpdateAsync(tableEntity);

            var result = _mapper.Map<TableResponse>(updatedTable);
            return result;
        }
    }
}
