Projet Overview 
The Hotel Management System is an advanced software designed to manage the Hotel operations such as managing reservation , billing, payment etc. 
Users can book rooms for specific dates,admin can manage everything in the app.
[Screenshots of UI]

Built With
* ASP.NET Framework
* BootStrap
* SQL Server

 Prerequisites
Before running the application, ensure you have the following installed:
* Visual Studio IDE
* SQL Server Management Studio

Installation and Setup
Clone the Repository:
git clone https://github.com/MensurH/internstellar-hotel-management-system.git
Restore Dependencies:
Open the solution file HotelManagementSystem.sln in Visual Studio and restore the NuGet packages for the solution.

Configure Connection String:
Update the connection string in AppSettings.json with your SQL Server credentials.

Run Migrations:
Open Package Manager Console in Visual Studio and execute the following commands:
Update-Database

Run the Application:
Press F5 or click on Start Debugging in Visual Studio to run the application. The url is https://localhost:5000.

The Hotel Management System provides functionalities such as:
*Managing rooms and roometypes
*Managing reservations
*Managing Bookings
*Managing Employees 
*Payment Management (Stripe,Paypal)etc

Contributions to improve the Hotel Management System are welcome.
To contribute:
Fork the project
Create your feature branch (git checkout -b feature/my-feature)
Commit your changes (git commit -am 'Add my feature')
Push to the branch (git push origin feature/my-feature)
Open a pull request

Acknowledgments
Fluent Validation
https://docs.fluentvalidation.net/en/latest/
Auto Mapper
https://dotnettutorials.net/lesson/automapper-in-c-sharp/
Onion Architecture
https://romanglushach.medium.com/understanding-hexagonal-clean-onion-and-traditional-layered-architectures-a-deep-dive-c0f93b8a1b96
