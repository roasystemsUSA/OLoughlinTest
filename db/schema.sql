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