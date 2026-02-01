using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizManagement.Models;

namespace QuizManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;

        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        // GET: User/Login
        public IActionResult Login()
        {
            return View(new UserLoginModel());
        }

        // POST: User/Login
        [HttpPost]
        public IActionResult Login(UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginModel);
            }

            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "[dbo].[PR_User_Login2]";
                        sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                        sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;

                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(sqlDataReader);

                            if (dataTable.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dataTable.Rows)
                                {
                                    HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                                    HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                                }

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Invalid username or password.";
                                return View(userLoginModel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred: " + e.Message;
                return View(userLoginModel);
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Registration", userModel);
            }
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "[dbo].[PR_User_Register]"; 
                        sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                        sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                        sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                        sqlCommand.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = userModel.Mobile;
                        
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred: " + e.Message;
                return View("Registration", userModel);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}