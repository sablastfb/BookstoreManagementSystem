# Bookstore Management System

A comprehensive .NET Web API solution for managing bookstore operations with automated data import capabilities.

## Architecture Overview

This solution is divided into **5 separate projects**, each serving a specific purpose:

### **WebApp - Backend**
The core Web API application handling all business logic and data operations.

### **Scheduler**
Automated job scheduler for periodic data import operations.

### **DataGenerator**
Utility project for simulating third-party APIs and generating realistic test data.

### **UnitTest**
Comprehensive unit test suite ensuring code quality and reliability.

### **IntegrationTest**
End-to-end integration tests using Docker containers for realistic testing scenarios.

---

## Getting Started

### Prerequisites
- .NET SDK 8.0 or higher
- Docker and Docker Compose
- Any IDE (Visual Studio, VS Code, Rider)

### Setup Instructions

1. **Build the Solution**
   ```bash
   dotnet build
   ```

2. **Start the Database**
   ```bash
   docker compose up -d
   ```
   This will start a PostgreSQL database container.

3. **Launch the WebApp**
   ```bash
   dotnet run --project WebApp
   ```
   The application will open with Swagger documentation available.

4. **Generate Test Data**
    - While the WebApp is running, start the DataGenerator
    - Use the provided `http.rest` file to execute endpoints
    - **Important**: Add data in this order: Authors and Genres first, then Books, then Reviews

5. **Authentication**
    - Navigate to the User endpoint in Swagger
    - Login with credentials:
        - **Username**: `admin`
        - **Password**: `admin`
    - Copy the generated JWT token
    - Click "Authorize" in the top-right corner of Swagger and enter the token

6. **Test the Endpoints**
    - All endpoints are now accessible based on your role permissions

7. **Start the Scheduler** (Optional)
    - Run the Scheduler project while WebApi and DataGenerator are active
    - This will simulate hourly data imports

---

## Technical Architecture

### Backend Design
The architecture follows patterns similar to the [ASP.NET Core RealWorld example](https://github.com/gothinkster/aspnetcore-realworld-example-app), providing a proven, maintainable foundation.

**Core Architectural Components:**

- **CQRS Pattern** implemented with MediatR
    - **Commands**: Handle data modification operations
    - **Queries**: Handle data retrieval operations
- **Entity Framework Core** for object-relational mapping
- **Transaction Management**: All operations execute within transactions via MediatR pipeline
- **Fluent Validation** applied to all command operations
- **Domain Separation**: Clear distinction between Domain models and DTOs (referred to as "Envelopes")

### Supporting Technologies
- **Swagger** for comprehensive API documentation
- **API Versioning** for backward compatibility
- **Serilog** for structured logging
- **Centralized Error Handling** following RealWorld patterns with user-friendly error messages

### Database Operations
Custom SQL queries are maintained in separate `.sql` files to leverage IDE support and facilitate direct database testing and optimization.

---

## Data Generator

A specialized utility library designed to simulate realistic third-party API integration with Bogus:

**Features:**
- **JWT Authentication** for secure API communication
- **Realistic Data Generation** using appropriate data generation libraries
- **Typo Simulation Engine**:
    - Creates book titles with intentional typographical errors
    - Supports testing of fuzzy matching algorithms
    - Returns both original and modified titles for comparison
- **Reference Data Management**: Genres implemented as hardcoded lookup tables for consistency

---

## Scheduler Service

**Implementation**: Quartz.NET

**Configuration:**
- **Execution Schedule**: Hourly intervals, continuous operation, set with cron expression
- **Primary Job**: `BookImportJob` handles bulk data import operations
- **Data Source**: Mock data provided by DataGenerator with authenticated client simulation
- **Error Simulation**: Includes intentional typos to test matching resilience

---

## Book Import Algorithm Implementation

### **Optimized Duplicate Detection Strategy**

The book import process implements an efficient algorithm designed to handle large datasets while providing fuzzy matching capabilities:

**Algorithm Steps:**

1. **Database Preparation**
    - Load all existing book titles from the database
    - Apply normalization to all strings (trimming, case conversion)
    - Create both a List and HashSet of normalized titles for dual-purpose lookup

2. **Import Processing**
    - Retrieve new books from the external data source
    - Iterate through each candidate book title

3. **Duplicate Detection Logic**
    - **Primary Check**: Use HashSet for exact match detection with O(log n) complexity
    - **Fuzzy Matching**: Apply FuzzySharp library for similarity comparison against the normalized title list
    - **Acceptance Criteria**: Books passing both exact and fuzzy match thresholds are added to the import queue

4. **Batch Processing**
    - Collect all validated books into a single collection
    - Execute bulk insert operation to minimize database round trips

## Testing Framework

### Unit Testing
- **Framework**: XUnit with AAA (Arrange, Act, Assert) methodology
- **Assertion Library**: Fluent Assertions for enhanced readability and maintainability
- **Coverage**: Focuses on business logic validation and edge case handling

### Integration Testing
- **Framework**: XUnit with TestContainers integration
- **Test Environment**: Isolated Docker containers ensure consistent test conditions
- **Database**: Fresh PostgreSQL instance created for each test suite
- **Resource Management**: Automatic container lifecycle management with post-test cleanup

---

## API Endpoints

### Authentication & Authorization
- **Authentication Method**: JWT Bearer tokens
- **Role-Based Access Control**:
    - **Read Role**: Access to GET endpoints only
      - Username: reader
      - Password: reader 
    - **ReadWrite Role**: Full CRUD operation access
      - Username: admin
      - Password: admin
---

## Future Enhancements

**Potential Improvements:**
- **Enhanced Logging**: More granular operation tracking and performance metrics and open telemetry
- **Advanced Error Handling**: Detailed error categorization and recovery strategies
- **Caching Strategy**: Redis integration for improved response times
- **Monitoring Integration**: Application Performance Monitoring (APM) tools
- **API Rate Limiting**: Protection against excessive request volumes
- **Replace client with context in data generator**: I notice it too late 
---

## Project Reflection

This comprehensive task presented multiple technical challenges requiring integration of various .NET ecosystem components. The solution demonstrates practical implementation of CQRS patterns, automated scheduling, realistic data generation, and thorough testing methodologies. The combination of clean architecture principles with robust testing infrastructure creates a maintainable and scalable foundation for bookstore management operations.

