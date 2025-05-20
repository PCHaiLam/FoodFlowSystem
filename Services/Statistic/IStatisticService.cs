using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodFlowSystem.DTOs.Responses.Statistic;

namespace FoodFlowSystem.Services.Statistic
{
    public interface IStatisticService
    {
        Task<DailyStatisticResponse> GetStatisticByArangeDateAsync(DateTime startDate, DateTime endDate);
    }
} 