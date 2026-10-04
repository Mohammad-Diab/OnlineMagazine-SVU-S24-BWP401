using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers
{
    [ApiController]
    public class MyControllerBase : ControllerBase
    {
        internal string getTokenValue()
        {
            return Request.Headers.Where(x => x.Key.ToLower() == "mytoken").FirstOrDefault().Value.ToString() ?? "";
        }
    }
}
