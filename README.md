# Bookshop - ASP.NET Core 8 MVC Project

Welcome to **Bookshop**, an ASP.NET Core 8 MVC project designed to streamline the management and operations of a bookshop. This comprehensive application caters to various user roles, ensuring smooth functionality for administrators, companies, employees, and regular users. With intuitive features and robust security measures, Bookshop simplifies bookshop management and enhances the user experience.

## Features

- **User Roles**: Bookshop accommodates four types of users, each with specific roles and privileges: Admin, Company, Employee, and User.
- **Role-based Access Control**: Different user roles have distinct permissions, ensuring secure access and efficient operations.
- **Admin Dashboard**: Administrators can perform Create, Update, Read, and Delete (CURD) operations seamlessly, managing the entire system effortlessly.
- **Company Management**: Companies can oversee employee details, product listings, book categories, and monitor user order status for streamlined operations.
- **Employee Module (Coming Soon)**: A dedicated module for employees will be introduced soon, offering enhanced functionality for internal processes.
- **User Experience Optimization**: Users can browse the website, add products to their cart, make payments securely using Stripe payment integration, and dynamically modify their orders for a seamless shopping experience.
- **Microsoft Identity Integration**: Bookshop leverages Microsoft Identity to manage user authentication and authorization effectively.
- **SQL Database Integration**: User data is securely stored and managed using SQL database technology, ensuring reliability and scalability.
- **N-tier Architecture**: Bookshop follows an N-tier architecture, ensuring separation of concerns and scalability.
- **Repository Pattern**: The repository pattern is utilized to abstract the data access layer, promoting code maintainability and testability.
- **Unit of Work**: Bookshop implements the Unit of Work pattern to manage transactions and ensure data consistency.

## Getting Started

To get started with Bookshop, follow these simple steps:

1. **Clone the Repository**: Clone this repository to your local machine using the following command:
   ```
   git clone [https://github.com/SwarnajitSaha/BookStore.git]
   ```

2. **Setup Database**: Configure your SQL database settings in the `appsettings.json` file. Execute migrations to create the necessary database schema:
   ```
   add migration "DatabaseSetup"
   update database
   ```

3. **Run the Application**: Build and run the application using the following command:
   ```
   dotnet run
   ```

4. **Access the Application**: Once the application is running, access it through your web browser at `[https://localhost:7248/]`.

## Contributing

Contributions are welcome and encouraged! Whether you want to report a bug, request a feature, or submit a pull request, your input is valued. Please refer to the [Contribution Guidelines](CONTRIBUTING.md) for more details.



## Acknowledgments

- Thanks to the Udemy for ASP.NET Core 8 MVC course.

