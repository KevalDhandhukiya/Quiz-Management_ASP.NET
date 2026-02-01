# Quiz Management System

A comprehensive web application developed using **ASP.NET Core MVC** for efficient management of quizzes, questions, and users. This system provides a streamlined interface for administrators to create, edit, and organize quizzes while tracking system metrics via a dynamic dashboard.

## 🚀 Features

*   **Dashboard Overview**: Real-time insights into the total number of quizzes, questions, users, and question difficulty levels.
*   **Quiz Management**: Full CRUD (Create, Read, Update, Delete) capabilities for Quizzes.
*   **Question Bank**: Manage a repository of questions, categorize them by difficulty (Question Levels), and assign them to specific quizzes.
*   **User Management**: Administration of user accounts and profiles.
*   **Quiz-wise Questions**: Organize and map specific questions to individual quizzes.
*   **Secure Access**: Implements authentication and authorization checks (via `CheckAccess` attribute).

## 🛠️ Technology Stack

*   **Backend**: ASP.NET Core MVC (C#)
*   **Database**: Microsoft SQL Server (MSSQL)
*   **Data Access**: ADO.NET with Stored Procedures
*   **Frontend**: HTML5, CSS3, JavaScript, Bootstrap (Razor Views)
*   **IDE**: Microsoft Visual Studio

## 📂 Project Structure

*   **/Controllers**: Contains the core logic for handling user requests (Home, Quiz, Question, User, etc.).
*   **/Models**: Defines the data structures and view models (QuizModel, QuestionModel, UserModel, etc.).
*   **/Views**: Razor/HTML files for the user interface.
*   **/wwwroot**: Static assets like CSS, JavaScript, and images.

## ⚙️ Setup & Installation

1.  **Clone the Repository**
    ```bash
    git clone https://github.com/keval2310/Quiz-Management.git
    cd Quiz-Management
    ```

2.  **Database Configuration**
    *   Ensure you have **Microsoft SQL Server** installed.
    *   Update the connection string in `appsettings.json` to point to your local SQL Server instance.
    ```json
    "ConnectionStrings": {
      "ConnectionString": "Server=YOUR_SERVER_NAME;Database=QuizManagementDB;Trusted_Connection=True;"
    }
    ```
    *   *Note: This project relies on Stored Procedures (e.g., `PR_Quiz_SelectAll`, `PR_Question_SelectAll`). Ensure your database schema and procedures are correctly set up.*

3.  **Run the Application**
    *   Open `QuizManagement.sln` in Visual Studio.
    *   Build the solution (`Ctrl + Shift + B`).
    *   Run the application (`F5`).

## 🤝 Contributing

Contributions are welcome! Please fork the repository and submit a pull request for any enhancements or bug fixes.

## 📄 License

This project is licensed under the MIT License.
