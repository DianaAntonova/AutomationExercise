using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationExercise.SignupValidationTests
{
          [Category("Signup")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_SignupWithoutStreetAddressTests
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
                    public void KNOWNISSUE_SignupWithoutStreetAddress_ShouldShowRequiredFieldError()
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

                              // STEP 2: Enter Account Information with MISSING street address

                              // Title (optional)
                              var mrRadio = driver.FindElements(By.Id("id_gender1"));
                              if (mrRadio.Count > 0)
                              {
                                        mrRadio[0].Click();
                              }

                              // Valid password (за да не пречи паролата)
                              driver.FindElement(By.Id("password")).SendKeys("ValidPass123!");

                              // Address Information – попълваме всички ЗАДЪЛЖИТЕЛНИ полета,
                              // освен Address1 (Street address), който УМИШЛЕНО оставяме празен.
                              driver.FindElement(By.Id("first_name")).SendKeys("TestFirst");
                              driver.FindElement(By.Id("last_name")).SendKeys("TestLast");
                              // !!! Address1 пропускаме нарочно:
                              // driver.FindElement(By.Id("address1")).SendKeys("Test Street 123");

                              driver.FindElement(By.Id("state")).SendKeys("TestState");
                              driver.FindElement(By.Id("city")).SendKeys("TestCity");
                              driver.FindElement(By.Id("zipcode")).SendKeys("1000");
                              driver.FindElement(By.Id("mobile_number")).SendKeys("0888123456");

                              // Click "Create Account"
                              driver.FindElement(By.XPath("//button[@data-qa='create-account']")).Click();

                              // EXPECTATION:
                              // Без Street Address акаунт НЕ трябва да се създава.
                              // Тоест НЕ трябва да стигаме до /account_created.
                              string forbiddenUrl = "https://automationexercise.com/account_created";
                              string actualUrl = driver.Url;

                              if (actualUrl.StartsWith(forbiddenUrl, StringComparison.OrdinalIgnoreCase))
                              {
                                        Console.WriteLine(
                                            "KNOWN ISSUE: System allowed account creation WITHOUT a required Street Address. " +
                                            $"User was redirected to '{actualUrl}', but missing Address1 should block account creation."
                                        );
                              }

                              Assert.That(
                                  actualUrl.StartsWith(forbiddenUrl, StringComparison.OrdinalIgnoreCase),
                                  Is.False,
                                  "KNOWN ISSUE: Account was created even though the required Street Address field was left empty. " +
                                  $"Expected NOT to reach '{forbiddenUrl}', but current URL is '{actualUrl}'. " +
                                  "Proper 'Address' required-field validation must be implemented."
                              );
                    }
          }
}
