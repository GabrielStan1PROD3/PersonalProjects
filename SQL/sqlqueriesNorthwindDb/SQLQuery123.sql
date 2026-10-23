USE Northwind


SELECT  c.CompanyName ,
COUNT(DISTINCT cat.CategoryId) as 'Category_Count'
	FROM CUSTOMERS c
	INNER JOIN Orders o
		ON c.CustomerId = o.CustomerId 
	INNER JOIN [Order Details] od 
		ON o.OrderId = od.OrderId
	INNER JOIN Products p 
		ON od.ProductId = p.ProductId
	INNER JOIN Categories cat
		ON p.CategoryId = cat.CategoryId
			GROUP BY c.CompanyName
			HAVING  COUNT(DISTINCT cat.CategoryId) >5
			ORDER BY Category_Count DESC
GO
