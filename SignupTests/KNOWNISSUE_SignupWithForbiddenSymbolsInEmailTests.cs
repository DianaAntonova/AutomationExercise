using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationExercise.SignupValidationTests
{
          [Category("Signup")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_SignupWithForbiddenSymbolsInEmailTests
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
                    public void KNOWNISSUE_SignupWithEmailContainingDollar_ShouldNotAllowAccountCreation()
                    {
                              RunForbiddenSymbolEmailTest('$');
                    }

                    [Test]
                    public void KNOWNISSUE_SignupWithEmailContainingAmpersand_ShouldNotAllowAccountCreation()
                    {
                              RunForbiddenSymbolEmailTest('&');
                    }

                    [Test]
                    public void SignupWithEmailContainingOpeningParenthesis_ShouldNotAllowAccountCreation()
                    {
                              RunForbiddenSymbolEmailTest('(');
                              //system prevents using this symbol
                    }

                    [Test]
                    public void SignupWithEmailContainingComma_ShouldNotAllowAccountCreation()
                    {
                              RunForbiddenSymbolEmailTest(',');
                              //system prevents using this symbol

                    }

                    [Test]
                    public void SignupWithEmailContainingColon_ShouldNotAllowAccountCreation()
                    {
                              RunForbiddenSymbolEmailTest(':');
                              //system prevents using this symbol

                    }

                    /// <summary>
                    /// Helper method that performs the actual signup attempt
                    /// with a single forbidden symbol in the email address.
                    /// </summary>
                    private void RunForbiddenSymbolEmailTest(char forbiddenSymbol)
                    {
                              // Create email using the forbidden symbol
                              string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);
                              string email = $"user{randomPart}{forbiddenSymbol}test@test.bg";

                              // Fill signup form
                              var nameInput = driver.FindElement(By.XPath("//input[@data-qa='signup-name']"));
                              var emailInput = driver.FindElement(By.XPath("//input[@data-qa='signup-email']"));

                              nameInput.Clear();
                              emailInput.Clear();

                              nameInput.SendKeys("Forbidden Symbol Email User");
                              emailInput.SendKeys(email);

                              // Click Signup button
                              driver.FindElement(By.XPath("//button[@data-qa='signup-button']")).Click();

                              // EXPECTATION:
                              // Invalid email → user should remain on login/signup page
                              string expectedUrl = "https://automationexercise.com/login";
                              string actualUrl = driver.Url;

                              if (!actualUrl.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase))
                              {
                                        Console.WriteLine(
                                            $"KNOWN ISSUE: System incorrectly accepts an email containing forbidden character '{forbiddenSymbol}'. " +
                                            $"Expected to remain on '{expectedUrl}', but was redirected to '{actualUrl}'. " +
                                            $"Attempted email: {email}"
                                        );
                              }

                              Assert.That(
                                  actualUrl.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase),
                                  Is.True,
                                  $"KNOWN ISSUE: Email containing forbidden character '{forbiddenSymbol}' DID NOT trigger validation. " +
                                  $"System incorrectly processed email '{email}'. Expected to stay on '{expectedUrl}', but was '{actualUrl}'."
                              );
                    }
          }
}
