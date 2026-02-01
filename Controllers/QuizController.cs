using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using QuizManagement.Models;
using QuizManagement;

namespace QuizManagement.Controllers
{
    [CheckAccess]
    public class QuizController : Controller
    {
        private IConfiguration configuration;

        public QuizController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
/*        public IActionResult AddEdit_Quiz()
        {
            return View();
        }*/
        public IActionResult QuizList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Quiz_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuizDelete(int QuizID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Quiz_DeleteByPK";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = QuizID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Quiz deleted successfully.";
                return RedirectToAction("QuizList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Quiz: " + ex.Message;
                return RedirectToAction("QuizList");
            }
        }
        [HttpPost]
        public IActionResult QuizAddEdit(QuizModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuizID == null)
                {
                    command.CommandText = "PR_Quiz_Insert";
                }
                else
                {
                    command.CommandText = "PR_Quiz_UpdateByPK";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizID;
                    command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = DateTime.Now;
                }
                command.Parameters.Add("@QuizName", SqlDbType.VarChar).Value = model.QuizName;
                command.Parameters.Add("@QuizDate", SqlDbType.DateTime).Value = model.QuizDate;
                command.Parameters.Add("@TotalQuestions", SqlDbType.Int).Value = model.TotalQuestions;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("QuizList");
            }
            UserDropDown();
            return View("AddEdit_Quiz", model);
        }

        public IActionResult AddEdit_Quiz(int? QuizID)
        {
            if (QuizID == null)
            {
                UserDropDown();
                return View();
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Quiz_SelectByPK";
            command.Parameters.AddWithValue("@QuizID", QuizID);
            //command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuizModel model = new QuizModel();

            foreach (DataRow dataRow in table.Rows)
            {
                model.QuizName = dataRow["QuizName"].ToString();
                model.TotalQuestions = dataRow["TotalQuestions"].ToString();
                model.QuizDate = Convert.ToDateTime(dataRow["QuizDate"]);
                model.UserID = dataRow["UserID"].ToString();
            }
            UserDropDown();
            return View(model);
        }

        public void UserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "[dbo].[PR_User_Dropdown]";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<UserDropDown> userList = new List<UserDropDown>();
            foreach (DataRow data in dataTable2.Rows)
            {
                UserDropDown model = new UserDropDown();
                model.UserID = Convert.ToInt32(data["UserID"]);
                model.UserName = data["UserName"].ToString();
                userList.Add(model);
            }
            ViewBag.userList = userList;
        }
    }
}
