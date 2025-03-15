namespace FoodFlowSystem.DTOs.Requests.User
{
    public class CreateUserRequest
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
