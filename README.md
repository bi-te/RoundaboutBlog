clone repository

launch postgres database

run dotnet user-secrets init in project directory

add database connectin string named dbConnection  

add smtp section settings to user secrets in the following form:
"SmtpSettings": 
{
    "Host": "smtp.gmail.com",
    "Port": 465,
    "User": "email@gmail.com",
    "Password": "",
    "Ssl": true,
    "Company": ""
}

run dotnet ef database update in project directory
