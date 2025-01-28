# TaskNet Backend API

## Overview

**TaskNet Backend** is a RESTful API built with **ASP.NET Core 8.0**, providing user authentication, task management, and logging capabilities. The backend allows users to register, log in, reset their passwords, and manage tasks with pagination. The project uses **Entity Framework Core** with an **In-Memory Database** (for testing purposes) and **Serilog** for structured logging. It also includes **Swagger UI** for easy testing and exploring the API.

## Features

- **Authentication**:
  - User registration
  - Login with JWT authentication
  - Password reset and forgot password functionality

- **Task Management**:
  - CRUD operations on tasks (Create, Read, Update, Delete)
  - Pagination support for fetching tasks

- **Logging**:
  - Structured logging with **Serilog**
  
- **API Documentation**:
  - **Swagger UI** for exploring and testing the API

## Tech Stack

- **Backend Framework**: ASP.NET Core 8.0
- **Database**: Entity Framework Core (In-Memory Database)
- **Authentication**: JWT (JSON Web Tokens)
- **Logging**: Serilog
- **API Documentation**: Swagger
- **Packages**:
  - BCrypt.Net-Next (for password hashing)
  - Microsoft.AspNetCore.Authentication.JwtBearer (for JWT authentication)
  - Swashbuckle.AspNetCore (for Swagger UI)

## Prerequisites

Before getting started, make sure you have the following installed:

- **.NET 8 SDK**: [Download here](https://dotnet.microsoft.com/download/dotnet)
- **IDE**: Visual Studio or Visual Studio Code (with C# extension)


