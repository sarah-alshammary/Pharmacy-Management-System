Download & Run Guide
Overview
This project is a Pharmacy Management System that integrates:
•	SQL Server database
•	C# application (Visual Studio)
•	Python scripts (Map & Reminder) using Visual Studio Code

Requirements
Make sure the following are installed:
•	SQL Server (SSMS recommended)
•	Visual Studio (C# project)
•	Visual Studio Code (Python files)
•	Python 

1) Download the Project
1.	Open this GitHub repository
2.	Click Code → Download ZIP or clone it:
git clone <REPO_URL>
3.	The files are uploaded as regular folders (not zipped) and can be placed anywhere on your computer.

2) Database Setup (SQL Server)
1.	Open SQL Server / SSMS
2.	Run the SQL script included in the project
3.	Ensure the database is created successfully

3) Update Server Name (Functions File Only)
1.	Open the project in Visual Studio
2.	Open the Functions file
3.	Update the SQL Server name in the connection string to match your local setup
Note:
The update is done only inside the Functions file.

4) Python Files Setup (Visual Studio Code)
Map Module
Update the paths in CustomerInfo.cs according to where the map files are stored:
var scriptPath = @"C:\Users\sarah\Desktop\work\main.py";
var workingDir = @"C:\Users\sarah\Desktop\work";
Reminder Module
Update the reminder script paths:
var scriptPath = @"C:\Users\sarah\Desktop\reminde\Sarah.pyw";
var workingDir = @"C:\Users\sarah\Desktop\reminde";

5) Screen Size & UI Scaling Note
The application UI was initially developed on a small laptop screen.
When running the project on a device with a larger screen, the layout may appear scaled differently.
To address this:
•	Some UI elements were resized
•	Minor design adjustments were made
This behavior is expected, as the UI layout depends on screen resolution and size.

6) Security Note — Email Reminder File
The reminder module sends emails using Gmail.
Each user must update the following values in the Python file before running it:
SENDER_EMAIL = "your_email@gmail.com"
SENDER_PASSWORD = "YOUR_GMAIL_APP_PASSWORD"
PATIENT_EMAIL = "patient_email@gmail.com"
•	SENDER_EMAIL: Gmail account used to send reminders
•	SENDER_PASSWORD: Gmail App Password (not the normal account password)
•	PATIENT_EMAIL: Email address that receives reminders

7) Run the Project
1.	Open the solution in Visual Studio
2.	Verify the database connection
3.	Run the project

