DROP DATABASE AutoDB;
GO

CREATE DATABASE AutoDB;
GO


USE AutoDB;
GO

CREATE TABLE Roles (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Role NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL
);
GO


CREATE TABLE Accounts (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Login NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    Role_ID INT FOREIGN KEY REFERENCES Roles(ID)
);
GO

CREATE TABLE Customers (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Firstname NVARCHAR(50) NOT NULL,
    Surname NVARCHAR(50) NOT NULL,
    Patronymic NVARCHAR(100),
    Phone NVARCHAR(20) UNIQUE NOT NULL,
    Account_ID INT FOREIGN KEY REFERENCES Accounts(ID)
);
GO

CREATE TABLE Employees (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Firstname NVARCHAR(50) NOT NULL,
    Surname NVARCHAR(50) NOT NULL,
    Patronymic NVARCHAR(100),
    Post NVARCHAR(50) NOT NULL,
    Account_ID INT FOREIGN KEY REFERENCES Accounts(ID)
);
GO

CREATE TABLE PaymentMethods (
    ID INT PRIMARY KEY IDENTITY(1,1),
    PaymentMethod NVARCHAR(50) UNIQUE NOT NULL
);
GO

CREATE TABLE CarStatus (
    ID INT PRIMARY KEY IDENTITY(1,1),
    CarStatus NVARCHAR(50) UNIQUE NOT NULL
);
GO

CREATE TABLE CarCountries (
    ID INT PRIMARY KEY IDENTITY(1,1),
    CarCountry NVARCHAR(50)
);
GO

CREATE TABLE CarModels (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Brand NVARCHAR(50) NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    Year INT NOT NULL,
    Country_ID INT FOREIGN KEY REFERENCES CarCountries(ID),
    Status_ID INT FOREIGN KEY REFERENCES CarStatus(ID)
);
GO

CREATE TABLE Cars (
    ID INT PRIMARY KEY IDENTITY(1,1),
    CarModel_ID INT FOREIGN KEY REFERENCES CarModels(ID),
    Number NVARCHAR(17) NOT NULL,
    Mileage INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Condition NVARCHAR(20) NOT NULL,
    Color NVARCHAR(20) NOT NULL,
    Amount INT NOT NULL
);
GO

CREATE TABLE OrderCheck (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Customer_ID INT FOREIGN KEY REFERENCES Customers(ID),
    Employee_ID INT FOREIGN KEY REFERENCES Employees(ID),
    OrderDate DATE NOT NULL,
    OrderTime TIME NOT NULL,
    PaymentMethod_ID INT FOREIGN KEY REFERENCES PaymentMethods(ID),
    PaidAmount DECIMAL(10,2) NOT NULL
);
GO

CREATE TABLE OrderCar (
    ID INT PRIMARY KEY IDENTITY(1,1),
    OrderCheck_ID INT FOREIGN KEY REFERENCES OrderCheck(ID),
    Car_ID INT FOREIGN KEY REFERENCES Cars(ID),
    Amount INT NOT NULL
);
GO

INSERT INTO Roles (Role, Status) VALUES 
('Администратор', 'Активен'),
('Пользователь', 'Активен');
GO

INSERT INTO PaymentMethods (PaymentMethod) VALUES
('Наличные'),
('Банковская карта'),
('Онлайн перевод'),
('Кредит'),
('Рассрочка');
GO

INSERT INTO Accounts (Login, Password, Role_ID) VALUES 
('admin', 'admin123', 1),
('user', 'user123', 2);
GO

INSERT INTO Customers (Firstname, Surname, Patronymic, Phone, Account_ID) VALUES 
('Иван', 'Петров', 'Иванович', '+7-999-123-45-67', 2);
GO

INSERT INTO Employees (Firstname, Surname, Patronymic, Post, Account_ID) VALUES 
('Алексей', 'Сидоров', 'Владимирович', 'Manager', 1);
GO

INSERT INTO CarStatus (CarStatus) VALUES ('Продано'), ('В продаже'), ('Ожидает поставки');
GO

INSERT INTO CarCountries (CarCountry) VALUES ('Россия'), ('Германия'), ('Япония');
GO


INSERT INTO CarModels (Brand, Name, Year, Country_ID, Status_ID) VALUES
('Lada', 'Vesta', 2024, 1, 2),
('Lada', 'Granta', 2023, 1, 2),
('Lada', 'Niva Travel', 2024, 1, 2),
('Lada', 'Largus', 2023, 1, 3),
('BMW', 'X5', 2024, 2, 2),
('BMW', 'X6', 2023, 2, 2),
('BMW', '3 Series', 2024, 2, 3),
('Mercedes-Benz', 'E-Class', 2024, 2, 2),
('Mercedes-Benz', 'S-Class', 2023, 2, 1),
('Audi', 'Q7', 2024, 2, 2),
('Audi', 'A6', 2023, 2, 3),
('Toyota', 'Camry', 2024, 3, 2),
('Toyota', 'RAV4', 2023, 3, 2),
('Toyota', 'Land Cruiser Prado', 2024, 3, 3),
('Lexus', 'RX', 2024, 3, 2),
('Lexus', 'LX', 2023, 3, 1),
('Honda', 'Civic', 2024, 3, 2),
('Honda', 'CR-V', 2023, 3, 2),
('Nissan', 'X-Trail', 2024, 3, 3),
('Nissan', 'Qashqai', 2023, 3, 2);
GO

INSERT INTO Cars (CarModel_ID, Number, Mileage, Price, Condition, Color, Amount) VALUES
(1, 'A123BC777', 0, 1200000.00, 'Новый', 'Белый', 5),
(1, 'B456PE777', 15000, 1050000.00, 'Отличное', 'Черный', 2),
(1, 'C789HT777', 25000, 950000.00, 'Хорошее', 'Серебристый', 1),
(2, 'E159KM777', 0, 850000.00, 'Новый', 'Синий', 7),
(2, 'F753AC777', 35000, 720000.00, 'Хорошее', 'Красный', 3),
(3, 'G951EH777', 0, 1600000.00, 'Новый', 'Зеленый', 4),
(3, 'H482OX777', 10000, 1450000.00, 'Отличное', 'Коричневый', 2),
(5, 'K753AB777', 0, 8500000.00, 'Новый', 'Черный', 3),
(5, 'M159EC777', 22000, 7200000.00, 'Отличное', 'Белый', 1),
(6, 'N456PK777', 0, 9200000.00, 'Новый', 'Красный', 2),
(8, 'P789EH777', 18000, 6800000.00, 'Отличное', 'Серебристый', 2),
(12, 'R951KO777', 0, 3200000.00, 'Новый', 'Белый', 6),
(12, 'S753HP777', 12000, 2900000.00, 'Отличное', 'Черный', 3),
(13, 'T357PH777', 0, 3500000.00, 'Новый', 'Синий', 4),
(13, 'U159CB777', 28000, 3000000.00, 'Хорошее', 'Серый', 2),
(15, 'V753KO777', 0, 6800000.00, 'Новый', 'Белый', 2),
(19, 'X951PE777', 8000, 2800000.00, 'Отличное', 'Черный', 3),
(20, 'Y357AC777', 0, 2500000.00, 'Новый', 'Красный', 5);
GO


INSERT INTO OrderCheck (Customer_ID, Employee_ID, OrderDate, OrderTime, PaymentMethod_ID, PaidAmount) VALUES
(1, 1, '2025-01-15', '12:30:00', 2, 1200000.00),
(1, 1, '2025-01-20', '15:45:00', 1, 850000.00),
(1, 1, '2025-01-25', '10:15:00', 3, 3200000.00);
GO

INSERT INTO OrderCar (OrderCheck_ID, Car_ID, Amount) VALUES
(1, 1, 1),
(2, 4, 2),
(3, 12, 1);
GO
