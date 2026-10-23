SELECT p.ProductName , Sum(o.UnitPrice * o.Quantity) FROM Products p 
INNER JOIN [Order Details] o ON p.ProductId = o.ProductId
GROUP by p.ProductName 
Having  Sum(o.UnitPrice * o.Quantity) >  (SELECT AVG(UnitPrice * Quantity) FROM [Order Details]);
