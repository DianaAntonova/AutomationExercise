using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.SignupValidationTests
{
          [Category("Signup")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_SignupWithSpacesOnlyNameTests
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
                    public void KNOWNISSUE_SignupWithSpacesOnlyName_ShouldNotAllowAccountCreation()
                    {
                              // Generate unique email
                              string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);
                              string email = $"spacesonly{randomPart}@test.bg";

                              // Fill the signup fields with invalid name (spaces only)
                              var nameInput = driver.FindElement(By.XPath("//input[@data-qa='signup-name']"));
                              var emailInput = driver.FindElement(By.XPath("//input[@data-qa='signup-email']"));

                              nameInput.SendKeys("     ");  // ✖ spaces-only name
                              emailInput.SendKeys(email);

                              // Click Signup button
                              driver.FindElement(By.XPath("//button[@data-qa='signup-button']")).Click();

                              // After invalid input we EXPECT to stay on the login/signup page
                              var expectedUrl = "https://automationexercise.com/login";
                              var actualUrl = driver.Url;

                              if (!actualUrl.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase))
                              {
                                        Console.WriteLine(
                                            "KNOWN ISSUE: After submitting a spaces-only name, the user is redirected away from the login page. " +
                                            $"Expected to stay on '{expectedUrl}', but was redirected to '{actualUrl}'."
                                        );
                              }

                              Assert.That(
                                  actualUrl.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase),
                                  Is.True,
                                  $"KNOWN ISSUE: User did NOT remain on the login/signup page after submitting a spaces-only name. " +
                                  $"Expected URL to start with '{expectedUrl}', but was '{actualUrl}'."
                              );
                    }
          }
}
