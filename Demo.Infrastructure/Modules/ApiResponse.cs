using System.Net;

namespace Demo.Infrastructure.Modules;
public class ApiResponse
{
    public Object Result { get; set; }
    public bool isActive { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; } = new List<string>(); 

    public ApiResponse()
    {
        ErrorMessages = new List<string>();
    }
}
