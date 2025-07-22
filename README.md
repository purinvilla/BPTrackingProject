<img width="40%" alt="image" src="https://github.com/user-attachments/assets/97a2e96f-6331-4cc2-92f5-3cff5bb37787" /><img width="40%" alt="image" src="https://github.com/user-attachments/assets/dfe8f4e5-86c6-4146-8827-cb079b9bcd79" />

This project consists of a complete client/server system for users to track their own blood pressure. The front-end is coded in C# and uses the .NET MAUI framework, which allows for the generation of a cross-platform application; the back-end is coded in Java and uses Spring Boot alongside a REST API.

# Project Architecture

<img width="100%" alt="image" src="https://github.com/user-attachments/assets/8bd07b1a-52d6-4583-b0c7-5e2c68e92bcf" />

This project ships with a cross-platform application developed with .NET MAUI, which supports iOS, Android, macOS (for Mac Catalyst and above), and Windows. Android was used as the main target platform. It includes the following backend components:

• An embedded Tomcat server that utilizes Spring Framework.

• A REST API that uses CRUD (Create, Read, Update, Delete) for modifying data in the User and Admin classes.

• The use of Spring Data JPA in order to ease the process of development.

• A MySQL database to store all of the data.

• Security for supporting multiple users.

<img width="75%" alt="image" src="https://github.com/user-attachments/assets/dcdb0705-b127-4d08-8999-3949d745097c" />

For more information, check out the PDF in the docs folder.
