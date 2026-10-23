SELECT 
    c.CustomerId,
    c.Country,
    SUM(od.UnitPrice * od.Quantity) AS Dollars
       FROM Customers c
        INNER JOIN Orders o 
        ON c.CustomerId = o.CustomerId
        INNER JOIN [Order Details] od 
         ON od.OrderId = o.OrderId
        GROUP BY c.CustomerId, c.Country
HAVING SUM(od.UnitPrice * od.Quantity) >=
(
                        SELECT AVG(PerCountry)
                       FROM (
                        SELECT 
                    cou.Country,
                    SUM(od2.UnitPrice * od2.Quantity) AS PerCountry
                    FROM Customers cou
                     INNER JOIN Orders o2 
                    ON cou.CustomerId = o2.CustomerId
                    INNER JOIN [Order Details] od2 
                    ON od2.OrderId = o2.OrderId
                    GROUP BY cou.Country
                    ) AS x
                     WHERE x.Country = c.Country
   );

												 
