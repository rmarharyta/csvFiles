USE csvUpload;
GO 

INSERT INTO Users (Email, PasswordHash)
VALUES 
('rita@example.com', 'hash123'),
('john@example.com', 'hash456'),
('anna@example.com', 'hash789');

INSERT INTO CsvFiles (UserId, FileName)
VALUES
((SELECT TOP 1 UserId FROM Users WHERE Email='rita@example.com'), 'employees.csv'),
((SELECT TOP 1 UserId FROM Users WHERE Email='rita@example.com'), 'clients.csv'),
((SELECT TOP 1 UserId FROM Users WHERE Email='john@example.com'), 'sales.csv'),
((SELECT TOP 1 UserId FROM Users WHERE Email='anna@example.com'), 'products.csv');

INSERT INTO CsvRecords (FileId, Name, BirthDate, Married, Phone, Salary)
VALUES
((SELECT TOP 1 FileId FROM CsvFiles WHERE FileName='employees.csv'), 'John Doe', '1985-03-12', 1, '123-456-7890', 50000.50),
((SELECT TOP 1 FileId FROM CsvFiles WHERE FileName='employees.csv'), 'Jane Smith', '1990-07-25', 0, '987-654-3210', 60000.00),
((SELECT TOP 1 FileId FROM CsvFiles WHERE FileName='clients.csv'), 'Client A', '1975-11-03', 1, '555-111-2222', 0),
((SELECT TOP 1 FileId FROM CsvFiles WHERE FileName='clients.csv'), 'Client B', '1980-05-18', 0, '555-333-4444', 0),
((SELECT TOP 1 FileId FROM CsvFiles WHERE FileName='sales.csv'), 'Sale 1', '1992-08-12', 0, '000-111-2222', 1200.00),
((SELECT TOP 1 FileId FROM CsvFiles WHERE FileName='products.csv'), 'Product X', '2000-01-01', 0, '000-000-0000', 0);

