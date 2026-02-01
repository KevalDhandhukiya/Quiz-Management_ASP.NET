using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizManagement.Models;
using OfficeOpenXml;
using QuizManagement;

namespace QuizManagement.Controllers
{
    [CheckAccess]
    public class QuestionController : Controller
    {
        private IConfiguration configuration;

        public QuestionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult QuestionList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Question_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        public IActionResult QuestionDelete(int QuestionID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Question_DeleteByPK";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "City deleted successfully.";
                return RedirectToAction("QuestionList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the city: " + ex.Message;
                return RedirectToAction("QuestionList");
            }
        }

        //[HttpPost]
        //public IActionResult QuestionAddEdit(QuestionModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //        SqlConnection connection = new SqlConnection(connectionString);
        //        connection.Open();
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;

        //        if (model.QuestionID == null)
        //        {
        //            command.CommandText = "PR_Question_Insert";
        //        }
        //        else
        //        {
        //            command.CommandText = "PR_Question_UpdateByPK";
        //            command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
        //        }
        //        command.Parameters.Add("@QuestionText", SqlDbType.VarChar).Value = model.QuestionText;
        //        command.Parameters.Add("@QuestionLevelID", SqlDbType.VarChar).Value = model.QuestionLevelID;
        //        //command.Parameters.Add("@QuestionLevel", SqlDbType.VarChar).Value = model.QuestionLevel;
        //        command.Parameters.Add("@OptionA", SqlDbType.DateTime).Value = model.OptionA;
        //        command.Parameters.Add("@OptionB", SqlDbType.DateTime).Value = model.OptionB;
        //        command.Parameters.Add("@OptionC", SqlDbType.DateTime).Value = model.OptionC;
        //        command.Parameters.Add("@OptionD", SqlDbType.DateTime).Value = model.OptionD;
        //        command.Parameters.Add("@CorrectOption", SqlDbType.Int).Value = model.CorrectOption;
        //        command.Parameters.Add("@QuestionMark", SqlDbType.Int).Value = model.QuestionMark;
        //        command.Parameters.Add("@IsActive", SqlDbType.DateTime).Value = model.IsActive;
        //        command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
        //        command.ExecuteNonQuery();
        //        return RedirectToAction("QuestionList");
        //    }

        //    return View("AddEdit_Question", model);
        //}

        //public IActionResult AddEdit_Question(int? QuestionID)
        //{
        //    if (QuestionID == null)
        //    {

        //        return View();
        //    }

        //    string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    SqlCommand command = connection.CreateCommand();
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.CommandText = "PR_Question_SelectByPK";
        //    command.Parameters.AddWithValue("@QuestionID", QuestionID);
        //    //command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());
        //    SqlDataReader reader = command.ExecuteReader();
        //    DataTable table = new DataTable();
        //    table.Load(reader);
        //    QuestionModel model = new QuestionModel();

        //    foreach (DataRow dataRow in table.Rows)
        //    {
        //        model.QuestionText = dataRow["QuestionText"].ToString();
        //        model.QuestionLevelID = Convert.ToInt32(dataRow["QuestionLevelID"]);
        //        //model.QuestionLevel = dataRow["QuestionLevel"].ToString();
        //        model.OptionA = dataRow["OptionA"].ToString();
        //        model.OptionB = dataRow["OptionB"].ToString();
        //        model.OptionC = dataRow["OptionC"].ToString();
        //        model.OptionD = dataRow["OptionD"].ToString();
        //        model.CorrectOption = dataRow["CorrectOption"].ToString();
        //        //model.QuestionMark = Convert.ToInt32(dataRow["QuestionMark"]);
        //        model.IsActive = Convert.ToInt32(dataRow["IsActive"]);
        //        model.UserID = Convert.ToInt32(dataRow["UserID"]);
        //    }
        //    return View(model);
        //}

        public IActionResult AddQuestion(int? QuestionID)
        {
            if (QuestionID == null)
            {
                UserDropDown();
                QuestionLevelDropDown();
                return View();
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[PR_Question_SelectByPK]";
            command.Parameters.AddWithValue("@QuestionID", QuestionID);
            TempData["id"] = QuestionID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuestionModel model = new QuestionModel();

            foreach (DataRow dataRow in table.Rows)
            {
                model.Name = dataRow["QuestionText"].ToString();
                model.OptA = dataRow["OptionA"].ToString();
                model.OptB = dataRow["OptionB"].ToString();
                model.OptC = dataRow["OptionC"].ToString();
                model.OptD = dataRow["OptionD"].ToString();
                model.Correct = dataRow["CorrectOption"].ToString();
                model.Marks = Convert.ToInt32(dataRow["QuestionMarks"]);
                model.UserID = Convert.ToInt32(dataRow["UserID"]);
                model.Level = Convert.ToInt32(dataRow["QuestionLevelID"]);
            }
            UserDropDown();
            QuestionLevelDropDown();
            return View(model);
        }




        public IActionResult QuestionAdd(QuestionModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (model.QuestionID == null)
                {
                    command.CommandText = "[dbo].[PR_Question_Insert]";
                }
                else
                {
                    command.CommandText = "[dbo].[PR_Question_UpdateByPK]";
                        command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
                        command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = DateTime.Now; 
                }


                command.Parameters.Add("@QuestionText", SqlDbType.VarChar).Value = model.Name;
                command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = model.Level;
                command.Parameters.Add("@OptionA", SqlDbType.VarChar).Value = model.OptA;
                command.Parameters.Add("@OptionB", SqlDbType.VarChar).Value = model.OptB;
                command.Parameters.Add("@OptionC", SqlDbType.VarChar).Value = model.OptC;
                command.Parameters.Add("@OptionD", SqlDbType.VarChar).Value = model.OptD;
                command.Parameters.Add("@CorrectOption", SqlDbType.VarChar).Value = model.Correct;
                command.Parameters.Add("@QuestionMarks", SqlDbType.Int).Value = model.Marks;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                //command.Parameters.AddWithValue("@isModified", model.isActive) ;



                command.ExecuteNonQuery();
                return RedirectToAction("QuestionList");
            }
            UserDropDown();
            QuestionLevelDropDown();
            return View("AddQuestion", model);
        }


        //User DropDown
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


        //Question Level DropDown

        public void QuestionLevelDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "[dbo].[PR_QuestionLevel_Dropdown]";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<QuestionLevelDropDown> LevelList = new List<QuestionLevelDropDown>();
            foreach (DataRow data in dataTable2.Rows)
            {
                QuestionLevelDropDown model = new QuestionLevelDropDown();
                model.QuestionLevelID = Convert.ToInt32(data["QuestionLevelID"]);
                model.QuestionLevel = data["QuestionLevel"].ToString();
                LevelList.Add(model);
            }

            ViewBag.LevelList = LevelList;
        }
        public IActionResult ExportToExcel()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Question_SelectAll";

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cells[1, 1].Value = "QuestionID";
                worksheet.Cells[1, 2].Value = "QuestionText";
                worksheet.Cells[1, 3].Value = "CorrectOption";
                worksheet.Cells[1, 4].Value = "QuestionLevel";
                worksheet.Cells[1, 5].Value = "UserName";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuestionID"];
                    worksheet.Cells[row, 2].Value = item["QuestionText"];
                    worksheet.Cells[row, 3].Value = item["CorrectOption"];
                    worksheet.Cells[row, 4].Value = item["QuestionLevel"];
                    worksheet.Cells[row, 5].Value = item["UserName"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Data-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
    }
}