# ADR-003: Application and Domain Validation Strategy

**Status:** Accepted
**Decision:** Used FluentValidation for application/request validation while keeping domain validation within domain objects.

## Context

User Registration requires validation at different levels.

Some validation concerns are related to the structure of an incoming application request. For example, a registration request should contain a first name, a password and a password confirmation, and the password confirmation should match the supplied password.

Other validation rules represent actual business or domain rules. Examples include whether an email address is valid according to the application's domain requirements, whether a mobile number follows the required format, and whether a user's date of birth satisfies the application's age restrictions.

It would have been possible to place all of these rules inside the API, application handler or domain model. However, doing so would mix responsibilities and make the location of business rules less clear.

## Decision

I decided to separate validation into two responsibilities:

1. **Application-level validation** is handled using FluentValidation.
2. **Domain-level validation** remains inside the domain model, primarily through value objects and domain entities.

This means that validation is performed according to the responsibility of the layer that owns the rule.

---

## Application Validation

FluentValidation is used to validate the `RegisterUserCommand` before the registration use case proceeds.

Examples include:

* First name must be supplied.
* Other names must be supplied.
* Password must be supplied.
* Password must meet the minimum length requirement.
* Confirm password must be supplied.
* Confirm password must match the password.
* Required request fields must not be empty.
* Request fields must satisfy application-level length constraints.

These rules concern the validity and structure of the incoming application request.

The validator therefore exists alongside the registration use case in the Application layer.

For example:

```text
Wallet.Application
└── Users
    └── Registration
        ├── RegisterUserCommand.cs
        ├── RegisterUserHandler.cs
        ├── RegisterUserResponse.cs
        └── RegisterUserValidator.cs
```

---

## Domain Validation

Domain validation is handled within the Domain layer.

Important values are represented using value objects rather than unrestricted primitive values.

Examples include:

* `Email`
* `MobileNumber`
* `BDateOfBirth`

These objects enforce the rules associated with the values they represent.

For example:

```text
Email.Create(...)
MobileNumber.Create(...)
BDateOfBirth.Create(...)
```

The domain remains responsible for determining whether these values are valid according to the set business rules.

The `User` aggregate also protects its own invariants and lifecycle rules.

---

## Why Separate the Two?

The primary reason for this separation is **responsibility**.

Application validation answers:

> "Is this request properly formed so that the use case can process it?"

Domain validation answers:

> "Is this value or operation valid according to the set business rules of the domain?"

These are related concerns, but they are not the same concern.

---

## Why Not Put All Validation in FluentValidation?

An alternative would have been to place all validation rules inside FluentValidation.

This would have been very simple and convenient because all validation would be in one location.

However, it would create an undesirable dependency:

```text
Business Rule
     ↓
FluentValidation
```

This means the validity of an important domain value would become dependent on the application layer and the way the request is processed.

The same domain object could potentially be created through another application use case, background process, message consumer or future interface without passing through the same validator.

Keeping domain invariants inside the domain model means they remain enforceable regardless of how the domain is accessed.

---

## Validation Flow

The resulting registration flow is:

```text
HTTP Request
     │
     ▼
RegisterUserCommand
     │
     ▼
FluentValidation
     │
     │ invalid
     ├──────────────► Validation errors
     │
     ▼ valid
RegisterUserHandler
     │
     ▼
Domain Value Objects
     │
     ├── Email.Create(...)
     ├── MobileNumber.Create(...)
     └── BDateOfBirth.Create(...)
     │
     ▼
User Aggregate
     │
     ▼
Persistence
```

This creates a layered validation boundary without duplicating every rule in every layer.

---

## Avoiding Duplicate Validation

A deliberate decision was made not to duplicate domain rules unnecessarily in FluentValidation.

For example, the domain owns the detailed validation of `Email`.

FluentValidation therefore only ensures that an email value is present as part of the registration request rather than reproducing the complete email-format rule.

The same principle applies to:

* Mobile number format
* Date of birth restrictions
* Other domain-specific invariants

---

## Consequences

### Positive

* Application and domain responsibilities remain clearly separated.
* Domain invariants remain enforceable regardless of the entry point into the system.
* Request validation can evolve independently from domain rules.
* FluentValidation provides a clear mechanism for expressing multiple request validation errors.
* Domain objects remain responsible for protecting the correctness of the values they represent.
* Validation logic becomes easier to test at the appropriate layer.

### Negative

* Validation exists in more than one location.
* Some rules may appear similar across application and domain boundaries and therefore require careful consideration to avoid unnecessary duplication.

---

## Testing Strategy

Application validators should be tested independently to verify request-level validation.

Domain value objects and entities should have their own tests to verify domain invariants.

This results in separate test responsibilities:

```text
Application Tests
        │
        └── Request/application validation

Domain Tests
        │
        ├── Email rules
        ├── Mobile number rules
        ├── Date of birth rules
        └── User aggregate invariants
```

This separation helps identify whether a failure is caused by an invalid request or by a violation of a domain rule.

---

## Future Considerations

As Hydro Wallet grows, additional validation rules will be introduced.

Each rule should be classified according to its responsibility:

1. Is it about the structure or requirements of an application request?
2. Is it a business invariant that must always hold within the domain?

This classification will determine where the rule belongs.
