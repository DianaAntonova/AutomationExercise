using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.LoginAndSecurityTests
{
          public class LoginWithInvalidPasswordTests
          {
                    private IWebDriver driver;

                    [SetUp]
                    public void Setup()
                    {
                              var options = new ChromeOptions();
                              options.AddArgument("--start-maximized");

                              driver = new ChromeDriver(options);
                              driver.Manage().Window.Maximize();
                              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                              driver.Navigate().GoToUrl("https://automationexercise.com/login");

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

                    [Test]
                    public void LoginWithInvalidPassword_ShouldShowErrorMessage()
                    {
                              // Enter valid email
                              driver.FindElement(By.XPath("//input[@data-qa='login-email']"))
                                    .SendKeys("diant@abv.bg");

                              // Enter INVALID password
                              driver.FindElement(By.XPath("//input[@data-qa='login-password']"))
                                    .SendKeys("WRONG_PASSWORD");

                              // Click Login button
                              driver.FindElement(By.XPath("//button[@data-qa='login-button']")).Click();

                              // Assertion: Verify incorrect credentials message appears
                              var errorMessage = driver.FindElement(By.XPath("//p[contains(text(),'Your email or password is incorrect!')]")).Text;

                              Assert.That(
                                  errorMessage.Contains("Your email or password is incorrect!"),
                                  "Expected error message was not displayed for invalid password."
                              );
                    }
          }
}
