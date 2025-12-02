using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Xml.Linq;

namespace AutomationExercise.LoginAndSecurityTests
{
          public class LoginWithValidCredentialsTests
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
                    public void LoginWithValidCredentials_ShouldRedirectToHomePage()
                    {

                              // Enter valid login email
                              driver.FindElement(By.XPath("//input[@data-qa='login-email']"))
                                    .SendKeys("diant@abv.bg");

                              // Enter valid password
                              driver.FindElement(By.XPath("//input[@data-qa='login-password']"))
                                    .SendKeys("didi123456");

                              // Click Login button
                              driver.FindElement(By.XPath("//button[@data-qa='login-button']")).Click();

                              // Assertion: Verify "Logged in as" header appears
                              var loggedInHeader = driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]")).Text;
                              
                              Assert.That(loggedInHeader.Contains("Logged in as"), "Login was not successful or 'Logged in as' header not found.");    



                    }
          }

}
