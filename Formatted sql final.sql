
-- Create the Booking table
CREATE TABLE [dbo].[Booking] (
    [BookingID]   INT      IDENTITY (1, 1) NOT NULL, -- Primary Key, auto-incrementing
    [RoomNumber]  INT      NULL, -- Foreign Key to Rooms table
    [GuestID]     INT      NULL, -- Foreign Key to Guests table
    [BookingDate] DATETIME NULL, -- Date of booking
    [TotalPrice]  MONEY    NULL, -- Total price of the booking
    PRIMARY KEY CLUSTERED ([BookingID] ASC), -- Primary Key constraint
    CONSTRAINT [Book_unique] UNIQUE NONCLUSTERED ([RoomNumber] ASC), -- Unique constraint on RoomNumber
    CONSTRAINT [Book_unique2] UNIQUE NONCLUSTERED ([RoomNumber] ASC, [GuestID] ASC), -- Unique constraint on RoomNumber and GuestID
    FOREIGN KEY ([RoomNumber]) REFERENCES [dbo].[Rooms] ([RoomNumber]), -- Foreign Key constraint to Rooms table
    FOREIGN KEY ([GuestID]) REFERENCES [dbo].[Guests] ([GuestID]) -- Foreign Key constraint to Guests table
);
GO

-- Create the trigger RoomStatchange to update room and guest status after a booking is inserted
CREATE TRIGGER RoomStatchange
ON Booking
AFTER INSERT 
AS
BEGIN
    DECLARE @roomnum INT;
    SELECT @roomnum = RoomNumber FROM inserted;

    -- Update the room status to 'occupied'
    UPDATE Rooms SET Status = 'occupied'
    WHERE RoomNumber = @roomnum;

    -- Update the guest status to 'checked in'
    UPDATE Guests SET Status = 'checked in' 
    WHERE RoomNumber = @roomnum;
END;
GO

-- Create the trigger RoomStateChange2 to update room and guest status after a booking is deleted
CREATE TRIGGER RoomStateChange2
ON Booking
AFTER DELETE
AS
BEGIN
    DECLARE @roomnum INT;
    SELECT @roomnum = RoomNumber FROM deleted;

    -- Update the room status to 'available'
    UPDATE Rooms SET Status = 'available'
    WHERE RoomNumber = @roomnum;

    -- Update the guest status to 'checked out'
    UPDATE Guests SET Status = 'checked out' 
    WHERE RoomNumber = @roomnum;
END;
GO

-- Create the Guests table
CREATE TABLE [dbo].[Guests] (
    [GuestID]        INT          IDENTITY (1, 1) NOT NULL, -- Primary Key, auto-incrementing
    [GuestFirstName] VARCHAR (15) NOT NULL, -- Guest's first name
    [GuestLastName]  VARCHAR (15) NULL, -- Guest's last name
    [GuestPhone]     VARCHAR (13) NULL, -- Guest's phone number
    [CheckInDate]    DATETIME     NULL, -- Check-in date
    [CheckOutDate]   DATETIME     NULL, -- Check-out date
    [RoomNumber]     INT          NULL, -- Foreign Key to Rooms table
    [Status]         VARCHAR (15) NULL, -- Status of the guest (pending, checked in, checked out)
    [GuestDOB]       DATE         NULL, -- Guest's date of birth
    [GuestGender]    VARCHAR (6)  NULL, -- Guest's gender
    PRIMARY KEY CLUSTERED ([GuestID] ASC), -- Primary Key constraint
    CONSTRAINT [unique_guest] UNIQUE NONCLUSTERED ([GuestFirstName] ASC, [GuestLastName] ASC), -- Unique constraint on GuestFirstName and GuestLastName
    FOREIGN KEY ([RoomNumber]) REFERENCES [dbo].[Rooms] ([RoomNumber]), -- Foreign Key constraint to Rooms table
    CONSTRAINT [check_guest] CHECK ([Status] = 'pending' OR [Status] = 'checked out' OR [Status] = 'checked in') -- Check constraint on Status
);
GO

-- Create the Rooms table
CREATE TABLE [dbo].[Rooms] (
    [RoomNumber] INT            IDENTITY (1, 1) NOT NULL, -- Primary Key, auto-incrementing
    [RoomType]   VARCHAR (7)    NOT NULL, -- Type of the room (triple, single, twin)
    [Floor]      INT            NOT NULL, -- Floor number
    [Status]     VARCHAR (9)    NOT NULL, -- Status of the room (occupied, available)
    [RoomPrice]  DECIMAL (7, 2) NOT NULL, -- Price of the room
    PRIMARY KEY CLUSTERED ([RoomNumber] ASC), -- Primary Key constraint
    CHECK ([RoomType] = 'tripple' OR [RoomType] = 'single' OR [RoomType] = 'twin'), -- Check constraint on RoomType
    CHECK ([Status] = 'occupied' OR [Status] = 'available') -- Check constraint on Status
);
GO

-- Create the UserTbl table for login information
CREATE TABLE [dbo].[UserTbl] (
    [UId]    INT          IDENTITY (1, 1) NOT NULL, -- Primary Key, auto-incrementing
    [UName]  VARCHAR (50) NOT NULL, -- Username
    [UPhone] VARCHAR (50) NOT NULL, -- User's phone number
    [UPass]  VARCHAR (50) NOT NULL, -- User's password
    PRIMARY KEY CLUSTERED ([UId] ASC) -- Primary Key constraint
);
GO

-- Procedure to delete a single booking and update the guest status
CREATE PROC deleteasinglebooking
    @roomnum INT = NULL, -- Room number
    @fullname VARCHAR(30) -- Full name of the guest
AS
BEGIN
    -- Delete the booking
    DELETE FROM Booking WHERE RoomNumber = @roomnum;
    -- Update the guest status to 'checked out'
    UPDATE Guests SET status = 'checked out' WHERE RoomNumber = @roomnum;
END;
GO

-- Procedure to insert into bookings
CREATE PROC InsertintoBookings
    @Gname VARCHAR(30), -- Guest's full name
    @GDOB DATE, -- Guest's date of birth
    @Ggender VARCHAR(6), -- Guest's gender
    @Gphone VARCHAR(13), -- Guest's phone number
    @Gcheckindate DATE, -- Check-in date
    @Gcheckoutdate DATE, -- Check-out date
    @roomnum INT -- Room number
AS
BEGIN
    DECLARE @FirstName VARCHAR(15), @LastName VARCHAR(15), @guestid INT, @Totalprice MONEY;

    -- Split the full name into first name and last name
    EXEC usp_SPLITNAME @Gname, @FirstName OUTPUT, @LastName OUTPUT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insert into Guests table
        INSERT INTO Guests (GuestFirstName, GuestLastName, GuestDOB, GuestGender, GuestPhone, CheckInDate, CheckOutDate, RoomNumber, Status) 
        VALUES (@FirstName, @LastName, @GDOB, @Ggender, @Gphone, @Gcheckindate, @Gcheckoutdate, @roomnum, 'checked in');
        SET @guestid = SCOPE_IDENTITY();

        -- Calculate total price
        SELECT @Totalprice = DATEDIFF(DAY, @Gcheckindate, @Gcheckoutdate) * RoomPrice 
        FROM Rooms WHERE RoomNumber = @roomnum;

        -- Insert into Booking table
        INSERT INTO Booking (RoomNumber, GuestID, BookingDate, TotalPrice) 
        VALUES (@roomnum, @guestid, @Gcheckindate, @Totalprice);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
    END CATCH;
END;
GO

-- Procedure to modify the guest details
CREATE PROCEDURE MODIFYGuest
    @name VARCHAR(30) = NULL, -- Guest's full name
    @DOB DATE = NULL, -- Guest's date of birth
    @phone VARCHAR(13) = NULL, -- Guest's phone number
    @gender VARCHAR(6) = NULL, -- Guest's gender
    @roomnum INT -- Room number
AS
BEGIN
    DECLARE @fname VARCHAR(15) = NULL, @lname VARCHAR(15) = NULL;

    -- Split the full name into first name and last name if name is provided
    IF (@name IS NOT NULL)
    BEGIN
        EXEC usp_SPLITNAME @name, @fname OUTPUT, @lname OUTPUT;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Update Guests table
        UPDATE Guests SET
            [GuestFirstName] = COALESCE(@fname, [GuestFirstName]),
            [GuestLastName] = COALESCE(@lname, [GuestLastName]),
            [GuestDOB] = COALESCE(@DOB, [GuestDOB]),
            [GuestGender] = COALESCE(@gender, [GuestGender]),
            [GuestPhone] = COALESCE(@phone, [GuestPhone])
        WHERE RoomNumber = @roomnum;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
    END CATCH;
END;
GO

-- Procedure to split the full name into first name and last name
CREATE PROC usp_SPLITNAME
    @FullName VARCHAR(30), -- Full name of the guest
    @FirstName VARCHAR(15) OUTPUT, -- First name output
    @LastName VARCHAR(15) OUTPUT -- Last name output
AS
BEGIN
    DECLARE @SpaceIndex INT;

    -- Find the index of the first space
    SET @SpaceIndex = CHARINDEX(' ', @FullName);

    -- If there's no

 space, assume only the first name is provided
    IF @SpaceIndex = 0
    BEGIN
        SET @FirstName = @FullName;
        SET @LastName = '';
    END
    ELSE
    BEGIN
        -- Extract the first name
        SET @FirstName = SUBSTRING(@FullName, 1, @SpaceIndex - 1);

        -- Extract the last name
        SET @LastName = SUBSTRING(@FullName, @SpaceIndex + 1, LEN(@FullName));
    END
END;
GO

-- Create the BookingView view to display booking details
CREATE VIEW BookingView
AS
SELECT DISTINCT 
    G.GuestFirstName + ' ' + G.GuestLastName AS [Full Name], -- Full name of the guest
    DATEDIFF(YEAR, G.GuestDOB, GETDATE()) AS Age, -- Age of the guest
    G.GuestDOB AS [Date Of Birth], -- Date of birth of the guest
    G.GuestGender AS Gender, -- Gender of the guest
    G.GuestPhone AS [Phone Number], -- Phone number of the guest
    G.CheckInDate AS [Check In Date], -- Check-in date
    G.CheckOutDate AS [Check Out Date], -- Check-out date
    R.RoomNumber AS [Room Number], -- Room number
    R.RoomType AS [Room Type], -- Type of the room
    R.Floor, -- Floor number
    R.RoomPrice AS [Room Price], -- Price of the room
    B.BookingDate AS [Book Date], -- Date of booking
    B.TotalPrice AS [Total Price] -- Total price of the booking
FROM Rooms AS R
JOIN Guests AS G ON B.RoomNumber = G.RoomNumber
JOIN Booking AS B ON B.RoomNumber = R.RoomNumber;
GO

-- Create the GuestView view to display guest details
CREATE VIEW GuestView
AS
SELECT 
    GuestFirstName + ' ' + GuestLastName AS [Full Name], -- Full name of the guest
    DATEDIFF(YEAR, GuestDOB, GETDATE()) AS Age, -- Age of the guest
    GuestDOB AS [Date Of Birth], -- Date of birth of the guest
    GuestGender AS Gender, -- Gender of the guest
    GuestPhone AS [Phone Number], -- Phone number of the guest
    Status -- Status of the guest
FROM Guests;
GO

-- Create the RoomsView view to display available rooms
CREATE VIEW RoomsView
AS
SELECT * FROM Rooms 
WHERE Status NOT LIKE 'occupied'; -- Only select rooms that are not occupied
GO
