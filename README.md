**Roundabout blog** is a blog application written in Asp.Net Core. It provides the following features:
1. User Authentication ([registration](https://github.com/bi-te/RoundaboutBlog/blob/master/images/register.jpg), [log in](https://github.com/bi-te/RoundaboutBlog/blob/master/images/login.jpg), [log out](https://github.com/bi-te/RoundaboutBlog/blob/master/images/logout.jpg))
2. [Change password](https://github.com/bi-te/RoundaboutBlog/blob/master/images/password.jpg)
3. [Forgot password](https://github.com/bi-te/RoundaboutBlog/blob/master/images/forgot_password.jpg)
4. [Two-factor authentication](https://github.com/bi-te/RoundaboutBlog/blob/master/images/2fa.jpg)
5. [View and Edit user profile](https://github.com/bi-te/RoundaboutBlog/blob/master/images/profile.jpg)
6. [Download and Delete personal data](https://github.com/bi-te/RoundaboutBlog/blob/master/images/personal_data.jpg)
7. [View](https://github.com/bi-te/RoundaboutBlog/blob/master/images/view_post_comment.jpg) and [Add](https://github.com/bi-te/RoundaboutBlog/blob/master/images/add_post.jpg) post
8. [View and Add comment on a post](https://github.com/bi-te/RoundaboutBlog/blob/master/images/view_post_comment.jpg)
9. [Delete comments](https://github.com/bi-te/RoundaboutBlog/blob/master/images/delete_comment.jpg) (your comment or any on your post)
10. [Home page with posts](https://github.com/bi-te/RoundaboutBlog/blob/master/images/view_posts.jpg)
11. [User dashboard](https://github.com/bi-te/RoundaboutBlog/blob/master/images/dashboard.jpg)

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
