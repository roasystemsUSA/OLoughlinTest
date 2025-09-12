# OLoughlinTest

Mini aplicación de **Gestión de Citas** con clientes y citas, desarrollada como prueba técnica.  
Incluye backend en **.NET 9 WebAPI** con **Entity Framework Core (Database First)**,  
frontend en **Angular 20**, persistencia en **SQL Server**, y despliegue en **IIS**.

---

## 🚀 Tecnologías
- Backend: ASP.NET Core 9 (WebAPI REST, JSON)
- ORM: Entity Framework Core (Database First)
- Frontend: Angular 20, HTML5, CSS3
- Base de datos: Microsoft SQL Server
- Despliegue: IIS 10

---

## 📂 Estructura del repositorio

/backend -> API REST en .NET 9
/frontend -> Angular 20 (UI)
/db -> Scripts de base de datos (schema.sql, seed.sql opcional)

---

## 🗄️ Base de Datos
Esquema definido en `/db/schema.sql`.

```sql
CREATE TABLE Customers (
    Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Email NVARCHAR(255) NULL
);

CREATE TABLE Appointments (
    Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    CustomerId UNIQUEIDENTIFIER NOT NULL REFERENCES Customers(Id),
    DateTime DATETIME2 NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('scheduled','done','cancelled'))
);

-- Constraint opcional para evitar doble booking
CREATE UNIQUE INDEX UQ_Appointments_Slot 
    ON Appointments(CustomerId, DateTime);
