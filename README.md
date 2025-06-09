**Roundabout blog** is a blog application written in Asp.Net Core. It provides the following features:
1. User Authentication (registration, log in, log out)
2. Change password
3. Forgot password
4. Two-factor authentication
5. View and Edit user profile
6. Download and Delete personal data
7. View and Add post
8. View and Add comment on a post
9. Delete comments (your comment or any on your post)
10. Home page with posts
11. User dashboard

---
##### **Technology**
1. Asp.Net Core (Backend framework)
2. Entity Framework Core (Object-Relational Mapper)
3. Asp.Net Core Identity (Authentication and Authorization, user pages templates)
4. MailKit (SMTP library)
5. Razor Pages (Server-side pages framework)

---
##### **How to launch**
1. clone repository
2. launch postgres database
3. run dotnet user-secrets init in project directory
4. add database connect string to user-secrets named `dbConnection`
5. add smtp section settings to user-secrets in the following form:
```
	"SmtpSettings":   
	{  
		"Host": "smtp.gmail.com",  
		"Port": 465,  
		"User": "email@gmail.com",  
		"Password": "",  
		"Ssl": true,  
		"Company": ""  
	}
```
6. run `dotnet ef database update` in project directory
7. run `dotnet run`
