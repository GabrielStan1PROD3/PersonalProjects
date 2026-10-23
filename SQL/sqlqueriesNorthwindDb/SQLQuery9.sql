SELECT CONCAT(emp.FirstName , ' ', emp.LastName) as 'Name',
		COUNT (DISTINCT p.CategoryId) as 'categories'
					FROM Employees emp 
					INNER JOIN  ORDERS o
					ON emp.EmployeeId = o.EmployeeId
					INNER JOIN [Order Details] od 
					ON o.OrderID  = od.OrderId 
					INNER JOIN 
					Products p 
					ON od.ProductId = p.ProductId 
					GROUP BY CONCAT(emp.FirstName , ' ', emp.LastName)
					HAVING
					COUNT (DISTINCT p.CategoryId) = 1


-- Returns employees who have sold products from exactly one distinct category.
-- Joins Employees → Orders → Order Details → Products to determine which categories each employee has sold.
-- Groups by employee full name and counts DISTINCT CategoryId values.
-- The HAVING clause filters employees whose sales involve only a single category.