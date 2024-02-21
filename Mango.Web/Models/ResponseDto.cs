namespace Mango.Web.Models
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true; //Default value
        public string Msg { get; set; } = string.Empty;
    }
}
