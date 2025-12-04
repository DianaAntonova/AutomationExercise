using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomationExercise.SignupValidationTests
{
          [Category("Signup")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_SignupWithShortPasswordTests
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
                    public void KNOWNISSUE_SignupWithShortPassword_ShouldShowValidationError()
                    {
                              // Arrange: generate random user name and email
                              string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);
                              string userName = $"TestUser{randomPart}";
                              string email = $"{userName.ToLower()}@test.bg";

                              // STEP 1: New User Signup (name + email)
                              var signupNameInput = driver.FindElement(By.XPath("//input[@data-qa='signup-name']"));
                              var signupEmailInput = driver.FindElement(By.XPath("//input[@data-qa='signup-email']"));

                              signupNameInput.SendKeys(userName);
                              signupEmailInput.SendKeys(email);

                              driver.FindElement(By.XPath("//button[@data-qa='signup-button']")).Click();

                              // STEP 2: Enter Account Information with VERY short password

                              // Title (optional, но е добре да изберем нещо)
                              var mrRadio = driver.FindElements(By.Id("id_gender1"));
                              if (mrRadio.Count > 0)
                              {
                                        mrRadio[0].Click();
                              }

                              // Password too short
                              driver.FindElement(By.Id("password")).SendKeys("1");

                              // Address Information – попълваме само задължителните полета
                              driver.FindElement(By.Id("first_name")).SendKeys("TestFirst");
                              driver.FindElement(By.Id("last_name")).SendKeys("TestLast");
                              driver.FindElement(By.Id("address1")).SendKeys("Test Street 123");

                              driver.FindElement(By.Id("state")).SendKeys("TestState");
                              driver.FindElement(By.Id("city")).SendKeys("TestCity");
                              driver.FindElement(By.Id("zipcode")).SendKeys("1000");
                              driver.FindElement(By.Id("mobile_number")).SendKeys("0888123456");

                              // Click "Create Account"
                              driver.FindElement(By.XPath("//button[@data-qa='create-account']")).Click();

                              // EXPECTATION:
                              // With such a short password ('1'), account MUST NOT be created.
                              // User should NOT be on /account_created page.
                              string forbiddenUrl = "https://automationexercise.com/account_created";
                              string actualUrl = driver.Url;

                              if (actualUrl.StartsWith(forbiddenUrl, StringComparison.OrdinalIgnoreCase))
                              {
                                        Console.WriteLine(
                                            "KNOWN ISSUE: System allowed account creation with an extremely short password ('1'). " +
                                            $"User was redirected to '{actualUrl}', but short passwords should be rejected by password policy."
                                        );
                              }

                              Assert.That(
                                  actualUrl.StartsWith(forbiddenUrl, StringComparison.OrdinalIgnoreCase),
                                  Is.False,
                                  "KNOWN ISSUE: Account was created even though the password was too short (length 1). " +
                                  $"Expected NOT to reach '{forbiddenUrl}', but current URL is '{actualUrl}'. " +
                                  "Proper password length validation must be implemented."
                              );
                    }
          }
}
