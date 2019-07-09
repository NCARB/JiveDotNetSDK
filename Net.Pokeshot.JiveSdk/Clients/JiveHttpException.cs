using System.Web;

namespace Net.Pokeshot.JiveSdk.Clients
{
    public class JiveHttpException : HttpException
    {
        public string Json { get; set; }
        public JiveHttpException(int code, string message, string json):
            base(code, $"{message}\n\n{json}")
        {
            Json = json;
        }
    }
}