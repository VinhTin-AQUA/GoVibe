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

- controller

```txt
Controllers/
 ├── Products/
 │    ├── AdminProductsController.cs
 │    ├── UserProductsController.cs
 │    └── PublicProductsController.cs
 │
 ├── Users/
 │    ├── AdminUsersController.cs
 │    ├── UserUsersController.cs
 │    └── PublicUsersController.cs
 │
 └── Orders/
      ├── AdminOrdersController.cs
      ├── UserOrdersController.cs
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

## resource

- https://vietnamtourism.gov.vn/post/66450

## Features

### Admin:

- CURD phân loại, CURD địa điểm ✅
- Xem review từng địa điểm ✅
- Biểu đồ thống kê:
    - Địa điểm được đi nhiều nhất (dựa vào user_activity_log hoặc itinerary_items)
    - Số lượt truy cập theo địa điểm (dựa vào user_activity_log với action='view')
    - Tổng lượt truy cập toàn hệ thống
- Xem danh sách hỗ trợ & đánh giá sản phẩm từ user (bảng app_feedback)
- Xem danh sách hỗ trợ thêm địa điểm (bảng support_requests)
- lý người dùng đăng nhập (CRUD users, block/unblock)
- thông báo

### Client:

- Đăng nhập Google + thêm sở thích, hay đi 1 mình/nhiều người (lưu vào users.preferences)
- Tìm kiếm:
    - Theo tên, đánh giá
    - Lọc theo nhóm đi cùng (cần mapping với tags hoặc custom field) -> Thêm bảng place_suitability (place_id, group_type)
    - Lọc theo tâm trạng/hoạt động -> tags
    - Lọc theo ngân sách (price_min, price_max)
    - Lọc thời gian thực: "Đang mở cửa" (so sánh giờ hiện tại với opening_hours), "Còn bàn trống" (phức tạp, có thể bỏ qua MVP), "Đang có ưu đãi" (join promotions)
- Hiển thị bản đồ (Leaflet/Google Maps + PostGIS)
- Lưu điểm đến yêu thích (favorites)
- Đánh giá địa điểm (reviews)
- Lưu lịch trình (itineraries + items)
- Hỗ trợ thêm địa điểm mới (support_requests)
- Đánh giá phần mềm (app_feedback)

### AI:

- Hiển thị sự kiện (wiki) & đề xuất 1 điểm đến -> Dùng bảng events + gợi ý place ngẫu nhiên theo sở thích user
- Gợi ý lịch trình hôm nay theo sự kiện (gợi ý event gần nhất + place liên quan)
- Tự động lên lịch trình theo sở thích (dùng preferences + gợi ý place phù hợp)
- Lộ trình di chuyển tối ưu: API riêng nhận danh sách place_id, tính toán thứ tự tối ưu dùng Google Distance Matrix hoặc OSRM

### Vấn đề

- giữa các service có mối quan hệ, ví dụ user thêm sản phẩm vào giỏ hàng => tạo service cardService

- với search engine, mỗi product sẽ có các ảnh, vậy ảnh lưu ở đâu để elastic search trả về kết quả có kèm ảnh trong mỗi product
    - với ảnh public
        - tạo image proxy, khi backend gửi url về client thì mã hóa aes key, key này gửi về proxy giải mã để lấy key trả ảnh về client

    - với ảnh private
        - chỉ lưu key, backend gen presignedurrl

    

### step

- theme
- giao diện home
- giao diện search
- login
- profile

- thiết kế lại database
- thêm các method upload, xóa, get file
- sinh dữ liệu ngẫu nhiên
- biểu đồ thống kê
- api gợi ý địa điểm cho trang home theo từng danh mục
- api tìm kiếm
    - tìm kiếm thuần database
    - sử dụng elastíc search
    - tìm kiếm theo bộ lọc
- đăng nhập google
- lưu điêm đến yêu thích
- đánh giá
- lưu lịch trình
- hỗ trợ và trợ giúp thêm các địa điểm mới
- cào dữ liệu
- hỗ trợ và đánh giá sản phẩm

## Thiết kế

1. Các vai trò:

- Admin - quản lý nền tảng
- Client (Người dùng) - người dùng cuối
- AI - tính năng thông minh (có thể là service riêng hoặc tích hợp)

2. Luồng chức năng chính:

- Tìm kiếm + lọc địa điểm (phức tạp, cần full-text search, geo search)
- Bản đồ
- Yêu thích, đánh giá, lịch trình
- Hỗ trợ thêm địa điểm mới (crowdsourcing)
- Gợi ý AI (sự kiện, lịch trình, tối ưu lộ trình)

## API

- Auth
    - POST /auth/google - login google
    - GET /auth/profile - lấy thông tin user
    - PUT /auth/preferences - cập nhật sở thích

- laces
    - GET /places - danh sách địa điểm (có phân trang, filter)
    - GET /places/search - search với Elasticsearch
    - GET /places/nearby - lấy địa điểm gần vị trí
    - GET /places/:id - chi tiết
    - POST /places - user đề xuất địa điểm mới (chỉ tạo pending)
    - PUT /places/:id - admin approve/edit
    - DELETE /places/:id - admin delete

- Favorites
    - GET /favorites - danh sách yêu thích
    - POST /favorites/:placeId - thêm
    - DELETE /favorites/:placeId - xóa

- Reviews
    - /places/:placeId/reviews
    - POST /places/:placeId/reviews
    - PUT /reviews/:id
    - DELETE /reviews/:id

- Itineraries
    - CRUD /itineraries
    - GET /itineraries/:id/optimize - tối ưu lộ trình

- Admin
    - GET /admin/dashboard/stats - thống kê
    - GET /admin/support-requests
    - GET /admin/feedbacks
    - PUT /admin/users/:id/role
- AI Suggestions
    - GET /ai/suggest-today - gợi ý lịch trình hôm nay
    - GET /ai/events - lấy sự kiện wiki
    - POST /ai/optimize-route - tối ưu lộ trình
