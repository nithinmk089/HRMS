
-- === 3. FUNCTIONS ===
CREATE FUNCTION hr.fn_GetEmployeeAge
(
    @DateOfBirth DATE
)
RETURNS INT
AS
BEGIN
    DECLARE @Age INT;
    IF @DateOfBirth IS NULL RETURN NULL;
    SET @Age = DATEDIFF(year, @DateOfBirth, GETUTCDATE()) - 
               CASE WHEN (MONTH(@DateOfBirth) > MONTH(GETUTCDATE())) OR 
                         (MONTH(@DateOfBirth) = MONTH(GETUTCDATE()) AND DAY(@DateOfBirth) > DAY(GETUTCDATE())) 
                    THEN 1 ELSE 0 END;
    RETURN @Age;
END;