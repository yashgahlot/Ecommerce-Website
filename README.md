🚁 HeliSound

Full-stack e-commerce web application built with ASP.NET Web Forms, C#, and SQL Server.

📖 Overview

HeliSound is a role-based e-commerce system developed as part of a web application development course. The application simulates a real-world commerce workflow, including product management, order processing, reporting, and shipping operations.

The system supports multiple user roles and enforces secure authentication and structured data management.

✨ Features
User registration and authentication
Role-based access control (Admin, Customer, Shipping)
Product catalog and management
Order placement with billing and delivery information
Automatic tax and total calculation
Order history and tracking
Admin reporting (invoices and revenue)
Shipping workflow for order fulfillment
🛠️ Tech Stack
Frontend: ASP.NET Web Forms, CSS
Backend: C# (.NET Framework), ADO.NET
Database: SQL Server
Authentication: Forms Authentication
Security: SHA256 password hashing with salt
👥 User Roles
Customer
Browse products
Place orders
View order history
Track shipments
Admin
Manage users and products
View reports and revenue summaries
Shipping
View unshipped orders
Mark orders as shipped
Track shipment history
🛒 Ordering Workflow
Select supplier, category, and product
View product details and pricing
Enter quantity
Provide billing and delivery information
Place order

The system automatically calculates subtotal, tax (13%), and total amount. Orders are stored in the database with associated invoice and item details.

🗄️ Database

The application uses a relational database with the following core tables:

Users
Suppliers
Categories
Products
Invoice
InvoiceItem

All relationships are enforced using foreign keys.

Database script: HeliSoundDB.sql

⚙️ Setup Instructions
1. Database Setup
Open SQL Server Management Studio
Create a database named HeliSoundDB
Run the HeliSoundDB.sql script
2. Configure Connection String

Update Web.config:

<connectionStrings>
  <add name="HeliSoundDB"
       connectionString="Server=.;Database=HeliSoundDB;Trusted_Connection=True;"
       providerName="System.Data.SqlClient"/>
</connectionStrings>
3. Run the Application
Open the solution in Visual Studio
Press F5 to run
🔑 Test Accounts
Role	Email	Password
Admin	admin@helisound.local
	Admin123
Customer	a@a.com
	Admin@123
Shipping	shipping@helisound.local
	shipping123
🔒 Security Notes
Passwords are hashed using SHA256 with salt
All database operations use parameterized queries to prevent SQL injection
Role-based authorization restricts access to sensitive pages
📌 Notes

This project was developed to demonstrate core concepts in web application development, including authentication, database design, and multi-role system architecture.

👨‍💻 Author

Yash Gahlot
