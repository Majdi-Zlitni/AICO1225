User Story: Login with Invalid Credentials
ID: TC-002 Title: Login Failure on Incorrect Password

As a: security-conscious user of saucedemo.com I want to: see a clear error message when I enter the wrong password So that I can: be sure that unauthorized users cannot access my account.

Acceptance Criteria
Given I am on the login page at https://www.saucedemo.com.
When I enter the valid username standard_user into the input field with id="user-name".
And I enter an incorrect password, such as wrong_password, into the input field with id="password".
And I click the login button with id="login-button".
Then I should remain on the same page and not be redirected.
And an error message must be displayed.
And the error message should contain the text "Username and password do not match any user in this service".
