```txt
Client
   │
   ▼
API Gateway
   │
   ├── Place API
   └── Search API
```

Database: Postgresql
File: MinIO
Search: ElasticSearch

Entities:

- Categories (Du lịch ,Quán ăn ,Cafe ,Bar ,Khu vui chơi ,Trung tâm thương mại)

    ```scheme
    Categories
    -----------
    id (PK)
    name
    description
    icon
    created_at
    ```

- Places (địa điểm)

    ```scheme
    Places
    --------
    id (PK)
    name
    description
    address_id
    category_id (FK -> Categories.id)

    phone
    website
    opening_hours

    average_rating
    total_reviews

    created_at
    updated_at
    status (active, pending, hidden)
    ```

- Address

    ```scheme
    Addresses
    --------
    id (PK)

    street
    ward
    district
    city
    country

    full_address
    ```

- Place_Images

    ```scheme
    Place_Images
    --------------
    id (PK)
    place_id (FK -> Places.id)
    image_url
    caption
    uploaded_by (FK -> Users.id)
    created_at
    ```

- Reviews (đánh giá)

    ```scheme
    Reviews
    ---------
    id (PK)
    place_id (FK -> Places.id)
    user_id (FK -> Users.id)

    rating          (1-5)
    comment
    created_at
    updated_at
    ```

- Review_Images

    ```scheme
    Review_Images
    --------------
    id (PK)
    review_id (FK -> Reviews.id)
    image_url
    ```

- Amenities (tiện ích) (Wifi, Bãi giữ xe, Máy lạnh, Phòng VIP, View đẹp)

    ```scheme
    Amenities
    -----------
    id (PK)
    name
    icon
    ```

- Place_Amenities (nhiều - nhiều)

    ```scheme
    Place_Amenities
    ----------------
    place_id (FK -> Places.id)
    amenity_id (FK -> Amenities.id)

    PRIMARY KEY(place_id, amenity_id)
    ```

## Migration

- get

```bash
dotnet ef migrations list --project GoVibe.Infrastructure --startup-project GoVibe.API 
```

- Add

```bash
dotnet ef migrations add InitialCreate --project GoVibe.Infrastructure --startup-project GoVibe.API
```

- update database

```bash
dotnet ef database update --project GoVibe.Infrastructure --startup-project GoVibe.API

dotnet ef database update 0 --project GoVibe.Infrastructure --startup-project GoVibe.API 
```

- remove migration

```bash
dotnet ef migrations remove --project GoVibe.Infrastructure --startup-project GoVibe.API 
```

## Frontend

```bash
ng new my-workspace --no-create-application

cd my-workspace
ng generate application my-app
ng generate library my-lib

ng g library components --project-root=libs/components;

ng s dashboard
```