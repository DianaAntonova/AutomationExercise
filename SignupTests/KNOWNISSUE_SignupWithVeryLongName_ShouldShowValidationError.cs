using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationExercise.SignupValidationTests
{
          [Category("Signup")]
          [Category("KnownIssue")]
          public class KNOWNISSUESignupWithVeryLongName_ShouldShowValidationError
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
                    public void KNOWNISSUE_SignupWithVeryLongName_ShouldShowValidationError()
                    {
                              // Generate long invalid username (50 characters)
                              string longName = new string('A', 50);

                              // Generate unique email
                              string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);
                              string email = $"longname{randomPart}@test.bg";

                              // Fill the signup fields
                              var nameInput = driver.FindElement(By.XPath("//input[@data-qa='signup-name']"));
                              var emailInput = driver.FindElement(By.XPath("//input[@data-qa='signup-email']"));

                              nameInput.SendKeys(longName);
                              emailInput.SendKeys(email);

                              // Click Signup button
                              driver.FindElement(By.XPath("//button[@data-qa='signup-button']")).Click();

                              // EXPECTATION:
                              // System should NOT allow a 50-character name and should keep user on login/signup page
                              string expectedUrl = "https://automationexercise.com/login";
                              string actualUrl = driver.Url;

                              // Print info if the system incorrectly allows the long name
                              if (!actualUrl.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase))
                              {
                                        Console.WriteLine(
                                            "KNOWN ISSUE: System allows progressing to 'Enter Account Information' with a very long name (50 chars). " +
                                            $"Expected to stay on '{expectedUrl}', but was redirected to '{actualUrl}'."
                                        );
                              }

                              // Test must fail until validation is implemented
                              Assert.That(
                                  actualUrl.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase),
                                  Is.True,
                                  "KNOWN ISSUE: User did NOT remain on the login/signup page after submitting a 50-character name. " +
                                  $"Expected URL to start with '{expectedUrl}', but was '{actualUrl}'. " +
                                  "Proper name length validation must be implemented."
                              );
                    }
          }
}
