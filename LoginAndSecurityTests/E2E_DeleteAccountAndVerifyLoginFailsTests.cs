using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.LoginAndSecurityTests
{
          public class E2EDeleteAccountAndVerifyLoginFailsTests
          {
                    public IWebDriver driver;
                    private string tempEmail;
                    private string tempPassword = "Test1234";

                    [SetUp]
                    public void Setup()
                    {
                              var options = new ChromeOptions();
                              options.AddUserProfilePreference("credentials_enable_service", false);
                              options.AddUserProfilePreference("profile.password_manager_enabled", false);

                              options.AddArgument("--disable-notifications");
                              options.AddArgument("--disable-infobars");
                              options.AddArgument("--disable-save-password-bubble");
                              options.AddArgument("--disable-popup-blocking");

                              driver = new ChromeDriver(options);
                              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                              driver.Manage().Window.Maximize();
                              driver.Navigate().GoToUrl("https://automationexercise.com/");

                              // Accept cookies popup if visible
                              var cookieButtons = driver.FindElements(By.XPath("//p[@class='fc-button-label']"));
                              if (cookieButtons.Count > 0)
                                        cookieButtons[0].Click();

                              // Generate unique email for this test only
                              tempEmail = $"tempuser_{Guid.NewGuid()}@example.com";
                    }

                    [TearDown]
                    public void TearDown()
                    {
                              driver.Quit();
                              driver.Dispose();
                    }

                    private void ScrollTo(IWebElement el)
                    {
                              ((IJavaScriptExecutor)driver)
                                  .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", el);
                    }

                    [Test]
                    public void DeleteAccountAndVerifyLoginFails_ShouldShowIncorrectCredentialsMessage()
                    {
                              // ================================
                              // STEP 1 – CREATE USER ACCOUNT
                              // ================================
                              IWebElement loginLink = driver.FindElement(By.CssSelector("a[href='/login']"));
                              ScrollTo(loginLink);
                              loginLink.Click();

                              // Go to SignUp section
                              IWebElement signupName = driver.FindElement(By.Name("name"));
                              ScrollTo(signupName);
                              signupName.SendKeys("Temporary User");

                              IWebElement signupEmail = driver.FindElement(By.CssSelector("input[data-qa='signup-email']"));
                              ScrollTo(signupEmail);
                              signupEmail.SendKeys(tempEmail);

                              IWebElement signupButton = driver.FindElement(By.CssSelector("button[data-qa='signup-button']"));
                              ScrollTo(signupButton);
                              signupButton.Click();

                              // Fill minimal account info
                              IWebElement title = driver.FindElement(By.Id("id_gender1"));
                              ScrollTo(title);
                              title.Click();

                              IWebElement passwordInput = driver.FindElement(By.Id("password"));
                              ScrollTo(passwordInput);
                              passwordInput.SendKeys(tempPassword);

                              // Required Address Info
                              driver.FindElement(By.Id("first_name")).SendKeys("Temp");
                              driver.FindElement(By.Id("last_name")).SendKeys("User");
                              driver.FindElement(By.Id("address1")).SendKeys("Test Street 1");
                              driver.FindElement(By.Id("state")).SendKeys("TS");
                              driver.FindElement(By.Id("city")).SendKeys("Test City");
                              driver.FindElement(By.Id("zipcode")).SendKeys("1000");
                              driver.FindElement(By.Id("mobile_number")).SendKeys("+359888123456");

                              IWebElement createAccountBtn =
                                  driver.FindElement(By.CssSelector("button[data-qa='create-account']"));
                              ScrollTo(createAccountBtn);
                              createAccountBtn.Click();

                              IWebElement accountCreated =
                                  driver.FindElement(By.XPath("//b[text()='Account Created!']"));
                              ScrollTo(accountCreated);
                              Assert.That(accountCreated.Displayed, "Account was NOT created.");

                              IWebElement continueBtn =
                                  driver.FindElement(By.CssSelector("a[data-qa='continue-button']"));
                              ScrollTo(continueBtn);
                              continueBtn.Click();

                              IWebElement loggedIn =
                                  driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));
                              ScrollTo(loggedIn);
                              Assert.That(loggedIn.Displayed, "User did NOT appear logged in after signup.");

                              // ================================
                              // STEP 2 – DELETE ACCOUNT
                              // ================================
                              IWebElement deleteAccountLink =
                                  driver.FindElement(By.CssSelector("a[href='/delete_account']"));
                              ScrollTo(deleteAccountLink);
                              deleteAccountLink.Click();

                              IWebElement accountDeletedMsg =
                                  driver.FindElement(By.XPath("//b[text()='Account Deleted!']"));
                              ScrollTo(accountDeletedMsg);
                              Assert.That(accountDeletedMsg.Displayed, "Account Deleted message missing.");

                              IWebElement continueAfterDelete =
                                  driver.FindElement(By.CssSelector("a[data-qa='continue-button']"));
                              ScrollTo(continueAfterDelete);
                              continueAfterDelete.Click();

                              // ================================
                              // STEP 3 – TRY LOGIN AGAIN (SHOULD FAIL)
                              // ================================
                              IWebElement loginLinkAgain =
                                  driver.FindElement(By.CssSelector("a[href='/login']"));
                              ScrollTo(loginLinkAgain);
                              loginLinkAgain.Click();

                              IWebElement loginEmail =
                                  driver.FindElement(By.CssSelector("input[data-qa='login-email']"));
                              ScrollTo(loginEmail);
                              loginEmail.SendKeys(tempEmail);

                              IWebElement loginPass =
                                  driver.FindElement(By.CssSelector("input[data-qa='login-password']"));
                              ScrollTo(loginPass);
                              loginPass.SendKeys(tempPassword);

                              IWebElement loginButtonAgain =
                                  driver.FindElement(By.CssSelector("button[data-qa='login-button']"));
                              ScrollTo(loginButtonAgain);
                              loginButtonAgain.Click();

                              // ================================
                              // STEP 4 – VERIFY FAIL MESSAGE
                              // ================================
                              IWebElement incorrectMsg =
                                  driver.FindElement(By.XPath("//p[text()='Your email or password is incorrect!']"));
                              ScrollTo(incorrectMsg);

                              Assert.That(
                                  incorrectMsg.Displayed,
                                  "Expected incorrect credentials message was NOT displayed."
                              );
                    }
          }
}
