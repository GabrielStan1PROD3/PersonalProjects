SELECT od.ProductId ,od2.ProductID , COUNT(*) as 'TIMES_ORDERED_TOGGETHER'
FROM [Order Details] od INNER JOIN [Order Details] od2 
ON 
od.ProductId < od2.ProductId AND od.OrderID = od2.OrderID
GROUP BY 
od.ProductId , od2.ProductiD
Having COUNT(*) >5
ORDER BY TIMES_ORDERED_TOGGETHER DESC;
