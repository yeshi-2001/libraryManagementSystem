USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'LibraryDB')
    CREATE DATABASE LibraryDB;
GO
--server mngment commnd(execute)
USE LibraryDB;
GO

-- ============================================
-- Tables
-- ============================================

CREATE TABLE Users (
    UserId      INT IDENTITY(1,1) PRIMARY KEY,
    Username    NVARCHAR(50)    NOT NULL UNIQUE,
    Password    NVARCHAR(256)   NOT NULL,
    FullName    NVARCHAR(100)   NOT NULL,
    Role        NVARCHAR(20)    NOT NULL DEFAULT 'Staff',
    CreatedDate DATETIME        NOT NULL DEFAULT GETDATE(),
    IsActive    BIT             NOT NULL DEFAULT 1
);
GO

CREATE TABLE Members (
    MemberId        INT IDENTITY(1,1) PRIMARY KEY,
    FullName        NVARCHAR(100)   NOT NULL,
    NIC             NVARCHAR(20)    NOT NULL UNIQUE,
    Email           NVARCHAR(100)   NULL,
    Phone           NVARCHAR(20)    NULL,
    Address         NVARCHAR(250)   NULL,
    RegisteredDate  DATETIME        NOT NULL DEFAULT GETDATE(),
    Status          NVARCHAR(20)    NOT NULL DEFAULT 'Active'
);
GO

CREATE TABLE Books (
    BookId              INT IDENTITY(1,1) PRIMARY KEY,
    ISBN                NVARCHAR(20)    NOT NULL UNIQUE,
    Title               NVARCHAR(200)   NOT NULL,
    Author              NVARCHAR(150)   NOT NULL,
    Category            NVARCHAR(80)    NULL,
    Publisher           NVARCHAR(120)   NULL,
    PublishYear         INT             NULL,
    Quantity            INT             NOT NULL DEFAULT 1,
    AvailableQuantity   INT             NOT NULL DEFAULT 1,
    ShelfLocation       NVARCHAR(50)    NULL,
    Status              NVARCHAR(20)    NOT NULL DEFAULT 'Active',
    CreatedDate         DATETIME        NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE BorrowRecords (
    BorrowId    INT IDENTITY(1,1) PRIMARY KEY,
    BookId      INT             NOT NULL REFERENCES Books(BookId),
    MemberId    INT             NOT NULL REFERENCES Members(MemberId),
    BorrowDate  DATETIME        NOT NULL DEFAULT GETDATE(),
    DueDate     DATETIME        NOT NULL,
    ReturnDate  DATETIME        NULL,
    Fine        DECIMAL(10,2)   NOT NULL DEFAULT 0,
    Status      NVARCHAR(20)    NOT NULL DEFAULT 'Borrowed'
);
GO

-- ============================================
-- User Stored Procedures
-- ============================================

CREATE PROCEDURE sp_User_GetAll
AS
    SELECT UserId, Username, Password, FullName, Role, CreatedDate, IsActive FROM Users;
GO

CREATE PROCEDURE sp_User_GetById @UserId INT
AS
    SELECT UserId, Username, Password, FullName, Role, CreatedDate, IsActive FROM Users WHERE UserId = @UserId;
GO

CREATE PROCEDURE sp_User_GetByUsername @Username NVARCHAR(50)
AS
    SELECT UserId, Username, Password, FullName, Role, CreatedDate, IsActive FROM Users WHERE Username = @Username;
GO

CREATE PROCEDURE sp_User_Insert
    @Username NVARCHAR(50), @Password NVARCHAR(256), @FullName NVARCHAR(100), @Role NVARCHAR(20)
AS
    INSERT INTO Users (Username, Password, FullName, Role)
    VALUES (@Username, @Password, @FullName, @Role);
    SELECT SCOPE_IDENTITY();
GO

CREATE PROCEDURE sp_User_Update
    @UserId INT, @FullName NVARCHAR(100), @Role NVARCHAR(20), @IsActive BIT
AS
    UPDATE Users SET FullName = @FullName, Role = @Role, IsActive = @IsActive WHERE UserId = @UserId;
GO

CREATE PROCEDURE sp_User_UpdatePassword
    @UserId INT, @Password NVARCHAR(256)
AS
    UPDATE Users SET Password = @Password WHERE UserId = @UserId;
GO

CREATE PROCEDURE sp_User_Delete @UserId INT
AS
    DELETE FROM Users WHERE UserId = @UserId;
GO

CREATE PROCEDURE sp_User_Search
    @SearchTerm NVARCHAR(100), @PageNumber INT, @PageSize INT
AS
    SELECT UserId, Username, Password, FullName, Role, CreatedDate, IsActive,
           COUNT(*) OVER() AS TotalCount
    FROM Users
    WHERE Username LIKE '%' + @SearchTerm + '%' OR FullName LIKE '%' + @SearchTerm + '%'
    ORDER BY UserId
    OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
GO

-- ============================================
-- Member Stored Procedures
-- ============================================

CREATE PROCEDURE sp_Member_GetAll
AS
    SELECT MemberId, FullName, NIC, Email, Phone, Address, RegisteredDate, Status FROM Members;
GO

CREATE PROCEDURE sp_Member_GetById @MemberId INT
AS
    SELECT MemberId, FullName, NIC, Email, Phone, Address, RegisteredDate, Status FROM Members WHERE MemberId = @MemberId;
GO

CREATE PROCEDURE sp_Member_Insert
    @FullName NVARCHAR(100), @NIC NVARCHAR(20), @Email NVARCHAR(100),
    @Phone NVARCHAR(20), @Address NVARCHAR(250), @Status NVARCHAR(20)
AS
    INSERT INTO Members (FullName, NIC, Email, Phone, Address, Status)
    VALUES (@FullName, @NIC, @Email, @Phone, @Address, @Status);
    SELECT SCOPE_IDENTITY();
GO

CREATE PROCEDURE sp_Member_Update
    @MemberId INT, @FullName NVARCHAR(100), @NIC NVARCHAR(20), @Email NVARCHAR(100),
    @Phone NVARCHAR(20), @Address NVARCHAR(250), @Status NVARCHAR(20)
AS
    UPDATE Members SET FullName=@FullName, NIC=@NIC, Email=@Email,
        Phone=@Phone, Address=@Address, Status=@Status WHERE MemberId=@MemberId;
GO

CREATE PROCEDURE sp_Member_Delete @MemberId INT
AS
    IF EXISTS (SELECT 1 FROM BorrowRecords WHERE MemberId = @MemberId)
        SELECT 0;
    ELSE
    BEGIN
        DELETE FROM Members WHERE MemberId = @MemberId;
        SELECT 1;
    END
GO

CREATE PROCEDURE sp_Member_Search
    @SearchTerm NVARCHAR(100), @PageNumber INT, @PageSize INT
AS
    SELECT MemberId, FullName, NIC, Email, Phone, Address, RegisteredDate, Status,
           COUNT(*) OVER() AS TotalCount
    FROM Members
    WHERE FullName LIKE '%' + @SearchTerm + '%' OR NIC LIKE '%' + @SearchTerm + '%'
    ORDER BY MemberId
    OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
GO

-- ============================================
-- Book Stored Procedures
-- ============================================

CREATE PROCEDURE sp_Book_GetAll
AS
    SELECT BookId, ISBN, Title, Author, Category, Publisher, PublishYear,
           Quantity, AvailableQuantity, ShelfLocation, Status, CreatedDate FROM Books;
GO

CREATE PROCEDURE sp_Book_GetById @BookId INT
AS
    SELECT BookId, ISBN, Title, Author, Category, Publisher, PublishYear,
           Quantity, AvailableQuantity, ShelfLocation, Status, CreatedDate FROM Books WHERE BookId = @BookId;
GO

CREATE PROCEDURE sp_Book_Insert
    @ISBN NVARCHAR(20), @Title NVARCHAR(200), @Author NVARCHAR(150), @Category NVARCHAR(80),
    @Publisher NVARCHAR(120), @PublishYear INT, @Quantity INT, @ShelfLocation NVARCHAR(50), @Status NVARCHAR(20)
AS
    INSERT INTO Books (ISBN, Title, Author, Category, Publisher, PublishYear, Quantity, AvailableQuantity, ShelfLocation, Status)
    VALUES (@ISBN, @Title, @Author, @Category, @Publisher, @PublishYear, @Quantity, @Quantity, @ShelfLocation, @Status);
    SELECT SCOPE_IDENTITY();
GO

CREATE PROCEDURE sp_Book_Update
    @BookId INT, @ISBN NVARCHAR(20), @Title NVARCHAR(200), @Author NVARCHAR(150), @Category NVARCHAR(80),
    @Publisher NVARCHAR(120), @PublishYear INT, @Quantity INT, @ShelfLocation NVARCHAR(50), @Status NVARCHAR(20)
AS
    UPDATE Books SET ISBN=@ISBN, Title=@Title, Author=@Author, Category=@Category,
        Publisher=@Publisher, PublishYear=@PublishYear, Quantity=@Quantity,
        ShelfLocation=@ShelfLocation, Status=@Status WHERE BookId=@BookId;
GO

CREATE PROCEDURE sp_Book_Delete @BookId INT
AS
    IF EXISTS (SELECT 1 FROM BorrowRecords WHERE BookId = @BookId)
        SELECT 0;
    ELSE
    BEGIN
        DELETE FROM Books WHERE BookId = @BookId;
        SELECT 1;
    END
GO

CREATE PROCEDURE sp_Book_Search
    @SearchTerm NVARCHAR(100), @PageNumber INT, @PageSize INT
AS
    SELECT BookId, ISBN, Title, Author, Category, Publisher, PublishYear,
           Quantity, AvailableQuantity, ShelfLocation, Status, CreatedDate,
           COUNT(*) OVER() AS TotalCount
    FROM Books
    WHERE Title LIKE '%' + @SearchTerm + '%' OR ISBN LIKE '%' + @SearchTerm + '%'
       OR Author LIKE '%' + @SearchTerm + '%'
    ORDER BY BookId
    OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
GO

CREATE PROCEDURE sp_Book_DecrementAvailable @BookId INT
AS
    UPDATE Books SET AvailableQuantity = AvailableQuantity - 1
    WHERE BookId = @BookId AND AvailableQuantity > 0;
    SELECT @@ROWCOUNT;
GO

CREATE PROCEDURE sp_Book_IncrementAvailable @BookId INT
AS
    UPDATE Books SET AvailableQuantity = AvailableQuantity + 1
    WHERE BookId = @BookId AND AvailableQuantity < Quantity;
GO

-- ============================================
-- BorrowRecord Stored Procedures
-- ============================================

CREATE PROCEDURE sp_Borrow_GetAll
AS
    SELECT br.BorrowId, br.BookId, br.MemberId, br.BorrowDate, br.DueDate,
           br.ReturnDate, br.Fine, br.Status,
           b.Title AS BookTitle, b.ISBN, m.FullName AS MemberName, m.NIC
    FROM BorrowRecords br
    JOIN Books b ON br.BookId = b.BookId
    JOIN Members m ON br.MemberId = m.MemberId;
GO

CREATE PROCEDURE sp_Borrow_GetById @BorrowId INT
AS
    SELECT br.BorrowId, br.BookId, br.MemberId, br.BorrowDate, br.DueDate,
           br.ReturnDate, br.Fine, br.Status,
           b.Title AS BookTitle, b.ISBN, m.FullName AS MemberName, m.NIC
    FROM BorrowRecords br
    JOIN Books b ON br.BookId = b.BookId
    JOIN Members m ON br.MemberId = m.MemberId
    WHERE br.BorrowId = @BorrowId;
GO

CREATE PROCEDURE sp_Borrow_Insert
    @BookId INT, @MemberId INT, @DueDate DATETIME
AS
    INSERT INTO BorrowRecords (BookId, MemberId, DueDate)
    VALUES (@BookId, @MemberId, @DueDate);
    SELECT SCOPE_IDENTITY();
GO

CREATE PROCEDURE sp_Borrow_Update
    @BorrowId INT, @ReturnDate DATETIME, @Fine DECIMAL(10,2), @Status NVARCHAR(20)
AS
    UPDATE BorrowRecords SET ReturnDate=@ReturnDate, Fine=@Fine, Status=@Status WHERE BorrowId=@BorrowId;
GO

CREATE PROCEDURE sp_Borrow_Delete @BorrowId INT
AS
    IF EXISTS (SELECT 1 FROM BorrowRecords WHERE BorrowId = @BorrowId AND Status = 'Returned')
    BEGIN
        DELETE FROM BorrowRecords WHERE BorrowId = @BorrowId;
        SELECT @@ROWCOUNT;
    END
    ELSE
        SELECT 0;
GO

CREATE PROCEDURE sp_Borrow_Search
    @SearchTerm NVARCHAR(100), @Status NVARCHAR(20), @PageNumber INT, @PageSize INT
AS
    SELECT br.BorrowId, br.BookId, br.MemberId, br.BorrowDate, br.DueDate,
           br.ReturnDate, br.Fine, br.Status,
           b.Title AS BookTitle, b.ISBN, m.FullName AS MemberName, m.NIC,
           COUNT(*) OVER() AS TotalCount
    FROM BorrowRecords br
    JOIN Books b ON br.BookId = b.BookId
    JOIN Members m ON br.MemberId = m.MemberId
    WHERE (@Status = '' OR br.Status = @Status)
      AND (b.Title LIKE '%' + @SearchTerm + '%' OR m.FullName LIKE '%' + @SearchTerm + '%'
           OR m.NIC LIKE '%' + @SearchTerm + '%')
    ORDER BY br.BorrowId DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
GO

-- ============================================
-- Default Admin User (password: Admin@123)
-- ============================================
INSERT INTO Users (Username, Password, FullName, Role)
VALUES ('admin', '10000.wbmsd5YGPRs5p3Ld2kcFhg==.SnX7k3QwPvLmN8Rz1TqYeJdUoAiCbHgW4sXnKpMvZtE=', 'Administrator', 'Admin');
GO