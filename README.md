#  Advance Banking Application

A scalable full-stack banking platform built using ASP.NET Core, Angular, SQL Server, and Azure DevOps. The application simulates real-world banking operations through a modular microservice-inspired architecture, providing secure authentication, customer account management, transaction processing, enquiry services, and administrative workflows.

##  Project Overview

Advance Banking Application was developed to model core banking operations while emphasizing backend engineering principles such as API development, database design, authentication, testing, and CI/CD deployment practices.

The system is organized into multiple independent services, allowing separation of concerns and easier scalability while providing a modern Angular-based user interface.

---

##  Key Features

### Authentication & Security
- User authentication and authorization
- Secure login workflows
- Role-based access management
- Protected API endpoints

### Customer & Account Management
- Customer profile management
- Account creation and maintenance
- Account information retrieval
- Banking relationship management

### Transaction Processing
- Deposit and withdrawal operations
- Fund transfers
- Transaction history tracking
- Account balance updates

### Enquiry Services
- Account enquiry functionality
- Transaction lookups
- Customer information retrieval
- Banking service requests

### Administrative Operations
- Manager-level controls
- Customer account oversight
- Operational monitoring
- Service management workflows

---

##  System Architecture

```text
                    ┌─────────────────┐
                    │ Angular Frontend│
                    └────────┬────────┘
                             │
        ┌────────────────────┼────────────────────┐
        │                    │                    │
        ▼                    ▼                    ▼
┌─────────────┐     ┌────────────────┐   ┌──────────────┐
│ Auth API    │     │ Customer API   │   │ Manager API  │
└─────────────┘     └────────────────┘   └──────────────┘
                             │
                             ▼
                    ┌────────────────┐
                    │ SQL Database   │
                    └────────────────┘
```

---

##  Technology Stack

### Backend
- ASP.NET Core
- C#
- REST APIs
- Entity Framework
- SQL Server

### Frontend
- Angular
- TypeScript
- HTML5
- CSS3

### DevOps & Tools
- Azure DevOps
- Azure Pipelines
- Git
- Swagger
- Postman

---

##  Repository Structure

```text
Advance-Banking-Application/
│
├── Authentication-Api/
│   └── Authentication services
│
├── GroupA-Cust-Acc-Trans-Api/
│   └── Customer, Account & Transaction APIs
│
├── GroupA-Enquiry-Api/
│   └── Enquiry services
│
├── GroupA-Manager-Api/
│   └── Administrative operations
│
└── GroupA-FrontEnd-Angular/
    └── Angular frontend application
```

---

##  Getting Started

### Prerequisites

- .NET SDK
- SQL Server
- Node.js
- Angular CLI
- Visual Studio / VS Code

### Clone Repository

```bash
git clone https://github.com/ATHISH-RAJ-MOHAN/Advance-Banking-Application.git

cd Advance-Banking-Application
```

### Backend Setup

```bash
cd Authentication-Api

dotnet restore

dotnet build

dotnet run
```

Repeat for the remaining API projects.

### Frontend Setup

```bash
cd GroupA-FrontEnd-Angular

npm install

ng serve
```

Application will be available at:

```text
http://localhost:4200
```

---

##  Testing

- API validation using Swagger
- Endpoint testing using Postman
- Unit and integration testing for backend services
- Frontend validation using Angular testing utilities

---

##  CI/CD

The project includes Azure Pipeline configurations that support:

- Automated builds
- Continuous Integration
- Deployment workflows
- Quality validation checks

---

##  Engineering Highlights

- Designed modular backend services for maintainability and scalability.
- Built RESTful APIs supporting banking operations and account management.
- Implemented secure authentication and authorization workflows.
- Integrated SQL-based persistence for transactional data management.
- Leveraged Azure DevOps pipelines for automated build and deployment processes.
- Applied software engineering best practices including code reviews, testing, and version control.

---

##  Skills Demonstrated

- Backend Software Engineering
- REST API Development
- Database Design & SQL
- System Integration
- Authentication & Security
- CI/CD Pipelines
- Full-Stack Development
- Agile Development
- Cloud Deployment Workflows

---


MS Applied Data Science, University of Southern California

- LinkedIn: https://linkedin.com/in/athish-raj-mohan
- GitHub: https://github.com/ATHISH-RAJ-MOHAN
