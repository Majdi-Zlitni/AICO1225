# User Story: Successful Product Purchase and Checkout

**ID:** `TC-001`
**Title:** Successful End-to-End Purchase Flow

**As a:** registered user of `saucedemo.com`
**I want to:** log in, select a specific product, add it to my cart, and complete the checkout process
**So that I can:** successfully purchase an item.

---

### **Acceptance Criteria**

1.  **Given** I am on the login page at `https://www.saucedemo.com`.
2.  **When** I enter the username `standard_user` into the input field with `id="user-name"`.
3.  **And** I enter the password `secret_sauce` into the input field with `id="password"`.
4.  **And** I click the login button with `id="login-button"`.
5.  **Then** I should be redirected to the inventory page, which has the URL path `/inventory.html`.
6.  **And** I should see the "Products" title.
7.  **When** I locate the item named "Sauce Labs Fleece Jacket".
8.  **And** I click the corresponding "Add to cart" button, which has the `id="add-to-cart-sauce-labs-fleece-jacket"`.
9.  **And** I click the shopping cart container, which has the `id="shopping_cart_container"`.
10. **Then** I should be on the cart page, which has the URL path `/cart.html`.
11. **And** the "Sauce Labs Fleece Jacket" should be visible in the cart.
12. **When** I click the checkout button with `id="checkout"`.
13. **And** I enter `Majdi` into the first name field with `id="first-name"`.
14. **And** I enter `Zlitni` into the last name field with `id="last-name"`.
15. **And** I enter `12345` into the postal code field with `id="postal-code"`.
16. **And** I click the continue button with `id="continue"`.
17. **Then** I should be on the checkout overview page (`/checkout-step-two.html`).
18. **When** I click the finish button with `id="finish"`.
19. **Then** I should be on the checkout complete page (`/checkout-complete.html`).
20. **And** a header with the text "Thank you for your order!" must be displayed.
21. **When** I click the burger menu button with `id="react-burger-menu-btn"`.
22. **And** I click the logout link with `id="logout_sidebar_link"`.
23. **Then** I should be returned to the login page at `https://www.saucedemo.com`.
