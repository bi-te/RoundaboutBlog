*** Settings ***
Documentation    User Web Tests
Library    Browser
Test Setup      New Browser    chromium    headless=True
Test Teardown   Close Browser
Suite Teardown  Close Browser    ALL

*** Variables ***
${URL}          http://localhost:5154
${USERNAME}     user31
${EMAIL}        user31@mail.com
${PASSWORD}     Test_user31

*** Test Cases ***
Register User
    [Documentation]    Test user registration functionality
    [Tags]            registration    order:1
    Register Test
    
Complex User Operations
    [Documentation]    Test login, post creation, commenting, profile editing and logout
    [Tags]            complex    order:2
    Complex Test

Delete User
    [Documentation]    Test user deletion functionality
    [Tags]            deletion    order:3
    Delete User Test

*** Keywords ***
Register Test
    New Page    ${URL}/Identity/Account/Login?ReturnUrl=%2F
    Wait For Elements State   text="Error. You must be logged in first"  visible  timeout=1s
    Click    a >> "Register"
    Fill Text    [placeholder="username"]    ${USERNAME}
    Fill Text    [placeholder="name@example.com"]    ${EMAIL}
    Fill Text    "Password"    ${PASSWORD}
    Fill Text    "Confirm Password"    ${PASSWORD}
    Click    button >> "Register"
    ${success}=    Run Keyword And Return Status    
    ...    Wait For Elements State    text=Your registration has been    visible
    IF    ${success}
        Take Screenshot    filename=screenshots/registration_success.png
    ELSE
        Take Screenshot    filename=screenshots/registration_error.png
        Fail    Registration failed
    END

Complex Test
    # Login
    New Page    ${URL}/Identity/Account/Login?ReturnUrl=%2F
    Fill Text    [placeholder="name@example.com"]    ${EMAIL}
    Fill Text    [placeholder="password"]    ${PASSWORD}
    Click    button:text("Log in")
    ${success}=    Run Keyword And Return Status    
    ...    Wait For Elements State    h1:text("Blog Posts")    visible
    IF    ${success}
        Take Screenshot    filename=screenshots/login_success.png
    ELSE
        Take Screenshot    filename=screenshots/login_error.png
        Fail    Login failed
    END
    
    # Add post
    Click    li:has-text("${USERNAME} My Profile Dashboard")
    Click    a:text("Add Post")
    Fill Text    "Title"    My First Test Post
    Fill Text    "Content"    Test Content
    Click    button:text("Submit")
    ${success}=    Run Keyword And Return Status    
    ...    Wait For Elements State    "Post created successfully"    visible
    IF    ${success}
        Take Screenshot    filename=screenshots/post_creation_success.png
    ELSE
        Take Screenshot    filename=screenshots/post_creation_error.png
        Fail    Post creation failed
    END
    
    # View post
    Click    a >> "My First Test Post"
    
    # Add comment
    Fill Text    "Title"      Test
    Fill Text    "Comment"    TestComment
    Click        button:text("Add Comment")
    ${success}=    Run Keyword And Return Status    
    ...    Wait For Elements State    text="Comment added successfully"    visible
    ${has_title}=    Run Keyword And Return Status    
    ...    Get Text    main    contains    Test
    ${has_comment}=    Run Keyword And Return Status    
    ...    Get Text    main    contains    TestComment
    IF    ${success} and ${has_title} and ${has_comment}
        Take Screenshot    filename=screenshots/comment_success.png
    ELSE
        Take Screenshot    filename=screenshots/comment_error.png
        Fail    Comment addition failed
    END
    
    # Delete comment
    Handle Future Dialogs    dismiss
    Click    button:text("Delete")
    
    # Delete post
    Click    a:text("Edit")
    Click    button:text("Delete")
    
    # Edit profile
    Click    button:text("${USERNAME}")
    Click    a:text("My Profile")
    Fill Text    [placeholder="Please enter your first name."]    Darius
    Click    button:text("Save")
    ${success}=    Run Keyword And Return Status    
    ...    Wait For Elements State    text="Your profile has been updated"    visible
    ${name_updated}=    Run Keyword And Return Status    
    ...    Get Property    [placeholder="Please enter your first name."]    value    ==    Darius
    IF    ${success} and ${name_updated}
        Take Screenshot    filename=screenshots/profile_success.png
    ELSE
        Take Screenshot    filename=screenshots/profile_error.png
        Fail    Profile update failed
    END
    
    # Logout
    Click    button:text("Logout")
    Wait For Elements State    h1:text("Log in")    visible

Delete User Test
    # Login
    New Page    ${URL}/Identity/Account/Login?ReturnUrl=%2F
    Fill Text    [placeholder="name@example.com"]    ${EMAIL}
    Fill Text    [placeholder="password"]    ${PASSWORD}
    Click    button >> "Log in"
    Wait For Elements State    h1 >> "Blog Posts"    visible
    
    # Delete user
    Click    button:text("${USERNAME}")
    Click    a >> "My Profile"
    Click    a >> "Personal data"
    Click    a >> "Delete"
    Fill Text    [placeholder="Please enter your password."]    ${PASSWORD}
    Click    button >> "Delete data and close my account"
    Wait For Elements State    h1 >> "Log in"    visible