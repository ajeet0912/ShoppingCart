ClothBazar E-commerce Platform
ClothBazar is an e-commerce platform built with ASP.NET Core MVC and Entity Framework Core. This project provides a backend API and frontend interface for managing products, orders, shopping carts, and applying discounts. The platform supports various coupon types, enabling different discount logic based on cart contents.

1.Table of Contents
2.Project Overview
3.Features
4.Technologies
5.Project Structure
6.Installation and Setup
7.Usage
8.Discount Functionality
9.Contributing
10.License


Project Overview

ClothBazar allows customers to browse products, add them to a shopping cart, and apply discount codes at checkout. The platform includes authentication and authorization, allowing registered users to manage their orders and cart items.

Features

. Discount Codes: Apply percentage-based or product-specific discounts.
. Product Catalog: View and filter products by categories.
. Shopping Cart Management: Add, remove, and update items.
. Order Management: Place orders and view order history.
. User Authentication: Register and log in with ASP.NET Identity.
. Admin Panel (optional for further development): Manage products, categories, and coupons.

Technologies

. ASP.NET Core MVC - Web application framework for creating the platform.
. Entity Framework Core - ORM for database interactions.
. SQL Server - Database management.
. ASP.NET Identity - Authentication and Authorization for secure login and registration.

Project Structure
This project follows an n-tier architecture to separate concerns, which includes the following layers:

. ClothBazar.Web: Frontend web application using ASP.NET Core MVC for views and controllers.
. ClothBazar.Entities: Contains models and entities for database interactions.
. ClothBazar.Service: Handles business logic, including product management, cart management, and discount calculations.
. ClothBazar.Data: Data access layer using Entity Framework Core for database context and repository pattern.

Installation and Setup

Prerequisites

. .NET SDK 7 or higher
. SQL Server (or modify the connection string to another compatible database)

Steps

1. Clone the repository:

https://github.com/ajeet0912/ShoppingCart.git

cd ClothBazar


2. Configure Database:

In appsettings.json, update the connection string under ConnectionStrings:
json


"ConnectionStrings": {
  "SqlContext": "Server=yourservername;Database=databasename;Trusted_Connection=True;TrustServerCertificate=True"
}


Run Migrations:

Navigate to the project folder and run migrations to create the database schema:

dotnet ef database update


Run the Application:

dotnet run --project ClothBazar.Web
Access the Application:

Open a browser and navigate to https://localhost:7281 (or the port specified in your console).
Usage
Register or Log In:

Register a new account or log in with an existing account.
Browse Products:

Navigate through categories to view products, add them to the shopping cart, and apply coupon codes at checkout.
Apply Coupon Codes:

Use available coupon codes at checkout, such as:
CDP10: Flat 10% discount on all items in the cart.
CDPCAP: Buy 2 pairs of jeans and get 1 cap free.
Discount Functionality
Discounts can be applied using coupon codes. The project supports the following discount types:

Type-1: Discount on All Items - Coupon Code: CDP10

A flat 10% discount is applied to all items in the cart.
Type-2: Discount on Specific Items - Coupon Code: CDPCAP

For every 2 pairs of jeans purchased, 1 cap is given for free.
Core Methods
ApplyCouponAsync: Checks and applies the appropriate discount based on the coupon type.
ValidateCouponAsync: Validates if the coupon is active and has not expired.
LogCouponUsageAsync: Logs each use of a coupon code in the database.
Example Code Snippets
Applying a Discount:

csharp

public async Task<decimal> ApplyCouponAsync(string couponCode, ShoppingCartViewModels model)
{
    var coupon = await _db.Coupons
        .FirstOrDefaultAsync(c => c.Code == couponCode && c.IsActive && c.ExpiryDate >= DateTime.Now);

    if (coupon == null) return 0;

    decimal discountAmount = coupon.DiscountType switch
    {
        (int)DiscountType.AllItemsDiscount => model.OrderTotal * (coupon.DiscountValue / 100),
        (int)DiscountType.ProductSpecificDiscount => await ApplyProductSpecificDiscountAsync(model),
        _ => throw new InvalidOperationException("Invalid coupon type.")
    };

    await LogCouponUsageAsync(coupon.Id, model.ListShoppingCart.FirstOrDefault().Id, discountAmount);
    return discountAmount;
}
Product-Specific Discount:

csharp

private async Task<decimal> ApplyProductSpecificDiscountAsync(ShoppingCartViewModels model)
{
    decimal discountAmount = 0;
    int jeansCount = model.ListShoppingCart.Count(item => item.Product.Name.Contains("Jeans"));
    int freeCaps = jeansCount / 2;

    foreach (var item in model.ListShoppingCart)
    {
        if (item.Product.Name.Contains("Cap") && freeCaps > 0)
        {
            discountAmount += item.Product.Price;
            freeCaps--;
        }
    }

    return discountAmount;
}
Contributing
Contributions are welcome! If you have suggestions or new ideas, please open an issue or submit a pull request. When contributing, please ensure your code follows best practices and includes appropriate comments and documentation.

License
This project is licensed under the MIT License. See LICENSE for more details.

Replace https://github.com/yourusername/ClothBazar.git with your actual GitHub repository URL. This README provides clear instructions and information to help others understand, use, and contribute to your project.