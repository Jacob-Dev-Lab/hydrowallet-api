# ADR-002: Used Clean Architecture

**Status:** Accepted
**Decision:** Organised Hydro Wallet using Clean Architecture principles.

## Context

Hydro Wallet is a financial system intended to demonstrate the design and implementation of a maintainable backend system rather than simply provide a working CRUD API.

The application will eventually contain business rules around users, wallets, transactions, currency conversion and financial accounting.

The system is therefore designed to avoid tight coupling >>> business logic to ASP.NET Core, Entity Framework Core, SQL Server or other infrastructure technologies.

## Decision

The application is divided into four primary layers:

* Wallet.Api
* Wallet.Application
* Wallet.Infracstructure
* Wallet.Domain

### Wallet.Domain

Contains the core business model.

Examples include:

* Entities
* Aggregates
* Value objects
* Enums
* Domain rules
* Domain exceptions

The domain layer should not depend on ASP.NET Core, Entity Framework Core or SQL Server.

### Wallet.Application

Contains application use cases and orchestration.

Examples include:

* Commands
* Handlers
* Responses
* Application interfaces
* Application validation

The application layer coordinates business operations without implementing infrastructure concerns.

### Wallet.Infrastructure

Contains technical implementations required by the application.

Examples include:

* Entity Framework Core
* DbContext
* Repository implementations
* SQL Server configuration
* Password hashing implementation
* Database migrations

Infrastructure implemennts interfaces defined by the inner layers where appropriate.

### Wallet.Api

Contains the HTTP-facing concerns of the application.

Examples include:

* Controllers
* HTTP status codes
* Request/response handling
* Middleware
* Global exception handling
* Swagger/OpenAPI configuration

## Why This Architecture?

The primary reason for this architecture is separation of concerns.

The registration use case shouldn't care whether a user ends up in SQL Server, PostgreSQL or something else entirely. And the domain model has no business knowing it was reached through an HTTP request in the first place.

This separation makes individual parts of the system easier to test, replace and evolve.

## Alternatives Considered

### Traditional Layered Architecture

A conventional controller/service/repository structure would have been simpler to implement.

However, because Hydro Wallet is being designed as a portfolio project with increasingly complex domain behaviour, I chose stronger separation between domain, application and infrastructure concerns.

### Single Web API Project

Another option was to place controllers, business logic, EF Core configuration and domain entities inside one project.

This would reduce the initial project structure but would create stronger coupling (tight coupling) between business logic and infrastructure.

It was rejected because it would make the architectural boundaries less explicit.

## Consequences

### Positive

* Business logic is isolated from infrastructure.
* Domain logic can be tested independently.
* Infrastructure technologies can potentially be replaced.
* Dependencies are easier to reason about.
* The architecture provides clear boundaries as the application grows.

### Negative

* More projects and files are required.
* The architecture introduces additional concepts and abstractions.

## Future Considerations

The architecture will be reviewed as the application grows.

Abstractions will only be introduced where they provide meaningful value.
