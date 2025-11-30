using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise
{
          public class SignupWithInvalidEmailFormats_ShouldStayOnSignupLoginPage
          {
                    public IWebDriver driver;

                    [SetUp]
                    public void Setup()
                    {
                              driver = new ChromeDriver();
                              driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
                              driver.Manage().Window.Maximize();
                              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                              // Open homepage
                              driver.Navigate().GoToUrl("https://automationexercise.com/");

                              // Accept cookies if popup is displayed
                              driver.FindElement(By.XPath("//p[@class='fc-button-label']")).Click();
                    }

                    [TearDown]
                    public void TearDown()
                    {
                              driver.Quit();
                              driver.Dispose();
                    }

                    [Test]
                    public void SignupWithInvalidEmailFormatsShouldStayOnSignupLoginPage()
                    {
                              // 1. Navigate to Signup / Login page
                              driver.FindElement(By.CssSelector("a[href='/login']")).Click();

                              // 2. Enter any valid full name in the "New User Signup!" form
                              IWebElement nameInput = driver.FindElement(By.Name("name"));
                              nameInput.Clear();
                              nameInput.SendKeys("DiyanaAntonova");

                              // 3. List of invalid email formats to test
                              string[] invalidEmails =
                              {
                "dian@",             // missing domain
                "userexample.com",   // missing '@'
                "user123"            // no '@' and no domain
            };

                              foreach (var invalidEmail in invalidEmails)
                              {
                                        // Locate email field again for each iteration
                                        IWebElement emailInput = driver.FindElement(By.CssSelector("input[data-qa='signup-email']"));
                                        emailInput.Clear();
                                        emailInput.SendKeys(invalidEmail);

                                        // 4. Submit the signup form
                                        IWebElement signupButton = driver.FindElement(By.CssSelector("button[data-qa='signup-button']"));
                                        signupButton.Click();

                                        // 5. Expected Result:
                                        //    • User must remain on Signup/Login page
                                        //    • No redirect to account creation page should occur
                                        //    • "Enter Account Information" should NOT appear

                                        Assert.That(
                                            driver.Url.Contains("/login") || driver.Url.Contains("/signup"),
                                            $"Invalid email '{invalidEmail}' incorrectly allowed navigation away from the signup page."
                                        );

                                        // Ensure signup title is still displayed -> user is still on signup form
                                        IWebElement signupTitle = driver.FindElement(By.XPath("//h2[text()='New User Signup!']"));
                                        Assert.That(
                                            signupTitle.Displayed,
                                            $"Signup form was not displayed after entering invalid email '{invalidEmail}'."
                                        );
                                        // Допълнителна проверка – URL да си остане /login (Signup / Login page)
                                        Assert.That(driver.Url, Does.Contain("/login"), "User is not on the Signup / Login page after submitting existing email.");
                                        // Optional check:
                                        // Look for potential validation or error hints
                                        // (The website does NOT show explicit email format errors, so this is optional)
                                        // var errorElements = driver.FindElements(By.XPath("//*[contains(text(),'Invalid') or contains(text(),'incorrect')]"));
                                        // Assert.That(errorElements.Count == 0, "Unexpected error message appeared for invalid email.");
                              }
                    }
          }
}
