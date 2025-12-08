using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.LoginAndSecurityTests
{
          [Category("Security")]
          public class KNOWNISSUE_LoginWithMultipleWrongAttemptsTests
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
                    public void KNOWNISSUE_LoginWithMultipleWrongAttempts_ShouldShowLockoutOrWarning()
                    {
                              const int maxAttempts = 5;
                              bool isLockedOrWarningShown = false;
                              string lastErrorText = string.Empty;

                              for (int i = 0; i < maxAttempts; i++)
                              {
                                        var emailField = driver.FindElement(By.XPath("//input[@data-qa='login-email']"));
                                        var passwordField = driver.FindElement(By.XPath("//input[@data-qa='login-password']"));
                                        var loginButton = driver.FindElement(By.XPath("//button[@data-qa='login-button']"));

                                        emailField.Clear();
                                        passwordField.Clear();

                                        emailField.SendKeys("diant@abv.bg");
                                        passwordField.SendKeys("wrong_password");

                                        loginButton.Click();

                                        // Check for specific lockout/warning messages
                                        var lockoutMessages = driver.FindElements(
                                            By.XPath(
                                                "//p[contains(.,'too many attempts') or " +
                                                "contains(.,'locked') or " +
                                                "contains(.,'temporarily disabled') or " +
                                                "contains(.,'suspended')]"
                                            ));

                                        if (lockoutMessages.Count > 0)
                                        {
                                                  isLockedOrWarningShown = true;
                                                  break;
                                        }

                                        // Capture the last generic error message if present
                                        var genericError = driver.FindElements(
                                            By.XPath("//p[contains(text(),'Your email or password is incorrect!')]"));
                                        if (genericError.Count > 0)
                                        {
                                                  lastErrorText = genericError[0].Text;
                                        }
                              }

                              if (!isLockedOrWarningShown)
                              {
                                        Console.WriteLine(
                                            $"The account did NOT lock after {maxAttempts} invalid password attempts. " +
                                            $"Last system message: '{lastErrorText}'");
                              }

                              Assert.That(
                                  isLockedOrWarningShown,
                                  $"SECURITY ISSUE: The account did NOT lock or display a warning after {maxAttempts} invalid login attempts. " +
                                  "This test intentionally fails until proper lockout logic is implemented."
                              );
                    }
          }
}
