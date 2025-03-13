namespace Pawfect_Backend.Models
{
    //standard response
    public class Responses<T>
    {
      public  int StatusCode { get; set; }
      public  string Message { get; set; }
     public   string Error { get; set; }
      public  T Data { get; set; }


    }
}
