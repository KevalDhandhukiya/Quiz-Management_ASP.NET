using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizManagement.Models;
using QuizManagement;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QuizManagement.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            DashboardCountsModel model = new DashboardCountsModel();
            string connectionString = _configuration.GetConnectionString("ConnectionString") ?? "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Quizzes
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Quiz_SelectAll";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        model.TotalQuizzes = dt.Rows.Count;
                    }
                }

                // Questions
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Question_SelectAll";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        model.TotalQuestions = dt.Rows.Count;
                    }
                }

                // Users (using Dropdown SP as proxy)
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_Dropdown";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        model.TotalUsers = dt.Rows.Count;
                    }
                }

                 // Question Levels
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_QuestionLevel_SelectAll";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        model.TotalQuestionLevels = dt.Rows.Count;
                    }
                }
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
