using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationExercise.NewsletterAndContactTests
{
          [Category("Newsletter")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_NewsletterSubscription_InvalidEmailTests
          {
                    private IWebDriver driver;

                    [SetUp]
                    public void Setup()
                    {
                              var options = new ChromeOptions();
                              options.AddArgument("--start-maximized");

                              driver = new ChromeDriver(options);
                              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                              driver.Navigate().GoToUrl("https://automationexercise.com/");

                              // Accept cookies popup if present
                              var cookieButtons = driver.FindElements(By.XPath("//p[@class='fc-button-label']"));
                              if (cookieButtons.Count > 0)
                              {
                                        cookieButtons[0].Click();
                              }

                              ScrollToFooter();
                    }

                    [TearDown]
                    public void TearDown()
                    {
                              driver.Quit();
                              driver.Dispose();
                    }

                    private void ScrollToFooter()
                    {
                              ((IJavaScriptExecutor)driver)
                                  .ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    }

                    // --------------------------
                    // TEST 1: Empty email ""
                    // --------------------------
                    [Test]
                    public void Newsletter_EmptyEmail_ShouldShowRequiredFieldMessage()
                    {
                              var emailField = driver.FindElement(By.Id("susbscribe_email"));
                              var button = driver.FindElement(By.Id("subscribe"));

                              emailField.Clear();
                              button.Click();

                              string validationMessage = emailField.GetAttribute("validationMessage");
                              Console.WriteLine("Browser message: " + validationMessage);

                              Assert.That(
                                  validationMessage.Length > 0,
                                  "Browser did not block empty email input."
                              );
                    }

                    // --------------------------
                    // TEST 2: "1" → missing '@'
                    // --------------------------
                    [Test]
                    public void Newsletter_InvalidEmail_MissingAt_Symbol()
                    {
                              var emailField = driver.FindElement(By.Id("susbscribe_email"));
                              var button = driver.FindElement(By.Id("subscribe"));

                              emailField.Clear();
                              emailField.SendKeys("1");
                              button.Click();

                              string validationMessage = emailField.GetAttribute("validationMessage");
                              Console.WriteLine("Browser message: " + validationMessage);

                              Assert.That(
                                  validationMessage.Contains("@"),
                                  "Browser should warn that '@' is missing."
                              );
                    }

                    // --------------------------
                    // TEST 3: "1@" → incomplete domain
                    // --------------------------
                    [Test]
                    public void Newsletter_InvalidEmail_IncompleteDomain()
                    {
                              var emailField = driver.FindElement(By.Id("susbscribe_email"));
                              var button = driver.FindElement(By.Id("subscribe"));

                              emailField.Clear();
                              emailField.SendKeys("1@");
                              button.Click();

                              string validationMessage = emailField.GetAttribute("validationMessage");
                              Console.WriteLine("Browser message: " + validationMessage);

                              Assert.That(
                                  validationMessage.Contains("incomplete") || validationMessage.Contains("part"),
                                  "Browser should reject an incomplete email domain."
                              );
                    }

                    // --------------------------
                    // TEST 4: "test@abv" → Known Issue (no TLD)
                    // --------------------------
                    [Test]
                    public void KNOWNISSUE_Newsletter_InvalidEmail_NoTopLevelDomain()
                    {
                              var emailField = driver.FindElement(By.Id("susbscribe_email"));
                              var button = driver.FindElement(By.Id("subscribe"));

                              string invalidEmail = "test@abv";

                              emailField.Clear();
                              emailField.SendKeys(invalidEmail);
                              button.Click();

                              var successMsg = driver.FindElements(By.Id("success-subscribe"));

                              if (successMsg.Count > 0 && successMsg[0].Displayed)
                              {
                                        Console.WriteLine(
                                            $"KNOWN ISSUE: Email '{invalidEmail}' has no TLD (.com/.bg), " +
                                            "but system incorrectly accepted it."
                                        );

                                        Assert.Fail(
                                            "System should NOT accept newsletter subscription without a valid domain extension."
                                        );
                              }

                              Assert.Pass("System correctly rejected email without TLD (if fixed).");
                    }
          }
}
