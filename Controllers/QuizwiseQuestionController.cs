using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using QuizManagement.Models;
using QuizManagement;

namespace QuizManagement.Controllers
{
    [CheckAccess]
    public class QuizwiseQuestionController : Controller
    {
        private IConfiguration configuration;

        public QuizwiseQuestionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult AddEdit_QuizwiseQuestion(QuizwiseQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                QuizDropDown();
                QuestionDropDown();
                UserDropDown();
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuizwiseQuestionsID == null || model.QuizwiseQuestionsID == 0)
                {
                    command.CommandText = "PR_QuizWiseQuestions_Insert";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizID;
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                }
                else
                {
                    command.CommandText = "PR_QuizWiseQuestions_UpdateByPK";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = model.QuizwiseQuestionsID;
                    //command.Parameters.Add("@Created", SqlDbType.DateTime).Value = DateTime.Now;
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizID;
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                    //command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = DateTime.Now;

                }


                command.ExecuteNonQuery();
                return RedirectToAction("QuizwiseQuestionList");
            }
            else
            {
                QuizDropDown();
                QuestionDropDown();
                UserDropDown();
                return View("AddEdit_QuizwiseQuestion", model);
            }
        }
        //[HttpPost]
        //public IActionResult AddEdit_QuizwiseQuestion(QuizwiseQuestionModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        QuizDropDown();
        //        QuestionDropDown();
        //        UserDropDown();
        //        string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //        SqlConnection connection = new SqlConnection(connectionString);
        //        connection.Open();
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;

        //        if (model.QuizwiseQuestionsID == null)
        //        {
        //            command.CommandText = "PR_QuizWiseQuestions_Insert";
        //            command.Parameters.Add("@Created", SqlDbType.DateTime).Value = DateTime.Now;
        //        }
        //        else
        //        {
        //            command.CommandText = "PR_QuizWiseQuestions_Update";
        //            command.Parameters.Add("@QuizwiseQuestionsID", SqlDbType.Int).Value = model.QuizwiseQuestionsID;
        //        }
        //        command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizID;
        //        command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
        //        command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
        //        command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = DateTime.Now;

        //        command.ExecuteNonQuery();
        //        return RedirectToAction("QuizwiseQuestionList");
        //    }
        //    else
        //    {
        //        QuizDropDown();
        //        QuestionDropDown();
        //        UserDropDown();
        //        return View("AddEdit_QuizwiseQuestion", model);
        //    }
        //}

        public void UserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "[dbo].[PR_USER_DROPDOWN]";
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

        public void QuizDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "[dbo].[PR_QUIZ_DROPDOWN]";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<QuizDropdown> QuizList = new List<QuizDropdown>();
            foreach (DataRow data in dataTable2.Rows)
            {
                QuizDropdown model = new QuizDropdown();
                model.QuizID = Convert.ToInt32(data["QuizID"]);
                model.QuizName = data["QuizName"].ToString();
                QuizList.Add(model);
            }
            ViewBag.QuizList = QuizList;
        }

        public void QuestionDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "[dbo].[PR_QUESTION_DROPDOWN]";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<QuestionDropdown> QuestionList = new List<QuestionDropdown>();
            foreach (DataRow data in dataTable2.Rows)
            {
                QuestionDropdown model = new QuestionDropdown();
                model.QuestionID = Convert.ToInt32(data["QuestionID"]);
                model.QuestionText = data["QuestionText"].ToString();
                QuestionList.Add(model);
            }
            ViewBag.questionList = QuestionList;
        }

        public IActionResult EditQuizwiseQuestion(int QuizWiseQuestionsID)
        {
            if (QuizWiseQuestionsID == null)
            {
                QuizDropDown();
                QuestionDropDown();
                UserDropDown();
                return View("AddEdit_QuizwiseQuestion");
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[PR_QuizWiseQuestions_SelectByPK]";
            command.Parameters.AddWithValue("@QuizWiseQuestionsID", QuizWiseQuestionsID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuizwiseQuestionModel model = new QuizwiseQuestionModel();

            foreach (DataRow dataRow in table.Rows)
            {
                model.QuizID = Convert.ToInt32(dataRow["QuizID"]);
                model.QuestionID = Convert.ToInt32(dataRow["QuestionID"]);
                model.UserID = Convert.ToInt32(dataRow["UserId"]);

            }
            QuizDropDown();
            QuestionDropDown();
            UserDropDown();
            return View("AddEdit_QuizwiseQuestion", model);
        }

        public IActionResult QuizwiseQuestionList()
        {
            var connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_QuizWiseQuestions_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuizwiseQuestionDelete(int QuizWiseQuestionsID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_QuizWiseQuestions_DeleteByPK";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = QuizWiseQuestionsID;
                    command.ExecuteNonQuery();
                }
                TempData["SuccessMessage"] = "QuizwiseQuestion deleted successfully.";
                return RedirectToAction("QuizwiseQuestionList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the city: " + ex.Message;
                return RedirectToAction("QuizwiseQuestionList");
            }
        }

        public IActionResult QuizWizeQuestionDetails()
        {
            return View();
        }

    }
}