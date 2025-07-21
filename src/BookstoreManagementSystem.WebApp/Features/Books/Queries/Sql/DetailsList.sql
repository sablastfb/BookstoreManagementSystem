SELECT b."Title",                       
       b."Price",                          
       COALESCE(avg(r."Rating"), 0)        as AverageRating,
       STRING_AGG(DISTINCT a."Name", '| ') as AuthorNames,
       STRING_AGG(DISTINCT g."Name", '| ') as GenreNames
FROM "Books" b
         INNER JOIN "BookAuthors" ba ON b."Id" = ba."BookId"
         INNER JOIN "Authors" a ON a."Id" = ba."AuthorId"
         INNER JOIN "BookGenres" bg ON b."Id" = bg."BookId"
         INNER JOIN "Genres" g ON bg."GenreId" = g."Id"
         LEFT JOIN "Reviews" r ON b."Id" = r."BookId"
GROUP BY b."Id"
ORDER BY AverageRating DESC
LIMIT @Limit OFFSET @Offset;