using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.LoginAndSecurityTests
{
          public class E2ECartPersistsAfterLogoutLoginTests
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
                    public void CartItemsPersistAfterLogoutAndLogin_ShouldKeepProductsInCart()
                    {
                              // ================================
                              // STEP 1 – LOGIN WITH VALID USER
                              // ================================
                              IWebElement loginLink = driver.FindElement(By.CssSelector("a[href='/login']"));
                              ScrollToElement(loginLink);
                              loginLink.Click();

                              IWebElement emailInput =
                                  driver.FindElement(By.CssSelector("input[data-qa='login-email']"));
                              ScrollToElement(emailInput);
                              emailInput.SendKeys("diant@abv.bg");

                              IWebElement passwordInput =
                                  driver.FindElement(By.CssSelector("input[data-qa='login-password']"));
                              ScrollToElement(passwordInput);
                              passwordInput.SendKeys("didi123456");

                              IWebElement loginButton =
                                  driver.FindElement(By.CssSelector("button[data-qa='login-button']"));
                              ScrollToElement(loginButton);
                              loginButton.Click();

                              IWebElement loggedInAs =
                                  driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));
                              ScrollToElement(loggedInAs);
                              Assert.That(loggedInAs.Displayed, "User is not logged in after entering valid credentials.");

                              // ================================
                              // STEP 2 – ADD TWO PRODUCTS TO CART
                              // (reuse logic from E2EAddTwoProductsAndCompleteOrderTests)
                              // ================================

                              // Go to Products page
                              IWebElement productsLink = driver.FindElement(By.CssSelector("a[href='/products']"));
                              ScrollToElement(productsLink);
                              productsLink.Click();

                              // Pre-scroll so products grid loads
                              ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 500);");

                              // Add first product: product-id=1
                              IWebElement addProduct1Btn =
                                  driver.FindElement(By.CssSelector("a[data-product-id='1'].add-to-cart"));
                              ScrollToElement(addProduct1Btn);
                              addProduct1Btn.Click();

                              // Close modal (Continue Shopping)
                              IWebElement continueShoppingBtn1 =
                                  driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal"));
                              ScrollToElement(continueShoppingBtn1);
                              continueShoppingBtn1.Click();

                              // Go to brand POLO
                              IWebElement poloBrandLink =
                                  driver.FindElement(By.CssSelector("a[href='/brand_products/Polo']"));
                              ScrollToElement(poloBrandLink);
                              poloBrandLink.Click();

                              // Add second product: product-id=30 (POLO)
                              IWebElement addProduct30Btn =
                                  driver.FindElement(By.CssSelector("a[data-product-id='30'].add-to-cart"));
                              ScrollToElement(addProduct30Btn);
                              addProduct30Btn.Click();

                              // Close modal (Continue Shopping)
                              IWebElement continueShoppingBtn2 =
                                  driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal"));
                              ScrollToElement(continueShoppingBtn2);
                              continueShoppingBtn2.Click();

                              // ================================
                              // STEP 3 – OPEN CART AND VERIFY BOTH PRODUCTS
                              // ================================
                              IWebElement cartLink = driver.FindElement(By.CssSelector("a[href='/view_cart']"));
                              ScrollToElement(cartLink);
                              cartLink.Click();

                              // Verify product 1 is present
                              IWebElement product1Row = driver.FindElement(By.Id("product-1"));
                              ScrollToElement(product1Row);
                              Assert.That(product1Row.Displayed, "Product with id=1 is not present in the cart.");

                              // Verify product 30 is present
                              IWebElement product30Row = driver.FindElement(By.Id("product-30"));
                              ScrollToElement(product30Row);
                              Assert.That(product30Row.Displayed, "Product with id=30 is not present in the cart.");

                              // ================================
                              // STEP 4 – LOGOUT
                              // ================================
                              IWebElement logoutLink =
                                  driver.FindElement(By.CssSelector("a[href='/logout']"));
                              ScrollToElement(logoutLink);
                              logoutLink.Click();

                              // Verify logout by checking that "Signup / Login" is visible again
                              IWebElement signupLoginLink =
                                  driver.FindElement(By.CssSelector("a[href='/login']"));
                              ScrollToElement(signupLoginLink);
                              Assert.That(signupLoginLink.Displayed, "User is not logged out correctly.");

                              // ================================
                              // STEP 5 – LOGIN AGAIN WITH SAME USER
                              // ================================
                              ScrollToElement(signupLoginLink);
                              signupLoginLink.Click();

                              IWebElement emailInput2 =
                                  driver.FindElement(By.CssSelector("input[data-qa='login-email']"));
                              ScrollToElement(emailInput2);
                              emailInput2.SendKeys("diant@abv.bg");

                              IWebElement passwordInput2 =
                                  driver.FindElement(By.CssSelector("input[data-qa='login-password']"));
                              ScrollToElement(passwordInput2);
                              passwordInput2.SendKeys("didi123456");

                              IWebElement loginButton2 =
                                  driver.FindElement(By.CssSelector("button[data-qa='login-button']"));
                              ScrollToElement(loginButton2);
                              loginButton2.Click();

                              IWebElement loggedInAsAgain =
                                  driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));
                              ScrollToElement(loggedInAsAgain);
                              Assert.That(loggedInAsAgain.Displayed, "User is not logged in after second login.");

                              // ================================
                              // STEP 6 – OPEN CART AGAIN AND VERIFY PRODUCTS STILL EXIST
                              // ================================
                              IWebElement cartLinkAgain = driver.FindElement(By.CssSelector("a[href='/view_cart']"));
                              ScrollToElement(cartLinkAgain);
                              cartLinkAgain.Click();

                              // Product 1 should still be present
                              IWebElement product1RowAfterRelogin = driver.FindElement(By.Id("product-1"));
                              ScrollToElement(product1RowAfterRelogin);
                              Assert.That(
                                  product1RowAfterRelogin.Displayed,
                                  "Product with id=1 is missing from cart after logout/login."
                              );

                              // Product 30 should still be present
                              IWebElement product30RowAfterRelogin = driver.FindElement(By.Id("product-30"));
                              ScrollToElement(product30RowAfterRelogin);
                              Assert.That(
                                  product30RowAfterRelogin.Displayed,
                                  "Product with id=30 is missing from cart after logout/login."
                              );
                    }
          }
}
