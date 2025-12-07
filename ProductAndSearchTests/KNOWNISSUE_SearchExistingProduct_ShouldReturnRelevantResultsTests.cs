using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AutomationExercise.ProductAndSearchTests
{
          [Category("Search")]
          [Category("KnownIssue")]
          public class KNOWNISSUE_SearchExistingProduct_ShouldReturnRelevantResultsTests
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
                    public void KNOWNISSUE_SearchExistingProduct_ShouldReturnOnlyRelevantResults()
                    {
                              string searchKeyword = "Dress";

                              // Search field
                              var searchInput = driver.FindElement(By.Id("search_product"));
                              searchInput.SendKeys(searchKeyword);

                              // Click search button
                              driver.FindElement(By.Id("submit_search")).Click();

                              // Collect all product titles shown in results
                              var productElements = driver.FindElements(By.XPath("//div[@class='productinfo text-center']//p"));

                              // Assert that there is at least one result
                              Assert.That(productElements.Count, Is.GreaterThan(0),
                                  $"Expected at least 1 product to match '{searchKeyword}', but no products were returned.");

                              // Extract product names
                              List<string> productNames = productElements.Select(p => p.Text).ToList();

                              // Validate ALL results contain the keyword
                              foreach (var name in productNames)
                              {
                                        if (!name.ToLower().Contains(searchKeyword.ToLower()))
                                        {
                                                  Console.WriteLine(
                                                      $"KNOWN ISSUE: Search for '{searchKeyword}' returned unrelated product '{name}'. " +
                                                      "Search relevance is not correctly implemented."
                                                  );

                                                  Assert.Fail(
                                                      $"KNOWN ISSUE: Expected ONLY results containing '{searchKeyword}', " +
                                                      $"but found unrelated product: '{name}'. " +
                                                      "Search relevance must be improved so that all results match the search keyword."
                                                  );
                                        }
                              }

                              // Ако стигнем дотук, значи всички продукти са релевантни
                              Assert.Pass($"All {productNames.Count} results matched the search keyword '{searchKeyword}'.");
                    }
          }
}
