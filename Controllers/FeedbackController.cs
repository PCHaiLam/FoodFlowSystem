using FoodFlowSystem.DTOs.Requests.Feedback;
using FoodFlowSystem.Services.Feedback;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/feedbacks")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetFeedbacks()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }

        [HttpGet]
        public async Task<IActionResult> GetFeedback(int id)
        {
            var feedback = await _feedbackService.GetFeedbackAsync(id);
            return Ok(feedback);
        }

        [HttpGet("product")]
        public async Task<IActionResult> GetFeedbacksByProduct([FromQuery] int id)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByProductIdAsync(id);
            return Ok(feedbacks);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetFeedbacksByUser([FromQuery] int id)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(id);
            return Ok(feedbacks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequest request)
        {
            var feedback = await _feedbackService.CreateFeedbackAsync(request);
            return Ok(feedback);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeedback([FromBody] UpdateFeedbackRequest request)
        {
            var feedback = await _feedbackService.UpdateFeedbackAsync(request);
            return Ok(feedback);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            await _feedbackService.DeleteFeedbackAsync(id);
            return NoContent();
        }
    }
}
