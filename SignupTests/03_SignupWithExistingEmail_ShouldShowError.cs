using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.SignupTests
{
          public class SignupExistingEmailTests
          {
                    public IWebDriver driver;

                    [SetUp]
                    public void Setup()
                    {
                              driver = new ChromeDriver();
                              driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
                              driver.Manage().Window.Maximize();
                              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                            //  AcceptCookiesIfPresent();


                              // Отвори началната страница
                              driver.Navigate().GoToUrl("https://automationexercise.com/");
                              driver.FindElement(By.XPath("//p[@class=\"fc-button-label\"]")).Click();
                    }

                    [TearDown]
                    public void TearDown()
                    {
                              driver.Quit();
                              driver.Dispose();
                    }

                    [Test]
                    public void SignupWithExistingEmail_ShouldShowErrorAndStayOnSignupLoginPage()
                    {
                              // 1. Navigate to Signup / Login
                              driver.FindElement(By.CssSelector("a[href='/login']")).Click();

                              // 2. Enter any full name in "New User Signup!" form
                              IWebElement nameInput = driver.FindElement(By.Name("name"));
                              nameInput.Clear();
                              nameInput.SendKeys("DiyanaAntonova");

                              // 3. Enter already registered email
                              IWebElement emailInput = driver.FindElement(By.CssSelector("input[data-qa='signup-email']"));
                              emailInput.Clear();
                              emailInput.SendKeys("diant@abv.bg");

                              // 4. Submit the form
                              IWebElement signupButton = driver.FindElement(By.CssSelector("button[data-qa='signup-button']"));
                              signupButton.Click();

                              // 5. Verify message "Email Address already exist!" and that we are still on Signup / Login page
                              IWebElement errorMessage =
                                  driver.FindElement(By.XPath("//p[text()='Email Address already exist!']"));

                              Assert.That(errorMessage.Displayed, Is.True, "Error message is not displayed.");

                              // Допълнителна проверка – URL да си остане /login (Signup / Login page)
                                  Assert.That(driver.Url, Does.Contain("/signup"),"User is not on the Signup / Login page after submitting existing email.");
                    }
          }
}
