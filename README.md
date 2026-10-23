# Personal Projects

A collection of personal C#/.NET projects and SQL exercises exploring desktop applications, reusable data access, authentication, and relational queries.

Each folder is an independent project with its own structure and development stage. The descriptions below reflect the current source code, including the parts that are still being developed.

## Projects at a glance

| Folder | Project | Technologies | Main focus |
| --- | --- | --- | --- |
| [BLACKJACK](BLACKJACK/) | Desktop Blackjack game | C#, WPF, .NET 8 | MVVM, commands, game services, and UI state |
| [GENERIC-REPO](GENERIC-REPO/) | Generic data-access experiment | C#, .NET 10, EF Core, SQL Server | Generic repositories, DTO conversion, owned types, and persistence |
| [IDP](IDP/) | Victoria Identity Provider | C#, ASP.NET Core, .NET 10, EF Core, SQL Server | Registration, authentication, sessions, token rotation, and validation |
| [SQL](SQL/) | Northwind query exercises | T-SQL, SQL Server | Joins, aggregation, subqueries, and relational problem solving |

## BLACKJACK

A WPF implementation of Blackjack against a local dealer. It includes betting with an in-memory credit balance, card dealing, hit/stand actions, score calculation, round results, and progression to the next round.

### Architecture

The application follows an MVVM structure with game responsibilities split into services:

- **Models and data:** `Card` represents a card and its possible values. `BlackJackDataContext` supplies the deck through `IDataContext`; card images are included as application resources.
- **Deck, rules, and validation:** separate services handle dealing, score calculation, outcomes, payouts, and numeric input validation. Ace values are adjusted between 1 and 11, and the dealer draws until reaching 17.
- **Game coordination:** `BlackJackGameService` receives its dependencies through its constructor and coordinates the deck and rule services. Its `CardsReceived` event delivers the loaded and shuffled deck to the view model.
- **View models:** `GameViewModel` maintains the round state, observable card collections, credits, and commands. `CardViewModel` controls whether a card shows its face or back.
- **Views and commands:** XAML bindings, `INotifyPropertyChanged`, and a custom `ICommand` implementation connect the interface to the view models. The layout control composes the services and view model manually.

The main interaction is **WPF command → view model → game service → rules/deck → event and property updates → UI**. This keeps card and scoring operations separate from their visual representation.

### Current scope and startup

The game keeps its state in memory. Its current rules cover the implemented local player/dealer flow; advanced options such as split, double down, and insurance are outside the current implementation.

On Windows, with an SDK compatible with `net8.0-windows`:

```bash
dotnet run --project "BLACKJACK/BlackJack.App/BlackJack.App.csproj"
```

The solution is `BLACKJACK/BlackJack.slnx`.

## GENERIC-REPO

An experiment in reusable EF Core data access, using recipes and ingredients as the working domain. The solution is named `CookingApp`, and currently consists of **Domain** and **Infrastructure** class libraries.

### Architecture

**Domain** contains recipe and ingredient entities, category and relationship models, DTOs, enums, and owned types for recipe steps, preparation times, and nutritional values. Explicit conversion operators define the transformations between entities and DTOs. `PagedResult<TDto>` carries paginated results and their metadata.

**Infrastructure** contains `CookingAppContext`, entity configurations, migrations, value converters, and the repository implementations. It references Domain; both projects currently declare EF Core packages.

### Implementation details

- **Separate read and write abstractions:** `ReadOnlyRepo<TEntity, TDto>` provides retrieval and pagination. `WriteRepo<TEntity, TDto>` handles updates, owned values and collections, and soft deletion.
- **Generic conversion:** `EfGenericRepo<TEntity, TDto>` discovers explicit conversion operators through reflection and creates delegates. The main conversion delegates are initialized per generic entity/DTO pair, with caching for additional conversions.
- **Owned-type handling:** repository operations inspect EF metadata and use DTO naming conventions and `KeyIdentifierAttribute` to identify related types and collection elements.
- **Persistence mappings:** configurations define relationships, enum conversions, and JSON mappings for owned data such as steps, times, and nutrients.
- **Persistence bookkeeping:** shadow properties store soft-delete state, creation/update timestamps, and a row version. Query filters exclude soft-deleted entities, while save operations update timestamps.

The data path is **EF entity → conversion delegate → DTO** for reads, and **DTO → tracked entity changes → explicit save** for writes. Changes are committed through `SaveChangesInDbAsync()`.

### Current scope and build

This is a data-access library experiment. There is no application host, UI, or API entry point in this folder. The write implementation is still evolving, including the insertion operation. SQL Server access requires configuring `CookingAppContext`, whose connection string is currently empty.

With the .NET 10 SDK:

```bash
dotnet build "GENERIC-REPO/CookingApp.slnx"
```

## IDP — Victoria Identity Provider

An ASP.NET Core authentication API developed when I had roughly eight months of .NET experience. It explores the path from validating registration input and verifying credentials to persisting sessions, issuing tokens, and managing their lifecycle.

### Solution architecture

The solution separates the HTTP host, presentation, application services, domain models, persistence, and tests into six projects:

| Project | Responsibility |
| --- | --- |
| `VictoriaIdentityProvider.Api` | Application entry point, dependency injection, configuration, middleware, JWT bearer authentication, and development OpenAPI |
| `Infrastructure.Presentation` | Authentication/registration controllers and centralized exception handling with Problem Details responses |
| `VictoriaIdentityProvider.Application` | DTOs, validators, service contracts, orchestration facades, user/session/token services, factories, and event dispatching |
| `VictoriaIdentityProvider.Domain` | Users, sessions, refresh tokens, audit records, repository contracts, state enums, and domain errors |
| `VictoriaIdentityProvider.Infrastructure` | EF Core SQL Server context, mappings, migrations, repositories, hashing, encryption, and audit components |
| `VictoriaIdentityProvider.Tests` | Service, validation, cryptography, and persistence test cases |

The API host wires interfaces to their implementations. Keyed services select different hashing implementations for passwords and refresh tokens. Facades coordinate operations across smaller services, while factories construct session/token models and provide a common entry point to repository operations.

### Registration and authentication

The current code includes:

- Registration with field validation, repeated-password checks, and duplicate-email checks.
- Password validation combining local rules with a Pwned Passwords API lookup.
- Argon2id password hashing with random salts and configurable parameters and pepper.
- SMTP email confirmation, confirmation-token validation, and resending confirmation links.
- Email/password login with configurable email-confirmation checks and failed-attempt lockout handling.
- HS256 access JWTs containing user and session claims.
- Random refresh tokens whose HMAC-SHA256 hashes are stored in the database.
- Token rotation, session expiration/revocation handling, and logout for one session or all user sessions.
- Client metadata capture, including IP address, User-Agent, device ID, and device name.

### Login and token lifecycle

The login path is:

```text
AuthController
  -> AuthFacade
  -> SessionService
  -> LoginService
  -> UserService / repository
```

The login service checks the account, confirmation state, lockout state, and password. The session service then uses factories to create the session and refresh token, captures client metadata, persists the records, and returns an access JWT with the raw refresh token. The database retains the refresh-token hash.

Refresh requests pass through `TokenRotationService` and `HeaderValidatorFacade`. The implementation validates token/session state, records revocation reasons, links the previous refresh token to its replacement, updates the session, and generates the next token pair.

This separates **credential verification**, **session state**, and **token issuance** into distinct responsibilities. Session and token records preserve lifecycle information such as expiration, revocation time, revocation reason, and replacement-token references.

### Persistence and supporting mechanisms

- EF Core repositories implement the domain contracts for users, sessions, and refresh tokens.
- Entity mappings define relationships and concurrency fields, with migrations recording schema changes.
- AES value converters handle selected user fields: email, first name, and last name.
- Row-version fields support optimistic concurrency checks for persisted state.
- A connected `SaveChangesInterceptor` maintains creation/update timestamps.
- An event dispatcher, `LoginEvent`, and a login-audit handler provide infrastructure for event-based auditing. Publishing login events from the authentication flow is still unfinished.

### Tests and development stage

The separate test project uses **xUnit**, **Moq**, and **EF Core InMemory**. It contains cases for validators, password hashing and encryption, user and registration services, login, session generation, token rotation, logout, and persistence operations.

Database-oriented tests use EF InMemory, and external interactions are mocked where implemented. These cases are distinct from exercising the running HTTP API against SQL Server and SMTP.

RSA signing, password reset, geolocation, and the remaining login-audit integration are still planned or incomplete. The current token implementation uses HS256. The project also retains components from earlier iterations as its structure evolves.

### Local setup and startup

The application targets **.NET 10** and requires an accessible SQL Server database with the schema prepared from the included migrations.

Before running the API, provide local configuration for:

- `ConnectionStrings:DefaultConnection`.
- Argon2 parameters and pepper, AES and HMAC keys, and JWT signing settings.
- SMTP connection details and the confirmation-link base URL for email flows.

User Secrets and environment variables can supply local settings. Password validation also makes an external Pwned Passwords request. Several connection/key/SMTP values in the checked-in configuration are empty.

After configuration, run from the repository root:

```bash
dotnet run --project "IDP/VictoriaIdentityProvider/VictoriaIdentityProvider.Api/VictoriaIdentityProvider.Api.csproj"
```

The solution is `IDP/VictoriaIdentityProvider/VictoriaIdentityProvider.slnx`.

## SQL

A collection of T-SQL exercises using the Northwind database. The scripts are stored in `SQL/sqlqueriesNorthwindDb/` and focus on expressing questions through relational queries.

### Concepts explored

- Multi-table joins across customers, orders, order details, products, categories, and employees.
- Self-joins for finding products ordered together.
- `GROUP BY`, `HAVING`, aggregate functions, and distinct counts.
- Subqueries, derived tables, and correlated comparisons.
- Date-based grouping and coverage checks using shipped-order dates.

Examples include finding frequently co-ordered product pairs, customers who ordered from every product category, and customers represented in every shipping year present in the data.

These are standalone query exercises. Run individual scripts against a SQL Server Northwind database using a SQL query client; the folder does not include database provisioning.

## Working with the repository

The commands above assume the repository root as the current directory. Each project can also be opened through its own solution or project file.

- **BLACKJACK:** Windows and .NET desktop/WPF support for `net8.0-windows`.
- **GENERIC-REPO and IDP:** .NET 10 SDK; SQL Server is needed for their database-backed operations.
- **SQL:** SQL Server with the Northwind sample database.

Across the projects, the recurring focus is on separating responsibilities, making state transitions explicit, and exploring how application code connects to its UI or persistence layer.
