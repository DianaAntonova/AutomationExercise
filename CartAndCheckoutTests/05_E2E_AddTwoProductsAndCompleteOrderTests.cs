using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.CartAndCheckoutTests
{
          public class E2EAddTwoProductsAndCompleteOrderTests
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

                              driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
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
                                  .ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
                    }

                    [Test]
                    public void E2EAddTwoProductsAndCompleteOrder_ShouldPlaceOrderSuccessfully()
                    {
                              // 1. Navigate to Login page
                              driver.FindElement(By.CssSelector("a[href='/login']")).Click();

                              // 2. Login
                              driver.FindElement(By.CssSelector("input[data-qa='login-email']")).SendKeys("diant@abv.bg");
                              driver.FindElement(By.CssSelector("input[data-qa='login-password']")).SendKeys("didi123456");
                              driver.FindElement(By.CssSelector("button[data-qa='login-button']")).Click();

                              // Verify login
                              IWebElement loggedInAs = driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));
                              Assert.That(loggedInAs.Displayed, "User is not logged in.");

                              // 3. Navigate to Products
                              driver.FindElement(By.CssSelector("a[href='/products']")).Click();
                              ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 500);");

                              // 4. Add first product (id=1)
                              IWebElement addToCartBtn1 =
                                  driver.FindElement(By.CssSelector("a[data-product-id='1'].add-to-cart"));
                              ScrollToElement(addToCartBtn1);
                              addToCartBtn1.Click();

                              // 5. Continue Shopping
                              driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal")).Click();

                              // 6. Go to Brand → POLO
                              IWebElement poloBrand = driver.FindElement(By.CssSelector("a[href='/brand_products/Polo']"));
                              ScrollToElement(poloBrand);
                              poloBrand.Click();

                              // 7. Add POLO product (id=30)
                              IWebElement addToCartBtnPolo =
                                  driver.FindElement(By.CssSelector("a[data-product-id='30'].add-to-cart"));
                              ScrollToElement(addToCartBtnPolo);
                              addToCartBtnPolo.Click();

                              // 8. Continue Shopping
                              driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal")).Click();

                              // 9. Go to CART
                              driver.FindElement(By.CssSelector("a[href='/view_cart']")).Click();

                              // Verify both items exist
                              Assert.That(driver.FindElement(By.Id("product-1")).Displayed, "Product 1 missing.");
                              Assert.That(driver.FindElement(By.Id("product-30")).Displayed, "Product 30 missing.");

                              // 10. Proceed to checkout
                              driver.FindElement(By.CssSelector("a.btn.btn-default.check_out")).Click();

                              // 11. Scroll to "Place Order"
                              IWebElement placeOrderButton =
                                  driver.FindElement(By.CssSelector("a[href='/payment'].btn.btn-default.check_out"));
                              ScrollToElement(placeOrderButton);
                              placeOrderButton.Click();

                              // 12. Scroll to payment button
                              IWebElement payAndConfirmButton =
                                  driver.FindElement(By.CssSelector("button[data-qa='pay-button']"));
                              ScrollToElement(payAndConfirmButton);

                              // 13. Fill new card details (from mockcard.click)
                              driver.FindElement(By.Name("name_on_card")).SendKeys("Sarah Brown");
                              driver.FindElement(By.Name("card_number")).SendKeys("4958425467278829");
                              driver.FindElement(By.Name("cvc")).SendKeys("859");
                              driver.FindElement(By.Name("expiry_month")).SendKeys("12");
                              driver.FindElement(By.Name("expiry_year")).SendKeys("2029");

                              // 14. Confirm payment
                              payAndConfirmButton.Click();

                              // 15. Verify order success
                              IWebElement orderPlacedMessage =
                                  driver.FindElement(By.XPath("//b[text()='Order Placed!']"));
                              Assert.That(orderPlacedMessage.Displayed, "Order was NOT placed successfully!");
                    }
          }
}
