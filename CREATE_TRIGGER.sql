CREATE TRIGGER Payments_INSERT
ON Payments
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @OrderID int = (SELECT OrderID FROM inserted)
	DECLARE @ArrivalID int = (SELECT ArrivalID FROM inserted)
	DECLARE @Amount int = (SELECT Amount FROM inserted)
	--@Remains – сколько остаток в платеже
	DECLARE @Remains int = (SELECT Remains
	FROM Arrivals
	WHERE IDArrival = @ArrivalID)

    --если в выбранном поступлении остаток меньше того, что мы хотим списать, 
	--или если сумма нового начисления в заказ больше суммы заказа, то отменяем транзакцию
	IF @Remains < @Amount
	BEGIN
		ROLLBACK TRANSACTION;
		THROW 50001, 'Остаток на счету меньше списываемой суммы', 1;
	END

	ELSE IF @Amount + (SELECT PaymentAmount FROM Orders
					WHERE IDOrder = @OrderID) > (SELECT Payment FROM Orders
													WHERE IDOrder = @OrderID)
	BEGIN
		ROLLBACK TRANSACTION;
		THROW 50001, 'Начисление больше суммы заказа', 1;
	END
	ELSE 
	BEGIN
	UPDATE Arrivals
	SET Remains = Remains - @Amount
	WHERE IDArrival = @ArrivalID

	UPDATE Orders
	SET PaymentAmount = PaymentAmount + @Amount
	WHERE IDOrder = @OrderID
	
	INSERT INTO Payments (OrderID, ArrivalID, Amount) VALUES (@OrderID, @ArrivalID, @Amount)
	END
END
GO