# Project Overview

The Hotel Management System is an advanced software designed to manage hotel operations such as reservations, billing, and payments. Users can book rooms for specific dates, and administrators have full control over the application.

## Key Features

- **Architecture**: Onion architecture style for separation of concerns.
- **Technologies**: 
  - **Language**: C#
  - **Framework**: ASP.NET MVC
  - **Database**: Entity Framework Core with SQL Server

## Prerequisites
Before running the application, ensure you have the following installed:
- Visual Studio IDE
- SQL Server Management Studio

### Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/MensurH/internstellar-hotel-management-system.git
    ```

2. **Restore Dependencies:**
Open the solution file `HotelManagementSystem.sln` in Visual Studio and restore the NuGet packages for the solution.

3. **Configure Connection String:**
Update the connection string in `AppSettings.json` with your SQL Server credentials.

4. **Run Migrations:**
Open Package Manager Console in Visual Studio and execute the following commands:
```csharp
Update-Database

```
5. **Run the Application:**
Press F5 or click on Start Debugging in Visual Studio to run the application. The URL is [https://localhost:5000](https://localhost:5000).

## Functionality

### Core Features:

- **Room Management:** Manage rooms and room types.
- **Reservation Handling:** Efficiently handle guest bookings.
- **Employee Management:** Manage staff roles effectively.
- **Payment Integration:** Integrate Stripe and Paypal payments.
- **Single Sign-in:** Streamline access with Google sign-in.
- **Communication:** Use Outlook for guest communication.
- **Data Management:** Manage AWS RDS with SQL Server.
- **Access Control:** Authenticate and authorize Admins, Users, and Staff.
- **Admin Dashboard:** Provide an overview for administrators.

### Additional Features:

- **Payment Integration:** Seamless Stripe and Paypal integration.
- **Password Recovery:** Allow users to reset passwords via email.
- **Booking Confirmation:** Email notifications upon successful bookings.
- **Email Verification:** Send verification emails for user registration.


## Project Leader
- Mensur Hyseni ([GitHub](https://github.com/MensurH))

## Contributors
- Dimal Maloku ([GitHub](https://github.com/DimalMaloku1))
- Yll Shillova ([GitHub](https://github.com/yllshillova))
- Shkelzen Krasniqi ([GitHub](https://github.com/shkelzenkrasniqi))
- Endrit Zhitia ([GitHub](https://github.com/EndritZh))
- Art Shabani ([GitHub](https://github.com/artshabani))
- Leart Dellova ([GitHub](https://github.com/leartde))
- Ardi Osmani ([GitHub](https://github.com/ArdiOsmani))

## Internship Details

This project was developed during an internship with [StarLabs](https://www.starlabs.dev),
Kosovo's leading software development company. 
Contributions were made remotely, guided by the leadership.

