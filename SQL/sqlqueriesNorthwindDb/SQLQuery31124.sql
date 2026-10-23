USE Northwind


SELECT  p.ProductName,
COUNT(DISTINCT MONTH(o.shippedDate)) 'Number_Of_Months'
FROM Products p INNER JOIN [Order Details] od 
	on p.ProductId = od.ProductId
	INNER JOIN Orders o 
	on o.OrderId = od.OrderId
		WHERE o.ShippedDate 
					IS NOT NULL
					AND YEAR(o.ShippedDate) = 1997
	GROUP BY p.ProductName
	HAVING 
	COUNT(DISTINCT MONTH(o.shippedDate)) = 12