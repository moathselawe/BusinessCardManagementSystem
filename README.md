# Business Card Manager

## Table of Contents
- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Backend Architecture & Technologies](#backend-architecture-technologies)
- [Features](#features)
- [GitHub](#github)
- [Quick Start](#quick-start)
  - [Setup Backend](#setup-backend)
  - [Docker Setup Backend Only](#docker_setup)
  - [Setup Frontend](#setup-frontend)
  - [Setup Database](#setup-database)
- [Business Card Model](#business-card-model)
- [API Endpoints](#api-endpoints)
- [Frontend Usage](#frontend-usage)
- [Unit Tests](#unit-tests)
- [Database](#database)



## Overview
This project is a web application to manage business card information, including Get, GetAll, Create, CreateMany, View, Delete, Import/Export (CSV/XML), Print, and global filtering.  
Backend is built with .NET 9.0 Web API (C#), Frontend with Angular 20, and the database SQL Server.

## Tech Stack
- Backend: .NET 9 Web API (C#)
- Frontend: Angular 20
- Database: SQL Server
- Photo Encoding: Base64

## Backend Architecture & Technologies
- MediatR for CQRS-style request handling
- FluentValidation for validating commands and DTOs
- ValidationBehavior pipeline for automatic request validation
- EF Core for database access
- Repository Pattern implemented through BusinessCardRepository
- UnitOfWork for managing transactions
- FileParserService for CSV/XML parsing
- Controllers exposing REST APIs
- Swagger / OpenAPI for documentation
- FluentValidation auto-discovery for all validators
- CORS configuration to allow Angular frontend


## Features
- Add new business cards (with optional photo)
- View all business cards
- View a single business card
- Edit business card
- Delete a business card
- Import business cards from CSV or XML files
- Export business cards to CSV or XML
- Print business cards
- Optional filtering by Name, DOB, Email, Phone

## GitHub
- Repository link: (https://github.com/moathselawe/BusinessCardManagementSystem)
- 
## Quick Start
### Setup Backend
1. Clone the repo: `gh repo clone moathselawe/BusinessCardManagementSystem`
2. Navigate to backend: `cd HireMind.Api`
3. Restore packages: `dotnet restore`
4. Update connection string in appsettings.json
5. Run migrations : `dotnet ef database update`
6. Run the API: `dotnet run`

### Docker Setup Backend Only
From inside the backend folder (HireMind.Api), You can run the backend using Docker without installing the .NET SDK locally.
1. Build Docker Image 'docker build -t HireMind-api'
2. Run Container 'docker run -d -p 8080:80 --name HireMind-api-container HireMind-api'

### Setup Frontend
1. Navigate to frontend: `cd HireMind_UI`
2. Install dependencies: `npm install`
3. Start Angular app: `ng serve`
4. Open browser at: `http://localhost:62882/HireMind/ManageBusinesscards`

### Setup Database
- Connection string (in `appsettings.json` for backend):
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HireMind_DB;Trusted_Connection=True;TrustServerCertificate=True;"
}


## Business Card Model
- Arabic Name (string, required)
- English Name (string, required)
- Date of Birth (date, required)
- Email (string, required)
- Phone (string, required)
- Address (string, required)
- Logo / Photo (Base64 string, optional, max 1MB)


## API Endpoints

- **POST /api/businesscards/search**  
  Search business cards by filters.  
  **Body:** `SearchFiltersRqDto` (Name, Email, Phone, DOB, etc.)

- **GET /api/businesscards/getAll**  
  Get all business cards.

- **POST /api/businesscards/add**  
  Create a new business card.  
  **Body:** `CreateBusinessCardDto`  

- **GET /api/businesscards/get/{id}**  
  Get a single business card by ID.

- **PUT /api/businesscards/update**  
  Update an existing business card.  
  **Body:** `UpdateBusinessCardDto`  

- **DELETE /api/businesscards/delete/{id}**  
  Delete a business card by ID.

- **POST /api/businesscards/preview**  
  Preview business cards from uploaded file (CSV/XML).  
  **Body:** `IFormFile file`

- **POST /api/businesscards/createMany**  
  Create multiple business cards at once.  
  **Body:** `List<CreateBusinessCardDto>`  

- **POST /api/businesscards/exportfile**  
  Export business cards to a file (CSV/XML).  
  **Body:** `ExportRequestDto`  
  **Response:** File content with proper Content-Type

- **POST /api/businesscards/printpdf**  
  Generate a PDF of business cards.  
  **Body:** `GeneratePdfCommand`  
  **Response:** PDF file



## Frontend Usage

- **Business Cards Table**
  - View all business cards in a table.
  - Global search/filter across all fields (Name, Email, Phone, etc.).
  - Import business cards from CSV or XML files.
  - Export business cards to CSV or XML files.
  - Print business cards directly from the table.
  - Edit, delete, or view details of each business card.

- **Imported Files Preview Page**
  - View Saved card
  - View imported CSV/XML files before saving them.
  - Edit individual business cards inside the imported file.
  - Save selected cards to the main database after review.

- **Add New Business Card**
  - Add via a form with all required fields.
  - Optional photo/logo upload (Base64 encoded).


## Unit Tests
- Run `dotnet test BusinessCardsTestProject` to execute unit tests.


## Database
- The application uses SQL Server as its database. Below is the structure of the main table used in the project:
| Column               | Type             | Required | Description                                |
| -------------------- | ---------------- | -------- | ------------------------------------------ |
| Id                   | UNIQUEIDENTIFIER | Yes      | Primary Key, GUID                          |
| ArabicName           | NVARCHAR(MAX)    | Yes      | Business card name in Arabic               |
| EnglishName          | NVARCHAR(MAX)    | Yes      | Business card name in English              |
| DateOfBirth          | DATETIME2(7)     | Yes      | Date of Birth                              |
| Email                | NVARCHAR(MAX)    | Yes      | Email address                              |
| Phone                | NVARCHAR(MAX)    | Yes      | Phone number                               |
| Logo                 | NVARCHAR(MAX)    | No       | Photo/logo stored as Base64                |
| Address              | NVARCHAR(MAX)    | Yes      | Physical address                           |
| IsDeleted            | BIT              | Yes      | Soft delete flag (0 = active, 1 = deleted) |
| DeletedByUserId      | UNIQUEIDENTIFIER | No       | User who deleted the record                |
| DeleteDate           | DATETIME2(7)     | No       | Date of deletion                           |
| CreatedByUserId      | UNIQUEIDENTIFIER | Yes      | User who created the record                |
| CreatedDate          | DATETIME2(7)     | Yes      | Record creation timestamp                  |
| LastModifiedByUserId | UNIQUEIDENTIFIER | No       | User who last modified the record          |
| LastModifiedDate     | DATETIME2(7)     | No       | Last modification timestamp                |

- Run script.sql

To create the database and insert sample data:

Open SQL Server Management Studio (SSMS)

Select: script.sql

Click Execute or press F5

This script will create the database HireMind_DB, generate the BusinessCards table, and insert sample test data.
