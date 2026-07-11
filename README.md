# ClaimFlow 
An insurance claims processing system: submit a claim, get it validated,
scored asynchronously and decided.

![.NET Core CI](https://github.com/igor2080/ClaimFlow/actions/workflows/backend-ci.yml/badge.svg)

## Architecture
```mermaid
flowchart LR
    FE[React SPA] --> C[Caddy reverse proxy / HTTPS]
    C --> API[ASP.NET Core Web API]
    API -->|EF Core| DB[(PostgreSQL)]
    API -->|publish ClaimCreatedEvent| MQ[RabbitMQ / MassTransit]
    MQ --> W[Worker BackgroundService]
    W -->|scoring rules| DB
```


## Tech stack
```markdown
| Layer     | Technology                                    |
|-----------|-----------------------------------------------|
| API       | ASP.NET Core (.NET 9), FluentValidation       |
| Messaging | RabbitMQ via MassTransit                      |
| Data      | PostgreSQL, EF Core (code-first migrations)   |
| Worker    | .NET BackgroundService (MassTransit consumer) |
| Frontend  | React + TypeScript (Vite)                     |
| Infra     | Docker Compose, Caddy (reverse proxy, HTTPS)  |
| Tests     | xUnit                                         |
```
