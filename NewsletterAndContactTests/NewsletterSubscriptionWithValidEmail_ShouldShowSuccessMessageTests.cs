using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationExercise.NewsletterAndContactTests
{
          [Category("Newsletter")]
          public class NewsletterSubscriptionWithValidEmail_ShouldShowSuccessMessageTests
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
                    }

                    [TearDown]
                    public void TearDown()
                    {
                              driver.Quit();
                              driver.Dispose();
                    }

                    [Test]
                    public void NewsletterSubscriptionWithValidEmail_ShouldShowSuccessMessage()
                    {
                              // Generate a unique email for subscription
                              string random = Guid.NewGuid().ToString("N").Substring(0, 6);
                              string email = $"news_{random}@test.bg";

                              // Scroll to footer where subscription field is located
                              IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                              js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                              // Enter valid email
                              var emailField = driver.FindElement(By.Id("susbscribe_email"));
                              emailField.SendKeys(email);

                              // Click Subscribe button
                              driver.FindElement(By.Id("subscribe")).Click();

                              // Validate success message
                              var successMessage = driver.FindElement(By.Id("success-subscribe"));

                              Assert.That(
                                  successMessage.Displayed,
                                  Is.True,
                                  "Expected success message after newsletter subscription, but it was not displayed."
                              );

                              string messageText = successMessage.Text.Trim();
                              Console.WriteLine("Newsletter subscription message: " + messageText);

                              Assert.That(
                                  messageText.Contains("You have been successfully subscribed"),
                                  Is.True,
                                  $"Expected subscription confirmation message, but got: '{messageText}'."
                              );
                    }
          }
}
