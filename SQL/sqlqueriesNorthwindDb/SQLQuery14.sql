



/*Display the employees whose total sales (calculated as the sum of UnitPrice × Quantity across all their orders) are higher than the average total sales of all employees.
Use JOINs between Employees, Orders, and Order Details, group the results by employee, and use a subquery in the HAVING clause to compute the global average.
*/

SELECT e.FirstName ,e.LastName ,
Sum(od.UnitPrice *od.Quantity) as "Total",
Count(o.EmployeeId) "Total sales"
	FROM Employees e INNER JOIN Orders o 
	ON e.EmployeeId = o.EmployeeId
	INNER JOIN 
	[Order Details] od ON
	o.OrderId = od.OrderId
	GROUP BY 
	e.FirstName ,e.LastName
	Having 
	Sum(od.UnitPrice *od.Quantity) >(SELECT avg(Sales) 
													FROM (SELECT emp.EmployeeId ,
															SUM(ord.UnitPrice * ord.Quantity) as Sales FROM 
															Employees emp
															INNER JOIN 
															ORDERS o ON emp.EmployeeId = o.EmployeeId
															INNER JOIN [Order Details] ord 
															ON ord.OrderId = o.OrderId 
															GROUP BY emp.EmployeeId) as x);