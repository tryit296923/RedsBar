using System.Text;

namespace Alcoholic.Services
{
    public class CodeValidatorService
    {
        private const string ValidKey = "ValidationCode";
        private HttpContext httpContext { get; set; }

        public CodeValidatorService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }

        public string Generate()
        {
            string code = "";
            var letters = "ABCDEFGHJKLNOPQRSTUVWXYZ23456789abcdefghijklnopqrstuvwxyz".ToArray();
            Random r = new();
            for(int i = 0; i < 5; i++)
            {
                int index = r.Next(0, letters.Length);
                code = code + letters[index];
                // ASCII 顯示26個基本拉丁字母、阿拉伯數字和英式標點符號
            }
            byte[] bytes = Encoding.ASCII.GetBytes(code);
            httpContext.Session.Set(ValidKey, bytes);
            return code;
        }

        public bool Validate(string code)
        {
            bool isValid = false;
            byte[] bytes = null;
            if(httpContext.Session.TryGetValue(ValidKey,out bytes))
            {
                string ServerCode = Encoding.ASCII.GetString(bytes);
                if (ServerCode.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                {
                    isValid = true;
                }
            }
            return isValid;
        }
    }
}
