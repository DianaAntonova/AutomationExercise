📌 Automation Exercise – UI Test Automation (C# / Selenium / NUnit)

This repository contains automated UI tests for AutomationExercise.com, implemented using C#, Selenium WebDriver and NUnit.

📁 Test Categories

SignupTests – Positive & negative signup scenarios + Known Issues

LoginAndSecurityTests – Login validation, invalid credentials, lockout behavior

ProductAndSearchTests – Search results, search errors, product details

CartAndCheckoutTests – Add to cart, multi-product flows, checkout E2E

NewsletterAndContactTests – Newsletter email validation + invalid formats

✅ Key Features

Covers positive, negative, boundary and E2E scenarios

Includes KnownIssue tests where the website behaves incorrectly

Uses dynamic test data (e.g., unique emails)

Tests organized into clear, scalable folders

Professional assertions and structured reporting

▶️ How to Run

Requires:

.NET SDK

Chrome browser

Run all tests:

dotnet test


Run a specific category:

dotnet test --filter TestCategory=Signup
dotnet test --filter TestCategory=KnownIssue

🐞 Known Issues Detected

Discovered through automation:

Newsletter accepts emails without a top-level domain (e.g., test@abv)

Signup allows whitespace-only names and invalid characters

No account lockout after repeated failed login attempts

👩‍💻 Author

Diana Antonova
QA Automation Engineer (C#, Selenium, NUnit)
