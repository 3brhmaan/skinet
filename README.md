
## Features

### General Features
- User registration and login system.
- Role-based authorization for buyers and admins.
- Dynamic shopping cart functionality.
- Seamless checkout process with **Stripe** payment integration.
- Refund management through the admin panel.
- Order history accessible by both buyers and admins with role-specific views.

### Admin Panel Features
- Manage orders with refund capabilities using **Stripe**.
- View and process all orders with advanced accessibility controls.

### Technologies Used

#### Backend
- **Platform:** .NET 8
- **Database Access:** Entity Framework Core (EF Core)
- **Caching:** Redis
- **Design Patterns:**
  - Generic Repository
  - Unit of Work
  - Specification Pattern
- **Real-Time Communication:** SignalR
- **Payment Integration:** Stripe
- **Exception Handling:** Robust centralized error management
- **Caching:** Optimized for performance using Redis
- **Role-Based Authorization:** Secure user access levels
- **Database:** SQL Server

#### Frontend
- **Platform:** Angular 18
- **UI Framework:** Tailwind CSS
- **Material Design:** Angular Material
- **Error Handling:** Centralized and user-friendly error notifications
- **Real-Time Updates:** SignalR integration
- **Payment Integration:** Stripe
- **Performance Enhancements:**
  - HTTP Interceptors
  - Lazy Loading Modules

---

## Application Flow

### Buyer Flow
1. **Registration & Login:** Users can register and log in to access the app.
2. **Product Selection:** Browse and select products.
3. **Shopping Cart:** Add selected products to the cart.
4. **Checkout:** Complete the purchase via Stripe.
5. **Order History:** View past orders with detailed information.

### Admin Flow
1. **Login:** Admins access a secured admin panel.
2. **Order Management:** View all orders with full details.
3. **Refund Processing:** Manage refunds seamlessly through Stripe.

---

## Installation & Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) and npm
- [Redis](https://redis.io/download)
- Angular CLI
- [SQL Server](https://www.microsoft.com/sql-server)

### Backend Setup
1. Clone the repository.
  ```bash
    git clone git@github.com:3brhmaan/skinet.git
  ```
2. Run the application:
  ```bash
    dotnet run
  ```

### Frontend Setup
1. Navigate to the frontend folder: `cd client`
2. Install dependencies: `npm install`
3. Update environment files with Stripe configurations.
4. Start the development server:
  ```
    ng serve -o
  ```

---

## Acknowledgments
This project is inspired by the Udemy course: [Learn to Build an E-Commerce App with .NET Core and Angular](https://www.udemy.com/course/learn-to-build-an-e-commerce-app-with-net-core-and-angular/?couponCode=BFCPSALE24) by Neil Cummings.
