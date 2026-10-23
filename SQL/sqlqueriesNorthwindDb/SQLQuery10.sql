SELECT c.CompanyName ,
COUNT (DISTINCT YEAR(o.ShippedDate)) as 'DISTINCT YEARS'
FROM Customers c
INNER JOIN Orders o 
on C.CustomerId = o.CustomerId
GROUP BY CompanyName 
HAVING 
COUNT (DISTINCT YEAR(o.ShippedDate)) = (SELECT COUNT (DISTINCT YEAR(shippeddate))FROM Orders)


-- Returns customers who have placed orders in every distinct year present in the Orders table.
-- Joins Customers → Orders to determine the years in which each customer has shipped orders.
-- Groups by customer name and counts DISTINCT years extracted from ShippedDate.
-- The HAVING clause compares this count with the total number of distinct years across all orders.
-- Final result: only customers who have shipped orders in all years where the company has recorded orders.