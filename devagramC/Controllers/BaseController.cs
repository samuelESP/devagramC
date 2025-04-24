using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace devagramC.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {

    }
}
