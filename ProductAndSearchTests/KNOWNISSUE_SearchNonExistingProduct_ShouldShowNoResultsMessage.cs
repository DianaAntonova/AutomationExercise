using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace AutomationExercise.ProductAndSearchTests
{
          [Category("Search")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_SearchNonExistingProduct_ShouldShowNoResultsMessageTests
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
                              driver.Navigate().GoToUrl("https://automationexercise.com/products");

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
                    public void KNOWNISSUE_SearchNonExistingProduct_ShouldShowNoResultsMessage()
                    {
                              string searchKeyword = "umbrela123456"; // deliberately nonexistent

                              // Search input
                              var searchInput = driver.FindElement(By.Id("search_product"));
                              searchInput.SendKeys(searchKeyword);

                              // Click Search
                              driver.FindElement(By.Id("submit_search")).Click();

                              // Check for product cards after search
                              var productElements = driver.FindElements(By.XPath("//div[@class='productinfo text-center']//p"));

                              // Expectation:
                              // - NO products should appear
                              // - A 'no results' message SHOULD appear (but the site does not show any)
                              if (productElements.Count > 0)
                              {
                                        Console.WriteLine(
                                            $"KNOWN ISSUE: Search for nonexistent keyword '{searchKeyword}' returned {productElements.Count} products. " +
                                            $"Search results should be empty, but results were shown."
                                        );

                                        Assert.Fail(
                                            $"KNOWN ISSUE: Expected 0 results for keyword '{searchKeyword}', " +
                                            $"but found {productElements.Count}. Search engine incorrectly returns products."
                                        );
                              }

                              // Check if a 'No results' message exists (should, but does NOT)
                              var noResultsMessage = driver.FindElements(By.XPath("//*[contains(text(),'No results') or contains(text(),'not found')]"));

                              if (noResultsMessage.Count == 0)
                              {
                                        Console.WriteLine(
                                            "KNOWN ISSUE: No 'No products found' message is displayed when searching for a nonexistent product. " +
                                            "User receives no feedback that search returned no results."
                                        );

                                        Assert.Fail(
                                            "KNOWN ISSUE: Missing 'no results' warning message when searching for nonexistent products. " +
                                            "UX is unclear and breaks expected search behavior."
                                        );
                              }

                              Assert.Pass("Search correctly shows no results message (if ever implemented).");
                    }
          }
}
