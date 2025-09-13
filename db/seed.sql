-- Establece la base de datos de destino
USE OLaughlinTestDB;
GO

-- Limpia las tablas para empezar de cero si es necesario (opcional)
-- Ten cuidado si ejecutas esto en un entorno con datos existentes.
-- DELETE FROM Appointments;
-- DELETE FROM Customers;

-- 1. Inserción de 50 registros de ejemplo en la tabla Customers
PRINT 'Inserting sample data into the Customers table...';

DECLARE @i INT = 1;
DECLARE @customerName NVARCHAR(120);
DECLARE @email NVARCHAR(255);
DECLARE @firstNameIndex INT;
DECLARE @lastNameIndex INT;

DECLARE @firstNames TABLE (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(50)
);
INSERT INTO @firstNames (Name) VALUES
('Joe'), ('John'), ('Jane'), ('Mike'), ('Emily'), ('Chris'), ('Sarah'), ('David'), ('Laura'), ('James'),
('Olivia'), ('Robert'), ('Jessica'), ('Daniel'), ('Isabella'), ('Matthew'), ('Sophia'), ('William'), ('Mia'), ('Alexander'),
('Charlotte'), ('Michael'), ('Amelia'), ('Benjamin'), ('Evelyn'), ('Jacob'), ('Harper'), ('Ethan'), ('Abigail'), ('Mason'),
('Elizabeth'), ('Lucas'), ('Sofia'), ('Aiden'), ('Ella'), ('Logan'), ('Grace'), ('Noah'), ('Chloe'), ('Jack'),
('Zoe'), ('Liam'), ('Penelope'), ('Ryan'), ('Layla'), ('Caleb'), ('Nora'), ('Isaac'), ('Hazel'), ('Wyatt');

DECLARE @lastNames TABLE (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(50)
);
INSERT INTO @lastNames (Name) VALUES
('Doe'), ('Smith'), ('Thompson'), ('Williams'), ('Brown'), ('Jones'), ('Garcia'), ('Miller'), ('Davis'), ('Rodriguez'),
('Martinez'), ('Hernandez'), ('Lopez'), ('Gonzalez'), ('Wilson'), ('Anderson'), ('Thomas'), ('Taylor'), ('Moore'), ('Jackson'),
('Martin'), ('Lee'), ('Perez'), ('Hall'), ('Young'), ('Allen'), ('Sanchez'), ('Wright'), ('King'), ('Scott'),
('Green'), ('Baker'), ('Adams'), ('Nelson'), ('Hill'), ('Ramirez'), ('Campbell'), ('Mitchell'), ('Roberts'), ('Carter'),
('Phillips'), ('Evans'), ('Turner'), ('Torres'), ('Parker'), ('Collins'), ('Edwards'), ('Stewart'), ('Morris'), ('Cox');

WHILE @i <= 50
BEGIN
    SET @firstNameIndex = ROUND(RAND() * 49, 0) + 1;
    SET @lastNameIndex = ROUND(RAND() * 49, 0) + 1;

    SELECT @customerName = FirstName.Name + ' ' + LastName.Name
    FROM @firstNames AS FirstName
    CROSS JOIN @lastNames AS LastName
    WHERE FirstName.Id = @firstNameIndex AND LastName.Id = @lastNameIndex;

    SET @email = REPLACE(@customerName, ' ', '.') + '@example.com';
    
    INSERT INTO Customers (Name, Email)
    VALUES (@customerName, @email);

    SET @i = @i + 1;
END;

PRINT 'Customers data insertion complete.';


-- 2. Inserción de 50 registros de ejemplo en la tabla Appointments
PRINT 'Inserting sample data into the Appointments table...';

-- Obtiene los IDs de los clientes insertados
DECLARE @customerIds TABLE (CustomerId UNIQUEIDENTIFIER);
INSERT INTO @customerIds (CustomerId) SELECT Id FROM Customers;

DECLARE @j INT = 1;
DECLARE @customerId UNIQUEIDENTIFIER;
DECLARE @appointmentDateTime DATETIME2;
DECLARE @status NVARCHAR(20);
DECLARE @randomValue INT;

WHILE @j <= 50
BEGIN
    -- Selecciona un CustomerId aleatorio
    SELECT TOP 1 @customerId = CustomerId FROM @customerIds ORDER BY NEWID();

    -- Genera una fecha y hora aleatoria dentro del último año
    SET @appointmentDateTime = DATEADD(day, -ROUND(RAND() * 365, 0), GETDATE());

    -- Asigna un estado aleatorio
    SET @randomValue = ROUND(RAND() * 2, 0);
    SET @status = CASE @randomValue
        WHEN 0 THEN 'scheduled'
        WHEN 1 THEN 'done'
        ELSE 'cancelled'
    END;

    INSERT INTO Appointments (CustomerId, DateTime, Status)
    VALUES (@customerId, @appointmentDateTime, @status);

    SET @j = @j + 1;
END;

PRINT 'Appointments data insertion complete.';
PRINT 'Script finished successfully.';
GO
