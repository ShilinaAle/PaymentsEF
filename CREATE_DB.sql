CREATE DATABASE PaymentsDB
GO
USE PaymentsDB
GO

CREATE TABLE Orders
(
    IDOrder int IDENTITY PRIMARY KEY,
    OrderDate datetime NOT NULL,
	Payment money NOT NULL,
	PaymentAmount money DEFAULT 0,
);
GO
CREATE TABLE Arrivals
(
    IDArrival int IDENTITY PRIMARY KEY,
    ArrivalDate datetime NOT NULL,
	SumOfArrival money NOT NULL,
	Remains money DEFAULT 0,
);
GO
CREATE TABLE Payments
(
    OrderID int NOT NULL,
    ArrivalID int NOT NULL,
    Amount money DEFAULT 0,
	FOREIGN KEY (OrderID) REFERENCES Orders (IDOrder),
	FOREIGN KEY (ArrivalID) REFERENCES Arrivals (IDArrival)
);
GO
INSERT INTO Arrivals (ArrivalDate, SumOfArrival) VALUES(GETDATE(), 50);
INSERT INTO Arrivals (ArrivalDate, SumOfArrival) VALUES(GETDATE(), 30);
INSERT INTO Arrivals (ArrivalDate, SumOfArrival) VALUES(GETDATE(), 40);
INSERT INTO Orders (OrderDate, Payment) VALUES(GETDATE(), 30);
INSERT INTO Orders (OrderDate, Payment) VALUES(GETDATE(), 70);
INSERT INTO Orders (OrderDate, Payment) VALUES(GETDATE(), 10);


