
# Book Catalog API

## Overview

This is an ASP.NET Core Web API for managing a book catalog. The API provides endpoints to retrieve all books, filter books, and create new books. It utilizes MediatR for handling commands and queries and includes caching for improved performance.

## Getting Started

### Prerequisites

- .NET 7.0 SDK

### Installation

1. Clone the repository:

   ```
   git clone https://github.com/socdar96/BookCatalogAPI.git
   ```

2. Navigate to the project directory:

   ```
   cd book-catalog-api
   ```

3. Run the application:

   ```
   dotnet run
   ```

The API will be accessible at `https://localhost:5001` by default.

## API Endpoints

### Get All Books

Endpoint: `GET /api/books`

Retrieves all books from the catalog.

### Filter Books

Endpoint: `GET /api/books/filter`

Retrieves books based on filtering criteria such as category, page number, and page size.

### Create Book

Endpoint: `POST /api/books`

Creates a new book and returns its ID.

#### Request Body

```json
{
  "categoryId": 1,
  "title": "The Great Gatsby",
  "description": "A classic novel",
  "publishDateUtc": "2024-02-01T12:00:00Z"
}
```

## Project Structure

- **BookCatalogApi**
  - **Controllers**
    - `BooksController.cs`: Contains endpoints for managing books.
  - **Commands**
    - `CreateBookCommand.cs`: Represents the command to create a new book.
  - **Queries**
    - `GetBooksQuery.cs`: Represents the query to retrieve books with filtering options.
  - **Models**
    - `Book.cs`: Defines the structure of a book.

## Technologies Used

- ASP.NET Core
- MediatR
- MemoryCache
