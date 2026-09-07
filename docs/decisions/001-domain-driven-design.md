# ADR-001: Used Domain-Driven Design for the Domain Layer

**Date:** 2026-09-05
**Decision:** Used Domain-Driven Design (DDD) principles for domain modelling.

## Context

Hydro Wallet is a financial application, and operations like user registration, wallet creation, deposits, withdrawals and currency conversion all carry business rules that matter. Those rules can't just live in the API layer or get enforced through database constraints, they need somewhere to sit.

For example, a user's lifecycle is governed by rules around states such as:

* Pending
* Active
* Locked
* Suspended
* Deleted

As the system grows, additional business rules will be introduced around wallets, transactions, journals, balances and KYC. So the architecture needs a clear home for business logic, separate from infrastructure and UI concerns.

## Decision

I decided to apply Domain-Driven Design principles to the domain layer.

The domain model contains entities, value objects, domain rules and domain behaviour that represent concepts within the wallet system.

The `User` entity is modelled as an aggregate root.
Values like `Email`, `MobileNumber` and `BDateOfBirth` are represented as value objects that have their own validation and meaning.

## Why an Aggregate Root?

`User` is treated as an aggregate root because it defines a consistency boundary around user state and behaviour. Application code shouldn't be able to reach in and change important user properties directly. State changes only goes through behaviour the `User` entity itself exposes.

For example:

* `Activate()`
* `Lock()`
* `Unlock()`
* `Suspend()`
* `Reinstate()`
* `Delete()`

This allows lifecycle rules to remain close to the domain object that owns them rather than letting them scatter accross controllers, handlers and infrastructure code.

## Why Value Objects?

Certain values have domain meaning and validation rules but do not require their own identity.

Examples include:

* Email address
* Mobile number
* Date of birth

These are therefore represented as value objects rather than plain strings.

For example, `Email` is responsible for ensuring that an email value satisfies the domain's email requirements, while `MobileNumber` is responsible for the required UK mobile-number format.

This keeps the domain model expressive and prevents invalid values from being freely passed throughout the system.

## Alternatives Considered

### An Anemic Domain Model

One alternative was to use entities primarily as data containers and place most business rules inside application services or handlers.

This approach would make the domain objects simpler, but it would also make it easier for business rules to become scattered throughout the application.

This was rejected because Hydro Wallet is expected to contain increasingly complex financial business rules.

### Validation Only in the API Layer

Another option was to perform all validation in controllers or API request models.

This was rejected because the domain should not depend on whether a request originated from an HTTP API.

## Consequences

### Positive

* Business rules are closer to the domain concepts they govern.
* The domain model becomes more expressive.
* Value objects provide stronger guarantees around important values.
* Domain logic can be tested independently of the database and API.
* The approach provides a foundation for modelling more complex financial concepts later.

### Negative

* The model is more complex than using simple DTOs and database entities.
* Some persistence configuration is required to map value objects to the database.

## Future Considerations

As Hydro Wallet grows, aggregate boundaries will be reviewed carefully.

The decision is therefore not to make every entity an aggregate, but to introduce aggregate boundaries where they provide meaningful control over business invariants.
