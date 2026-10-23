SELECT cust.CompanyName, Count(DISTINCT p.CategoryId) FROM  Customers cust
INNER JOIN ORDERS o
ON cust.CustomerId = o.CustomerId
INNER JOIN 
[Order Details] od 
ON o.OrderId = od.OrderId
INNER JOIN Products p 
ON od.ProductId = p.ProductId 
GROUP BY cust.CompanyName 
Having 
COUNT (DISTINCT p.CategoryId) = (SELECT COUNT(CategoryId) FROM Categories)


-- Selects customers who have ordered products from all existing categories.
-- Joins Customers → Orders → Order Details → Products to determine which categories each customer has purchased from.
-- Groups results by customer and counts the DISTINCT categories they have ordered.
-- The HAVING clause compares this count with the total number of categories in the Categories table.
-- Final result: only customers who have purchased at least one product from every category.