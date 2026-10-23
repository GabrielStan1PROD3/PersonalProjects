USE Northwind


--SELECT Country , COUNT(CompanyName) FROM Customers
--GROUP BY Country

SELECT 
    c.Country,
    COUNT(DISTINCT c.CustomerID) AS CustomersWithOrders,
    (SELECT COUNT(*) 
     FROM Customers c2 
     WHERE c2.Country = c.Country) AS TotalCustomers
FROM Customers c
JOIN Orders o ON o.CustomerID = c.CustomerID
GROUP BY c.Country
HAVING COUNT(DISTINCT c.CustomerID) =
       (SELECT COUNT(*) 
        FROM Customers c2 
        WHERE c2.Country = c.Country);
