# DEV

## solution

```txt
src/
│
├── BuildingBlocks (shared libraries)/
│   ├── EventBus/
│   │   ├── RabbitMQ/
│   │   └── Abstractions/
│   │
│   ├── Logging/
│   │   └── Serilog/
│   │
│   ├── Common/
│   │   ├── Extensions/
│   │   ├── Exceptions/
│   │   └── Middleware/
│   │
│   └── Contracts/
│       ├── Events/
│       └── DTOs/
│
├── Services/
│   ├── AuthService/
│   │   ├── Auth.API/
│   │   ├── Auth.Application/
│   │   ├── Auth.Domain/
│   │   └── Auth.Infrastructure/
│   │
│   ├── OrderService/
│   │   ├── Order.API/
│   │   ├── Order.Application/
│   │   ├── Order.Domain/
│   │   └── Order.Infrastructure/
│   │
│   ├── ProductService/
│   │   ├── Product.API/
│   │   ├── Product.Application/
│   │   ├── Product.Domain/
│   │   └── Product.Infrastructure/
│   │
│   ├── SearchService/
│   │   ├── Search.API/
│   │   ├── Search.Application/
│   │   ├── Search.Domain/
│   │   └── Search.Infrastructure/

├── ApiGateway/
│   └── Gateway.API/
│
├── BackgroundJobs/
│   └── WorkerService/
│
└── docker/
    ├── docker-compose.yml
    └── elk/
```

### EventBus

- Wrapper cho RabbitMQ

```cs
public interface IEventBus
{
    Task PublishAsync<T>(T @event);
    void Subscribe<T, TH>();
}
```

### Contracts

- Chứa Integration Events

```cs
public class ProductCreatedEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
```

### Common

- BaseEntity
- Exception
- Middleware

### Logging

- Setup Serilog dùng chung

```cs
public static class LoggingExtensions
{
    public static void AddCustomLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog(...);
    }
}
```

- Gọi hàm log trong mỗi service trực tiếp đến ELK

### Cấu trúc bên trong 1 service

```txt
OrderService/

├── Order.API/
│   ├── Controllers/
│   ├── Middleware/
│   ├── Program.cs
│
├── Order.Application/
│   ├── Commands/
│   ├── Queries/
│   ├── Handlers/
│   ├── DTOs/
│
├── Order.Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Interfaces/
│
├── Order.Infrastructure/
│   ├── Persistence/
│   ├── Repositories/
│   ├── EventBus/
```

### BackgroundJobs

- Consume RabbitMQ: restart services thì không làm mất consumer
- Gửi email
- Sync dữ liệu
- Cron job
- Retry failed job
- Search Worker
- Notification Worker
- Analytics Worker
- Cấu trúc BackgroundJobs

```txt
BackgroundJobs/

├── SearchWorker/
│   ├── Worker.cs
│   ├── EventHandlers/
│   └── Program.cs
│
├── NotificationWorker/
│   ├── EmailService.cs
│
└── Shared/
```

## d
