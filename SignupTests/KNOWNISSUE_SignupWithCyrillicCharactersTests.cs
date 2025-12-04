using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationExercise.SignupValidationTests
{
          [Category("Signup")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_SignupWithCyrillicCharactersTests
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
                    public void KNOWNISSUE_SignupWithCyrillicCharacters_ShouldShowValidationError()
                    {
                              // Cyrillic-only name (I18N / character-set test)
                              string cyrillicName = "ТестПотребителКирилица";

                              // Generate unique email to avoid duplicates
                              string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);
                              string email = $"cyrillic{randomPart}@test.bg";

                              // Fill the signup fields
                              var nameInput = driver.FindElement(By.XPath("//input[@data-qa='signup-name']"));
                              var emailInput = driver.FindElement(By.XPath("//input[@data-qa='signup-email']"));

                              nameInput.SendKeys(cyrillicName);   // ✖ Cyrillic characters
                              emailInput.SendKeys(email);

                              // Click Signup button
                              driver.FindElement(By.XPath("//button[@data-qa='signup-button']")).Click();

                              // EXPECTATION (spec AD-T66):
                              // Cyrillic should be rejected → user should remain on login/signup page.
                              string expectedUrl = "https://automationexercise.com/login";
                              string actualUrl = driver.Url;

                              if (!actualUrl.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase))
                              {
                                        Console.WriteLine(
                                            "KNOWN ISSUE: System accepts Cyrillic-only name during signup and redirects to account creation. " +
                                            $"Expected to stay on '{expectedUrl}', but was redirected to '{actualUrl}'. " +
                                            $"Used name: '{cyrillicName}', email: '{email}'."
                                        );
                              }

                              Assert.That(
                                  actualUrl.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase),
                                  Is.True,
                                  "KNOWN ISSUE: Signup with Cyrillic characters in the Name field did NOT trigger validation. " +
                                  $"Expected URL to start with '{expectedUrl}', but was '{actualUrl}'. " +
                                  "Proper character-set validation for the Name field must be implemented."
                              );
                    }
          }
}
