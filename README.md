# URL Shortener Web App

This is a simple URL Shortener web application built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**.

The app allows users to:
- Shorten long URLs.
- Track and view statistics for each shortened URL via a secret URL.
- Monitor unique visitors per day and the top IP addresses that accessed the URL.

---

## ✅ Features
- Shortens long URLs and generates:
  - A public shortened URL.
  - A private secret stats URL.
- Stores and tracks IP addresses of visitors.
- Statistics include:
  - Unique visits per day (each IP counted once per day).
  - Top 10 IP addresses by visit count.
- Mock data is seeded automatically on first run.
- SecretUrl is not hidden in the app for testing purposes

---

## ✅ Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap

---

## ✅ How to Run the App Locally

### 1. **Clone the Repository**
1. Download or clone this repository to your computer
2. Open the Solution
3. Set Up the Database Connection
4. Run the App
5. Database Initialization
    -On first run:
    -The app will automatically apply database migrations.
    -Mock data will be seeded automatically if no records exist.

## ✅ Mock Data
https://example.com/very-long-url
ShortCode = abc123
SecretUrl = https://example.com/very-long-urladvlsmvpsjafo0i=w[vtwev8254vsd62f1
