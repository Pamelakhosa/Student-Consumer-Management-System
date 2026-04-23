# Student-Consumer-Management-System C#

## 📖 Overview

This project is a web-based management system built using **ASP.NET Core MVC**. It provides functionality for managing student and consumer records, with secure authentication and role-based access control.

The application follows clean architectural principles, including separation of concerns and the repository pattern, making it scalable and maintainable.

---

## ✨ Key Features

### 🔐 Authentication & Authorization

* User authentication using ASP.NET Core Identity
* Role-based access control (Admin, User, Consumer)
* Secure login and user management


### 🗄️ Data Persistence

* SQLite database integration
* Entity Framework Core for ORM
* Separate contexts for application data and authentication

###  Architecture & Design

* Repository pattern implementation
* Dependency injection
* Interface-based design
* Centralized exception handling

---

##  Technologies Used

* Framework: ASP.NET Core MVC
* Language: C#
* Database: SQLite
* ORM: Entity Framework Core
* Authentication: ASP.NET Core Identity
* Frontend: Razor Views

---

## 🚀 Getting Started

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/)
* Visual Studio or VS Code


## 🔐 Default Admin Access

For demonstration purposes, a default admin account is seeded:

* Email:admin@gmail.com
* Password: Admin@1234


## 📂 Project Structure

```
├── Models              # Data models (Student, Consumer)
├── Interfaces          # Service contracts
├── Repository          # Data access layer
├── Data                # Database contexts
├── Controllers         # MVC controllers
├── Views               # Razor UI components
└── Program.cs          # Application configuration
```

