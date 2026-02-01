using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizManagement.Models;
using QuizManagement;

namespace QuizManagement.Controllers
{
    [CheckAccess]
    public class QuestionLevelController : Controller
    {
        private IConfiguration configuration;

        public QuestionLevelController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        //public IActionResult AddEdit_QuestionLevel()
        //{
        //    return View();
        //}
        public IActionResult QuestionLevelList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_QuestionLevel_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
     
        public IActionResult QuestionLevelDelete(int QuestionLevelID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_QuestionLevel_DeleteByPK";
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = QuestionLevelID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "QuestionLevel deleted successfully.";
                return RedirectToAction("QuestionLevelList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the city: " + ex.Message;
                return RedirectToAction("QuestionLevelList");
            }
        }
        public IActionResult AddEditQuestionLevel(QuestionLevelModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuestionLevelID == null)
                {
                    command.CommandText = "PR_QuestionLevel_Insert";
                }
                else
                {
                    command.CommandText = "PR_QuestionLevel_UpdateByPK";
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = model.QuestionLevelID;

                    command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = DateTime.Now;
                }
                command.Parameters.Add("@QuestionLevel", SqlDbType.VarChar).Value = model.QuestionLevel;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;

                command.ExecuteNonQuery();
                return RedirectToAction("QuestionLevelList");
            }
            UserDropDown();
            return View("AddEdit_QuestionLevel", model);
        }

        public IActionResult AddEdit_QuestionLevel(int? QuestionLevelID)
        {
            if (QuestionLevelID == null)
            {
                UserDropDown();
                return View();
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_QuestionLevel_SelectByPK";
            command.Parameters.AddWithValue("@QuestionLevelID", QuestionLevelID);
            //command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuestionLevelModel model = new QuestionLevelModel();

            foreach (DataRow dataRow in table.Rows)
            {
                model.QuestionLevel = dataRow["QuestionLevel"].ToString();
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
