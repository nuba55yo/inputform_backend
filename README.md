# 📌 InputForm Backend

.NET 8 Minimal API backend for the **InputForm** system.  
Provides endpoints for submitting input form data with a profile image (multipart/form-data) and reading lookup data such as occupations.  
Built with modern architecture using MediatR, EF Core, and PostgreSQL.

---

## 🧰 Tech Stack

- .NET 8 (Minimal API)
- MediatR 8.1
- Entity Framework Core
- PostgreSQL (Npgsql)
- Swagger / OpenAPI
- Scrutor (Auto Dependency Injection Registration)

---

## ✨ Features

✅ Minimal API architecture  
✅ Multipart file upload (profile image)  
✅ Store images as `bytea` in PostgreSQL  
✅ MediatR command / handler pattern  
✅ Repository layer  
✅ Auto DI registration  
✅ Swagger for API testing  
✅ UTC-safe timestamps for PostgreSQL  
✅ CORS ready for frontend integration  

---

## 🗃 Database

**Database Name:** `db_inputform`

---

## 📌 Create Database

```sql
CREATE DATABASE db_inputform;
```

---

## 📌 Create Tables

```sql
CREATE TABLE public.input_form_entries (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name VARCHAR(150) NOT NULL,
    last_name VARCHAR(150) NOT NULL,
    email VARCHAR(255) NOT NULL,
    phone VARCHAR(50) NOT NULL,
    occupation VARCHAR(150) NOT NULL,
    sex VARCHAR(10) NOT NULL,
    birth_day DATE NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE public.input_form_entry_images (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    entry_id UUID NOT NULL,
    profile_image BYTEA NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    
    CONSTRAINT fk_entry
        FOREIGN KEY(entry_id) 
        REFERENCES public.input_form_entries(id)
        ON DELETE CASCADE
);
```

> ⚠️ Important:  
PostgreSQL `TIMESTAMPTZ` requires **UTC timestamps** from the application.

Example:

```csharp
var now = DateTime.UtcNow;
```

---

## 🚀 Getting Started

### Clone Repository

```bash
git clone https://github.com/nuba55yo/inputform_backend.git
cd inputform_backend
```

---

### Configure Connection String

Update `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=db_inputform;Username=NestCleanApi;Password=admin1559;Pooling=true;Maximum Pool Size=50"
  }
}
```

---

### Trust HTTPS Certificate (Recommended)

```bash
dotnet dev-certs https --trust
```

---

### Run Application

```bash
dotnet run
```

Swagger UI:

```
https://localhost:7133/swagger
```

---

## 🔗 CORS (For Angular / Frontend)

```csharp
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

app.UseCors();
```

---

## 📌 API Endpoints

### ✅ Get Occupations
```
GET /api/inputform/occupations
```

---

### ✅ Create Input Form (Multipart)

```
POST /api/inputform
```

**Form Fields**

| Field | Type |
|--------|--------|
| first_name | string |
| last_name | string |
| email | string |
| phone | string |
| occupation | string |
| sex | male / female |
| birth_day | yyyy-MM-dd |
| profile | file |

---

## 🧪 Example CURL

```bash
curl -X POST "https://localhost:7133/api/inputform" \
  -F "first_name=John" \
  -F "last_name=Doe" \
  -F "email=john@example.com" \
  -F "phone=0812345678" \
  -F "occupation=Engineer" \
  -F "sex=male" \
  -F "birth_day=2000-01-01" \
  -F "profile=@./test.png;type=image/png"
```

---

## 📁 Project Structure

```
inputform
 ├── Common/
 ├── Persistence/
 │    ├── AppDbContext.cs
 │    └── Entities/
 ├── Service/
 │    └── Inputform/
 │         ├── InputformRequest.cs
 │         ├── InputformResponse.cs
 │         ├── InputformHandler.cs
 │         └── InputformRepository.cs
 └── Program.cs
```

---

## 🧠 Architecture Notes

- Uses **Minimal API + MediatR**
- Repository handles database logic
- Handler manages request flow
- PostgreSQL recommended for production workloads
- Designed for clean, scalable service growth

---

## ⚠️ Common Issues

### Antiforgery Error (Minimal API)

If you see:

```
Endpoint contains anti-forgery metadata...
```

Disable it for API endpoints:

```csharp
app.MapGroup("/api")
   .DisableAntiforgery();
```

---

## 👨‍💻 Author

Developed for the **InputForm** backend system.
