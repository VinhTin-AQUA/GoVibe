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
│
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

## Exception

```txt
info: GoVibe.API.Middlewares.GlobalExceptionHandlingMiddleware[0]
      ===========================
fail: GoVibe.API.Middlewares.GlobalExceptionHandlingMiddleware[0]
      An unhandled exception occurred. RequestId: 0HNK5HQK5L8HF:00000001, Path: /api/Places
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Npgsql.PostgresException (0x80004005): 23503: insert or update on table "Place" violates foreign key constraint "FK_Place_Categories_CategoryId"

      DETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.
         at Npgsql.Internal.NpgsqlConnector.ReadMessageLong(Boolean async, DataRowLoadingMode dataRowLoadingMode, Boolean readingNotifications, Boolean isReadingPrependedMessage)
         at System.Runtime.CompilerServices.PoolingAsyncValueTaskMethodBuilder`1.StateMachineBox`1.System.Threading.Tasks.Sources.IValueTaskSource<TResult>.GetResult(Int16 token)
         at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming, CancellationToken cancellationToken)
         at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteReader(Boolean async, CommandBehavior behavior, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteReader(Boolean async, CommandBehavior behavior, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
        Exception data:
          Severity: ERROR
          SqlState: 23503
          MessageText: insert or update on table "Place" violates foreign key constraint "FK_Place_Categories_CategoryId"
          Detail: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.
          SchemaName: public
          TableName: Place
          ConstraintName: FK_Place_Categories_CategoryId
          File: ri_triggers.c
          Line: 2610
          Routine: ri_ReportViolation
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.NpgsqlExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at GoVibe.Infrastructure.Data.AppDbContext.SaveChangesAsync(CancellationToken cancellationToken) in C:\Users\tinhv\Desktop\f\GoVibe\GoVibe\GoVibe.Infrastructure\Data\AppDbContext.cs:line 36
         at GoVibe.Infrastructure.UnitOfWork.UnitOfWork.CommitAsync() in C:\Users\tinhv\Desktop\f\GoVibe\GoVibe\GoVibe.Infrastructure\UnitOfWork\UnitOfWork.cs:line 40
         at GoVibe.Infrastructure.UnitOfWork.UnitOfWork.CommitAsync() in C:\Users\tinhv\Desktop\f\GoVibe\GoVibe\GoVibe.Infrastructure\UnitOfWork\UnitOfWork.cs:line 46
         at GoVibe.API.Services.PlaceService.Add(AddPlaceRequest request) in C:\Users\tinhv\Desktop\f\GoVibe\GoVibe\GoVibe.API\Services\PlaceService.cs:line 72
         at GoVibe.API.Services.PlaceService.Add(AddPlaceRequest request) in C:\Users\tinhv\Desktop\f\GoVibe\GoVibe\GoVibe.API\Services\PlaceService.cs:line 86
         at GoVibe.API.Controllers.Places.PlacesController.Add(AddPlaceRequest request) in C:\Users\tinhv\Desktop\f\GoVibe\GoVibe\GoVibe.API\Controllers\Places\PlacesController.cs:line 30
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.TaskOfIActionResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at GoVibe.API.Middlewares.GlobalExceptionHandlingMiddleware.InvokeAsync(HttpContext context) in C:\Users\tinhv\Desktop\f\GoVibe\GoVibe\GoVibe.API\Middlewares\GlobalExceptionHandlingMiddleware.cs:line 26
info: GoVibe.API.Middlewares.GlobalExceptionHandlingMiddleware[0]
      ===========================
```