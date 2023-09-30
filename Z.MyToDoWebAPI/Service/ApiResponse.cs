namespace Z.MyToDoWebAPI.Service
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public object Result { get; set; }

        public ApiResponse(string message, bool status = false)
        {
            Message = message;
            Status = status;
        }

        public ApiResponse(bool status, object result)
        {
            Status = status;
            Result = result;
        }
    }
}
