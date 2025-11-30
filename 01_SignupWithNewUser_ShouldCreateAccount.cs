using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise
{
          public class SignupWithNewUser_ShouldCreateAccount
          {
                    public IWebDriver driver;

                    [SetUp]
                    public void Setup()
                    {
                              driver = new ChromeDriver();
                              driver.Manage().Window.Maximize();
                              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                              // Отваряме началната страница
                              driver.Navigate().GoToUrl("https://automationexercise.com/");

                              // Затваряме cookie popup
                              driver.FindElement(By.XPath("//p[@class='fc-button-label']")).Click();
                    }

                    [TearDown]
                    public void TearDown()
                    {
                              driver.Quit();
                              driver.Dispose();
                    }

                    [Test]
                    public void SignupWithNewUser_ShouldCreateAccount_Test()
                    {
                              // 1. Navigate to Signup / Login
                              driver.FindElement(By.CssSelector("a[href='/login']")).Click();

                              // 2. Enter any full name in "New User Signup!" form
                              driver.FindElement(By.XPath("//input[@name='name']")).SendKeys("DiyanaNewUser");

                              // 3. Enter a NEW unique email (за да не се повтаря потребителят)
                              string uniqueEmail = $"diyana_{Guid.NewGuid()}@example.com";
                              IWebElement emailInput = driver.FindElement(By.CssSelector("input[data-qa='signup-email']"));
                              emailInput.Clear();
                              emailInput.SendKeys(uniqueEmail);

                              // 4. Submit the Signup form
                              IWebElement signupButton = driver.FindElement(By.CssSelector("button[data-qa='signup-button']"));
                              signupButton.Click();

                              // 5. Verify that "ENTER ACCOUNT INFORMATION" section is visible
                              IWebElement enterAccountInfoHeader =
                                  driver.FindElement(By.XPath("//b[text()='Enter Account Information']"));
                              Assert.That(enterAccountInfoHeader.Displayed, Is.True,
                                  "Enter Account Information section is not visible.");

                              // 6. Fill required fields on Account Information page

                              // Title (Mr / Mrs) – взимаме произволен, напр. Mrs
                              driver.FindElement(By.Id("id_gender2")).Click();

                              // Password
                              IWebElement passwordInput = driver.FindElement(By.Id("password"));
                              passwordInput.SendKeys("Test1234!");

                              // Date of birth
                            //  new SelectElement(driver.FindElement(By.Id("days"))).SelectByValue("19");
                           //   new SelectElement(driver.FindElement(By.Id("months"))).SelectByValue("4");
                           //   new SelectElement(driver.FindElement(By.Id("years"))).SelectByValue("1988");

                              // Address information
                              driver.FindElement(By.Id("first_name")).SendKeys("Diyana");
                              driver.FindElement(By.Id("last_name")).SendKeys("Antonova");
                              driver.FindElement(By.Id("company")).SendKeys("Test Company");
                              driver.FindElement(By.Id("address1")).SendKeys("Test Address 1");
                              driver.FindElement(By.Id("address2")).SendKeys("Test Address 2");

                            //  new SelectElement(driver.FindElement(By.Id("country"))).SelectByText("Canada");

                              driver.FindElement(By.Id("state")).SendKeys("Test State");
                              driver.FindElement(By.Id("city")).SendKeys("Test City");
                              driver.FindElement(By.Id("zipcode")).SendKeys("1000");
                              driver.FindElement(By.Id("mobile_number")).SendKeys("+359888000000");

                              // 7. Click "Create Account"
                              driver.FindElement(By.CssSelector("button[data-qa='create-account']")).Click();

                              // 8. Verify "ACCOUNT CREATED!" message
                              IWebElement accountCreatedMessage =
                                  driver.FindElement(By.XPath("//b[text()='Account Created!']"));
                              Assert.That(accountCreatedMessage.Displayed, Is.True,
                                  "Account Created message is not displayed.");

                              // 9. Click "Continue"
                              driver.FindElement(By.CssSelector("a[data-qa='continue-button']")).Click();

                              // 10. Verify that user is logged in ("Logged in as {name}")
                              IWebElement loggedInAs =
                                  driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));
                              Assert.That(loggedInAs.Displayed, Is.True,
                                  "User is not logged in after account creation.");
                    }
          }
}
