using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise
{
          public class E2EGuestRegisterDuringCheckoutTests
          {
                    public IWebDriver driver;

                    [SetUp]
                    public void Setup()
                    {
                              var options = new ChromeOptions();

                              // Disable Chrome password manager / save password popups
                              options.AddUserProfilePreference("credentials_enable_service", false);
                              options.AddUserProfilePreference("profile.password_manager_enabled", false);

                              // Disable various browser popups/notifications
                              options.AddArgument("--disable-notifications");
                              options.AddArgument("--disable-infobars");
                              options.AddArgument("--disable-save-password-bubble");
                              options.AddArgument("--disable-popup-blocking");

                              driver = new ChromeDriver(options);
                              driver.Manage().Window.Maximize();
                              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                              // Open homepage
                              driver.Navigate().GoToUrl("https://automationexercise.com/");

                              // Accept cookies popup if present
                              var cookieButtons = driver.FindElements(By.XPath("//p[@class='fc-button-label']"));
                              if (cookieButtons.Count > 0)
                              {
                                        cookieButtons[0].Click();
                              }
                    }

                    [TearDown]
                    public void TearDown()
                    {
                              driver.Quit();
                              driver.Dispose();
                    }

                    private void ScrollToElement(IWebElement element)
                    {
                              ((IJavaScriptExecutor)driver)
                                  .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
                    }

                    [Test]
                    public void GuestUserCreatesAccountDuringCheckoutAndCompletesOrder_ShouldPlaceOrderSuccessfully()
                    {
                              // ================================
                              // STEP 1 – OPEN PRODUCTS PAGE
                              // ================================
                              IWebElement productsLink = driver.FindElement(By.CssSelector("a[href='/products']"));
                              ScrollToElement(productsLink);
                              productsLink.Click();

                              // ================================
                              // STEP 2 – ADD PRODUCT ID=3 & VIEW CART
                              // ================================
                              ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 500);");

                              IWebElement addSleevelessDressBtn =
                                  driver.FindElement(By.CssSelector("a[data-product-id='3'].add-to-cart"));
                              ScrollToElement(addSleevelessDressBtn);
                              addSleevelessDressBtn.Click();

                              // Click "Continue Shopping" in modal
                              IWebElement continueShoppingBtn =
                                  driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal.btn-block"));
                              ScrollToElement(continueShoppingBtn);
                              continueShoppingBtn.Click();

                              // Now go to Cart
                              IWebElement viewCartButton =
                                  driver.FindElement(By.CssSelector("a[href='/view_cart']"));
                              ScrollToElement(viewCartButton);
                              viewCartButton.Click();

                              // Verify product in cart (Sleeveless Dress)
                              IWebElement cartProduct =
                                  driver.FindElement(By.XPath("//a[contains(text(),'Sleeveless Dress')]"));
                              ScrollToElement(cartProduct);
                              Assert.That(cartProduct.Displayed, "Sleeveless Dress NOT found in cart.");

                              IWebElement cartTotal =
                                  driver.FindElement(By.XPath("//td[@class='cart_total']/p"));
                              ScrollToElement(cartTotal);
                              Assert.That(cartTotal.Displayed, "Cart total not visible.");

                              // ================================
                              // STEP 3 – PROCEED TO CHECKOUT AS GUEST
                              // ================================
                              IWebElement proceedToCheckout =
                                  driver.FindElement(By.CssSelector("a.btn.btn-default.check_out"));
                              ScrollToElement(proceedToCheckout);
                              proceedToCheckout.Click();

                              // Click "Register / Login"
                              IWebElement registerLoginLink =
                                  driver.FindElement(By.CssSelector("a[href='/login'] u"));
                              ScrollToElement(registerLoginLink);
                              registerLoginLink.Click();

                              // Verify that Login / Signup page opened
                              IWebElement newUserSignupHeader =
                                  driver.FindElement(By.XPath("//h2[text()='New User Signup!']"));
                              ScrollToElement(newUserSignupHeader);
                              Assert.That(
                                  newUserSignupHeader.Displayed,
                                  "Register/Login page did NOT open after clicking the link."
                              );

                              // ================================
                              // STEP 4 – CREATE ACCOUNT DURING CHECKOUT
                              // ================================
                              IWebElement nameInput = driver.FindElement(By.Name("name"));
                              ScrollToElement(nameInput);
                              nameInput.SendKeys("Diyana Antonova");

                              string uniqueEmail = $"guest_{Guid.NewGuid()}@example.com";

                              IWebElement signupEmail =
                                  driver.FindElement(By.CssSelector("input[data-qa='signup-email']"));
                              ScrollToElement(signupEmail);
                              signupEmail.SendKeys(uniqueEmail);

                              IWebElement signupBtn =
                                  driver.FindElement(By.CssSelector("button[data-qa='signup-button']"));
                              ScrollToElement(signupBtn);
                              signupBtn.Click();

                              // ENTER ACCOUNT INFO (без дата на раждане и без държава)
                              IWebElement accInfoHeader =
                                  driver.FindElement(By.XPath("//b[text()='Enter Account Information']"));
                              ScrollToElement(accInfoHeader);
                              Assert.That(
                                  accInfoHeader.Displayed,
                                  "Enter Account Information section is not visible."
                              );

                              IWebElement genderMrs = driver.FindElement(By.Id("id_gender2"));
                              ScrollToElement(genderMrs);
                              genderMrs.Click();

                              IWebElement passwordInput = driver.FindElement(By.Id("password"));
                              ScrollToElement(passwordInput);
                              passwordInput.SendKeys("Test1234");

                              // Address information (minimal required)
                              IWebElement firstName = driver.FindElement(By.Id("first_name"));
                              ScrollToElement(firstName);
                              firstName.SendKeys("Diyana");

                              IWebElement lastName = driver.FindElement(By.Id("last_name"));
                              ScrollToElement(lastName);
                              lastName.SendKeys("Antonova");

                              IWebElement address1 = driver.FindElement(By.Id("address1"));
                              ScrollToElement(address1);
                              address1.SendKeys("Test Address 1");

                              IWebElement state = driver.FindElement(By.Id("state"));
                              ScrollToElement(state);
                              state.SendKeys("Test State");

                              IWebElement city = driver.FindElement(By.Id("city"));
                              ScrollToElement(city);
                              city.SendKeys("Test City");

                              IWebElement zipcode = driver.FindElement(By.Id("zipcode"));
                              ScrollToElement(zipcode);
                              zipcode.SendKeys("1000");

                              IWebElement mobile = driver.FindElement(By.Id("mobile_number"));
                              ScrollToElement(mobile);
                              mobile.SendKeys("+359888000000");

                              IWebElement createAccBtn =
                                  driver.FindElement(By.CssSelector("button[data-qa='create-account']"));
                              ScrollToElement(createAccBtn);
                              createAccBtn.Click();

                              IWebElement accCreated =
                                  driver.FindElement(By.XPath("//b[text()='Account Created!']"));
                              ScrollToElement(accCreated);
                              Assert.That(accCreated.Displayed, "Account Created message is not displayed.");

                              IWebElement continueBtn =
                                  driver.FindElement(By.CssSelector("a[data-qa='continue-button']"));
                              ScrollToElement(continueBtn);
                              continueBtn.Click();

                              IWebElement loggedInAs =
                                  driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));
                              ScrollToElement(loggedInAs);
                              Assert.That(
                                  loggedInAs.Displayed,
                                  "User is not logged in after account creation."
                              );

                              // ================================
                              // STEP 5 – RETURN TO CHECKOUT SUMMARY
                              // ================================
                              IWebElement viewCartAgain =
                                  driver.FindElement(By.CssSelector("a[href='/view_cart']"));
                              ScrollToElement(viewCartAgain);
                              viewCartAgain.Click();

                              IWebElement cartProductAfterSignup =
                                  driver.FindElement(By.XPath("//a[contains(text(),'Sleeveless Dress')]"));
                              ScrollToElement(cartProductAfterSignup);
                              Assert.That(
                                  cartProductAfterSignup.Displayed,
                                  "Sleeveless Dress is missing from cart after registration."
                              );

                              IWebElement proceedToCheckout2 =
                                  driver.FindElement(By.CssSelector("a.btn.btn-default.check_out"));
                              ScrollToElement(proceedToCheckout2);
                              proceedToCheckout2.Click();

                              // ================================
                              // STEP 6 – PAYMENT
                              // ================================
                              IWebElement placeOrder =
                                  driver.FindElement(By.CssSelector("a[href='/payment'].btn.btn-default.check_out"));
                              ScrollToElement(placeOrder);
                              placeOrder.Click();

                              IWebElement payBtn =
                                  driver.FindElement(By.CssSelector("button[data-qa='pay-button']"));
                              ScrollToElement(payBtn);

                              IWebElement nameOnCard = driver.FindElement(By.Name("name_on_card"));
                              ScrollToElement(nameOnCard);
                              nameOnCard.SendKeys("Diyana Antonova");

                              IWebElement cardNumber = driver.FindElement(By.Name("card_number"));
                              ScrollToElement(cardNumber);
                              cardNumber.SendKeys("4111111111111111");

                              IWebElement cvc = driver.FindElement(By.Name("cvc"));
                              ScrollToElement(cvc);
                              cvc.SendKeys("123");

                              IWebElement expMonth = driver.FindElement(By.Name("expiry_month"));
                              ScrollToElement(expMonth);
                              expMonth.SendKeys("12");

                              IWebElement expYear = driver.FindElement(By.Name("expiry_year"));
                              ScrollToElement(expYear);
                              expYear.SendKeys("2027");

                              ScrollToElement(payBtn);
                              payBtn.Click();

                              // ================================
                              // STEP 7 – VERIFY SUCCESS
                              // ================================
                              IWebElement orderPlaced =
                                  driver.FindElement(By.XPath("//b[text()='Order Placed!']"));
                              ScrollToElement(orderPlaced);
                              Assert.That(
                                  orderPlaced.Displayed,
                                  "Order success message 'Order Placed!' was NOT displayed."
                              );
                    }
          }
}
