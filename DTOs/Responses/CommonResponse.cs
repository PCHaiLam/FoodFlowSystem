namespace FoodFlowSystem.DTOs.Responses
{
    public class CommonResponse
    {
        public object Data { get; set; }
        public object Metadata { get; set; }
        public static CommonResponse Success(object data, object metadata)
        {
            return new CommonResponse
            {
                Data = data,
                Metadata = metadata
            };
        }
        public static CommonResponse Error(object data, object metadata)
        {
            return new CommonResponse
            {
                Data = data,
                Metadata = metadata
            };
        }
    }
}