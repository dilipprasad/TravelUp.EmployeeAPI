DB
adminuser
TravelUpUser1122###

#Entity Framewwork Connection string
Server=tcp:travelup.database.windows.net,1433;Initial Catalog=EmployeeInfo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";


------------
# ASP.NET Web API - Local Setup Instructions

This project is an ASP.NET Web API that provides RESTful endpoints for managing and accessing data. Follow the instructions below to set up and run the application locally.

## Table of Contents
1. [Prerequisites](#prerequisites)  
2. [Installation](#installation)  
3. [Configuration](#configuration)  
4. [Running the Application](#running-the-application)  
5. [Testing the API](#testing-the-api)  
6. [Troubleshooting](#troubleshooting)  

---

## Prerequisites

1. **.NET 6.0 SDK or later**  
   - You can download the .NET 6.0 SDK from the [Microsoft .NET Downloads](https://dotnet.microsoft.com/en-us/download) page.  
   - Verify the installation by running:
     ```bash
     dotnet --version
     ```
     It should return a version number 6.x or higher.

2. **Git (optional, if you want to clone via Git)**  
   - Download and install from [Git Downloads](https://git-scm.com/downloads).

3. **IDE or Text Editor** (Recommended: Visual Studio 2022 or Visual Studio Code)  
   - [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)  
   - [Visual Studio Code](https://code.visualstudio.com/download)

---

## Installation

1. **Clone or Download the Project**  
   - **Using Git**:  
     ```bash
     git clone https://github.com/YourUsername/YourAspNetWebAPIProject.git
     ```
     Then navigate into the project folder:
     ```bash
     cd TravelUp.EmployeeAPI
     ```
   - **Using Download ZIP**:  
     - Click the green "Code" button on the repository page and select "Download ZIP".  
     - Extract the ZIP file into a folder of your choice.

2. **Restore Dependencies**  
   If you’re using the command line:  
   ```bash
   dotnet restore
---


## Configuration

1. **appsettings.json**  
   - The default configuration is in `appsettings.json`.  
   - If you need different settings for development, modify `appsettings.Development.json`.

2. **Environment Variables** (if needed)  
   - You can override settings in `appsettings.json` using environment variables.  
   - For example, if the project references a connection string named `DefaultConnection`, you could set an environment variable:
     ```bash
     set ASPNETCORE_ConnectionStrings__DefaultConnection="YourConnectionString"
     ```

> **Note:** If your project doesn’t require additional configuration, you can skip this section.

---

## Running the Application

### Option 1: Using Visual Studio

1. Open the solution (`.sln` file) in Visual Studio.  
2. Make sure the Web API project is set as the **Startup Project**.  
3. Press **F5** (or click the **Run** button).  
4. Visual Studio will compile and launch the application. You can view the logs in the **Output** window.

### Option 2: Using Visual Studio Code

1. Open the folder containing your project in VS Code.  
2. Ensure you have the **C#** extension installed for debugging.  
3. Press **F5** or go to the **Run and Debug** tab and start the debugger.  
4. VS Code will compile and launch the application on the configured port.

### Option 3: Using the .NET CLI

1. In the project directory (where the `.csproj` file is located), run:
   ```bash
   dotnet build


----

## Testing the API

Once the application is running, you can test the endpoints by using a browser, [Postman](https://www.postman.com/).

- **Example using Postman**:  
  ```bash
  https://localhost:7100/

  
- **Example using Swagger**:  
  ```bash
  https://localhost:7100/swagger/index.html

 ---

 ## Inital Migration for Entity Framework
 
  ```bash
  Cd into TravelUp.EmployeeAPI\TravelUp.Employee.Data
  dotnet tool install -g dotnet-ef
  dotnet ef migrations add InitialCreate


For Updates
  ```bash
  Cd into TravelUp.EmployeeAPI\TravelUp.Employee.Data
  dotnet ef database update

  For Backup - Force Migration with the path
  ```bash
  cd C:\Code\TravelUp\TravelUp.EmployeeAPI\TravelUp.EmployeeAPI
  dotnet ef database update --verbose --project C:\Code\TravelUp\TravelUp.EmployeeAPI\TravelUp.EmployeeAPI\TravelUp.Employee.Data\TravelUp.EmployeeAPI.Data.csproj --startup-project C:\Code\TravelUp\TravelUp.EmployeeAPI\TravelUp.EmployeeAPI\TravelUp.EmployeeAPI\TravelUp.EmployeeAPI.csproj

----


## For Testing 

Install the following packages to allow the test to be run in visual studio
    Install-Package MSTest.TestFramework
    Install-Package MSTest.TestAdapter

