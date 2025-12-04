using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationExercise.SignupValidationTests
{
          [Category("Signup")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_SignupWithInvalidZipCharactersTests
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
                    public void KNOWNISSUE_SignupWithInvalidZipCharacters_ShouldShowValidationError()
                    {
                              // Generate random valid user for the signup
                              string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);
                              string userName = $"TestUser{randomPart}";
                              string email = $"{userName.ToLower()}@test.bg";

                              // STEP 1: initial signup page
                              driver.FindElement(By.XPath("//input[@data-qa='signup-name']")).SendKeys(userName);
                              driver.FindElement(By.XPath("//input[@data-qa='signup-email']")).SendKeys(email);
                              driver.FindElement(By.XPath("//button[@data-qa='signup-button']")).Click();

                              // STEP 2: Account Information — fill everything normally except ZIP

                              // Select Title
                              var mrRadio = driver.FindElements(By.Id("id_gender1"));
                              if (mrRadio.Count > 0)
                              {
                                        mrRadio[0].Click();
                              }

                              // Valid password
                              driver.FindElement(By.Id("password")).SendKeys("ValidPass123!");

                              // Address fields (all valid EXCEPT Zipcode)
                              driver.FindElement(By.Id("first_name")).SendKeys("TestFirst");
                              driver.FindElement(By.Id("last_name")).SendKeys("TestLast");
                              driver.FindElement(By.Id("address1")).SendKeys("Test Street 123");

                              driver.FindElement(By.Id("state")).SendKeys("Sofia");
                              driver.FindElement(By.Id("city")).SendKeys("Sofia");

                              // INVALID ZIP CODE → BUG SCENARIO
                              string invalidZip = "///////";
                              driver.FindElement(By.Id("zipcode")).SendKeys(invalidZip);

                              driver.FindElement(By.Id("mobile_number")).SendKeys("028889900");

                              // Submit the form
                              driver.FindElement(By.XPath("//button[@data-qa='create-account']")).Click();

                              // EXPECTATION:
                              // Zip code with invalid characters must NOT be accepted.
                              string forbiddenUrl = "https://automationexercise.com/account_created";
                              string actualUrl = driver.Url;

                              if (actualUrl.StartsWith(forbiddenUrl, StringComparison.OrdinalIgnoreCase))
                              {
                                        Console.WriteLine(
                                            $"KNOWN ISSUE: System allowed account creation with INVALID ZIP code '{invalidZip}'. " +
                                            $"Expected validation to block submission, but user was redirected to '{actualUrl}'."
                                        );
                              }

                              Assert.That(
                                  actualUrl.StartsWith(forbiddenUrl, StringComparison.OrdinalIgnoreCase),
                                  Is.False,
                                  $"KNOWN ISSUE: Account was created even though ZIP contained invalid characters ('{invalidZip}'). " +
                                  $"Expected NOT to reach '{forbiddenUrl}', but actual URL is '{actualUrl}'."
                              );
                    }
          }
}
