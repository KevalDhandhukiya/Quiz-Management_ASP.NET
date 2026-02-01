using Microsoft.AspNetCore.Mvc;
using QuizManagement;

namespace QuizManagement.Controllers
{
    [CheckAccess]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
