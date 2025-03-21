namespace Library.Models.Responses
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public void AddError(string error)
        {
            Success = false;
            Errors.Add(error);
        }
    }
}
