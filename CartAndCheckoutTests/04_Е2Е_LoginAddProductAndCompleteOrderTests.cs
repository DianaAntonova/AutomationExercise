using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.CartAndCheckoutTests
{
          public class E2ELoginAddProductAndCompleteOrderTests
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

                    /// <summary>
                    /// Helper method to scroll the page until the given element is in view.
                    /// </summary>
                    private void ScrollToElement(IWebElement element)
                    {
                              ((IJavaScriptExecutor)driver)
                                  .ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
                    }

                    [Test]
                    public void E2ELoginAddProductAndCompleteOrder_ShouldPlaceOrderSuccessfully()
                    {
                              // 1. Navigate to Login page
                              driver.FindElement(By.CssSelector("a[href='/login']")).Click();

                              // 2. Login with valid credentials
                              driver.FindElement(By.CssSelector("input[data-qa='login-email']"))
                                    .SendKeys("diant@abv.bg");

                              driver.FindElement(By.CssSelector("input[data-qa='login-password']"))
                                    .SendKeys("didi123456");

                              driver.FindElement(By.CssSelector("button[data-qa='login-button']")).Click();

                              // Verify user is logged in
                              IWebElement loggedInAs =
                                  driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));
                              Assert.That(loggedInAs.Displayed, "User is not logged in after entering valid credentials.");

                              // 3. Navigate to Products page
                              driver.FindElement(By.CssSelector("a[href='/products']")).Click();

                              // 4. Pre-scroll a bit so the products grid starts loading
                              ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 500);");

                              // 5. Locate the specific "Add to cart" button for product-id=1
                              IWebElement addToCartBtn =
                                  driver.FindElement(By.CssSelector("a[data-product-id='1'].add-to-cart"));

                              // Scroll until this button is visible in the viewport
                              ScrollToElement(addToCartBtn);

                              // 6. Click "Add to cart" for product 1
                              addToCartBtn.Click();

                              // 7. In the modal, click "Continue Shopping"
                              IWebElement continueShoppingButton =
                                  driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal"));
                              continueShoppingButton.Click();

                              // 8. Go to Cart page
                              driver.FindElement(By.CssSelector("a[href='/view_cart']")).Click();

                              // Verify product with id=1 is present in the cart
                              IWebElement productRow = driver.FindElement(By.Id("product-1"));
                              Assert.That(productRow.Displayed, "Product ID=1 was not found in the cart.");

                              // 9. Proceed to checkout
                              driver.FindElement(By.CssSelector("a.btn.btn-default.check_out")).Click();
                             
                              // 10. Scroll to "Place Order" button and click it
                              IWebElement placeOrderButton =
                                  driver.FindElement(By.CssSelector("a[href='/payment'].btn.btn-default.check_out"));

                              // Scroll until the button is visible
                              ScrollToElement(placeOrderButton);

                              // Click the button
                              placeOrderButton.Click();


                              // 11. Fill payment details
                              // Card generated from https://mockcard.click/?utm_source=chatgpt.com
                              IWebElement payAndConfirmButton =
                driver.FindElement(By.CssSelector("button[data-qa='pay-button']"));
                              ScrollToElement(payAndConfirmButton);

                              driver.FindElement(By.Name("name_on_card")).SendKeys("John Wilson");
                              driver.FindElement(By.Name("card_number")).SendKeys("4791636081493211");
                              driver.FindElement(By.Name("cvc")).SendKeys("191");
                              driver.FindElement(By.Name("expiry_month")).SendKeys("12");
                              driver.FindElement(By.Name("expiry_year")).SendKeys("2027");

                              // 12. Pay and confirm order
                              driver.FindElement(By.CssSelector("button[data-qa='pay-button']")).Click();

                              // 13. Verify success message "Order Placed!"
                              IWebElement orderPlacedMessage =
                                  driver.FindElement(By.XPath("//b[text()='Order Placed!']"));
                              Assert.That(orderPlacedMessage.Displayed,
                                  "Order success message 'Order Placed!' was NOT displayed.");
                    }
          }
}
