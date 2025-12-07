using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationExercise.ProductAndSearchTests
{
          [Category("Search")]
          [Category("ProductDetails")]
          public class ViewProductDetailsFromSearchResults_ShouldMatchListingInfoTests
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

                    /// <summary>
                    /// Scrolls the viewport so that the given element is brought into view.
                    /// </summary>
                    private void ScrollToElement(IWebElement element)
                    {
                              ((IJavaScriptExecutor)driver)
                                  .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
                    }

                    [Test]
                    public void ViewProductDetailsFromSearchResults_ShouldMatchListingInfo()
                    {
                              const string searchKeyword = "Top";

                              // Step 1: Search for a product keyword that returns multiple results
                              var searchInput = driver.FindElement(By.Id("search_product"));
                              searchInput.SendKeys(searchKeyword);

                              driver.FindElement(By.Id("submit_search")).Click();

                              // Step 2: Get the first product card from the search results
                              // We use the first product-image-wrapper under Searched Products section
                              var firstProductCard = driver.FindElement(
                                  By.XPath("(//div[@class='product-image-wrapper'])[1]")
                              );

                              // Extract product name and price from the card
                              var listedNameElement = firstProductCard.FindElement(
                                  By.XPath(".//div[@class='productinfo text-center']//p")
                              );

                              var listedPriceElement = firstProductCard.FindElement(
                                  By.XPath(".//div[@class='productinfo text-center']//h2")
                              );

                              string listedName = listedNameElement.Text.Trim();
                              string listedPrice = listedPriceElement.Text.Trim();

                              Assert.That(listedName, Is.Not.Empty,
                                  "Expected product name in listing to be non-empty.");
                              Assert.That(listedPrice, Is.Not.Empty,
                                  "Expected product price in listing to be non-empty.");

                              Console.WriteLine($"LISTING → Name: '{listedName}', Price: '{listedPrice}'");

                              // Step 3: Find the corresponding 'View Product' link inside the same card
                              var viewProductLink = firstProductCard.FindElement(
                                  By.XPath(".//a[contains(@href, '/product_details/')]")
                              );

                              // Scroll to the link so it is not hidden behind ads/footer
                              ScrollToElement(viewProductLink);

                              // Click via JavaScript to avoid click interception by overlays/ads
                              ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", viewProductLink);

                              // Step 4: We are now on the product details page – verify name and price
                              var detailsNameElement = driver.FindElement(
                                  By.XPath("//div[@class='product-information']/h2")
                              );

                              var detailsPriceElement = driver.FindElement(
                                  By.XPath("//div[@class='product-information']//span/span")
                              );

                              string detailsName = detailsNameElement.Text.Trim();
                              string detailsPrice = detailsPriceElement.Text.Trim();

                              Console.WriteLine($"DETAILS → Name: '{detailsName}', Price: '{detailsPrice}'");

                              // Step 5: Assertions – listing vs details must match
                              Assert.That(
                                  detailsName.Equals(listedName, StringComparison.OrdinalIgnoreCase),
                                  $"Product name mismatch between listing and details. " +
                                  $"Listing: '{listedName}', Details: '{detailsName}'."
                              );

                              Assert.That(
                                  detailsPrice,
                                  Is.EqualTo(listedPrice),
                                  $"Product price mismatch between listing and details. " +
                                  $"Listing: '{listedPrice}', Details: '{detailsPrice}'."
                              );
                    }
          }
}
