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
    Brand NVARCHAR(50) UNIQUE NOT NULL,
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
('Admin', 'Active'),
('User', 'Active');
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

INSERT INTO CarStatus (CarStatus) VALUES ('Продано'), ('В продаже'), ('На заказ');
GO

INSERT INTO CarCountries (CarCountry) VALUES ('Россия'), ('Германия'), ('Япония');
GO