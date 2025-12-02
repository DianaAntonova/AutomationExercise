
using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.CartAndCheckoutTests
{
          public class E2EMultiCategoryCheckoutTests
          {
                    public IWebDriver driver;

                    [SetUp]
                    public void Setup()
                    {
                              var options = new ChromeOptions();
                              options.AddUserProfilePreference("credentials_enable_service", false);
                              options.AddUserProfilePreference("profile.password_manager_enabled", false);

                              options.AddArgument("--disable-notifications");
                              options.AddArgument("--disable-infobars");
                              options.AddArgument("--disable-save-password-bubble");
                              options.AddArgument("--disable-popup-blocking");

                              driver = new ChromeDriver(options);
                              driver.Manage().Window.Maximize();
                              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                              driver.Navigate().GoToUrl("https://automationexercise.com/");

                              var cookieButtons = driver.FindElements(By.XPath("//p[@class='fc-button-label']"));
                              if (cookieButtons.Count > 0)
                                        cookieButtons[0].Click();
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
                    public void E2EMultiCategoryCheckout_ShouldPlaceOrderSuccessfully()
                    {
                              // LOGIN
                              driver.FindElement(By.CssSelector("a[href='/login']")).Click();

                              IWebElement email = driver.FindElement(By.CssSelector("input[data-qa='login-email']"));
                              ScrollToElement(email);
                              email.SendKeys("diant@abv.bg");

                              IWebElement pass = driver.FindElement(By.CssSelector("input[data-qa='login-password']"));
                              ScrollToElement(pass);
                              pass.SendKeys("didi123456");

                              IWebElement loginBtn = driver.FindElement(By.CssSelector("button[data-qa='login-button']"));
                              ScrollToElement(loginBtn);
                              loginBtn.Click();

                              Assert.That(driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]")).Displayed);

                              // NAVIGATE TO PRODUCTS
                              driver.FindElement(By.CssSelector("a[href='/products']")).Click();



                              // ADD PRODUCT 1
                              ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 500);");
                              IWebElement add1 = driver.FindElement(By.CssSelector("a[data-product-id='1'].add-to-cart"));
                              ScrollToElement(add1);
                              add1.Click();

                              IWebElement continue1 = driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal"));
                              ScrollToElement(continue1);
                              continue1.Click();

                              // GO TO POLO BRAND
                              IWebElement poloBrand = driver.FindElement(By.CssSelector("a[href='/brand_products/Polo']"));
                              ScrollToElement(poloBrand);
                              poloBrand.Click();

                              // ADD PRODUCT 30 (POLO)
                              IWebElement add30 = driver.FindElement(By.CssSelector("a[data-product-id='30'].add-to-cart"));
                              ScrollToElement(add30);
                              add30.Click();

                              IWebElement continue2 = driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal"));
                              ScrollToElement(continue2);
                              continue2.Click();

                              // GO TO CART
                              IWebElement viewCart = driver.FindElement(By.CssSelector("a[href='/view_cart']"));
                              ScrollToElement(viewCart);
                              viewCart.Click();

                              // Scroll to show entire cart page
                              IWebElement poloTitle = driver.FindElement(By.CssSelector("a[href='/product_details/30']"));
                              ScrollToElement(poloTitle);

                              // DELETE PRODUCT 1
                              IWebElement deleteFirst = driver.FindElement(By.CssSelector(".cart_delete i.fa-times"));
                              ScrollToElement(deleteFirst);
                              deleteFirst.Click();

                              // BACK TO PRODUCTS
                              IWebElement productsAgain = driver.FindElement(By.CssSelector("a[href='/products']"));
                              ScrollToElement(productsAgain);
                              productsAgain.Click();

                              // GO TO KOOKIE KIDS
                              IWebElement kookieKids = driver.FindElement(By.CssSelector("a[href='/brand_products/Kookie Kids']"));
                              ScrollToElement(kookieKids);
                              kookieKids.Click();

                              // ADD FIRST PRODUCT FROM KOOKIE KIDS
                              IWebElement addKookie = driver.FindElement(By.CssSelector("a.add-to-cart"));
                              ScrollToElement(addKookie);
                              addKookie.Click();

                              IWebElement continue3 = driver.FindElement(By.CssSelector("button.btn.btn-success.close-modal"));
                              ScrollToElement(continue3);
                              continue3.Click();

                              // GO TO CART AGAIN
                              IWebElement viewCart2 = driver.FindElement(By.CssSelector("a[href='/view_cart']"));
                              ScrollToElement(viewCart2);
                              viewCart2.Click();

                              // CHECKOUT
                              IWebElement checkout = driver.FindElement(By.CssSelector("a.btn.btn-default.check_out"));
                              ScrollToElement(checkout);
                              checkout.Click();

                              // PLACE ORDER
                              IWebElement placeOrder = driver.FindElement(By.CssSelector("a[href='/payment'].btn.btn-default.check_out"));
                              ScrollToElement(placeOrder);
                              placeOrder.Click();

                              // PAYMENT PAGE — scroll to pay button
                              IWebElement payBtn = driver.FindElement(By.CssSelector("button[data-qa='pay-button']"));
                              ScrollToElement(payBtn);

                              // FILL CARD DATA
                              driver.FindElement(By.Name("name_on_card")).SendKeys("Sarah Brown");
                              driver.FindElement(By.Name("card_number")).SendKeys("4958425467278829");
                              driver.FindElement(By.Name("cvc")).SendKeys("859");
                              driver.FindElement(By.Name("expiry_month")).SendKeys("12");
                              driver.FindElement(By.Name("expiry_year")).SendKeys("2029");

                              // CONFIRM ORDER
                              ScrollToElement(payBtn);
                              payBtn.Click();

                              // VERIFY SUCCESS
                              IWebElement orderPlaced =
                                  driver.FindElement(By.XPath("//b[text()='Order Placed!']"));

                              Assert.That(orderPlaced.Displayed, "Order was NOT placed successfully!");
                    }
          }
}
