USE [GuestHouse]
GO

/****** Object:  Table [dbo].[Guests]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Guests](
	[GuestID] [int] IDENTITY(1,1) NOT NULL,
	[GuestFirstName] [varchar](15) NOT NULL,
	[GuestLastName] [varchar](15) NULL,
	[GuestPhone] [varchar](13) NULL,
	[CheckInDate] [datetime] NULL,
	[CheckOutDate] [datetime] NULL,
	[RoomNumber] [int] NULL,
	[Status] [varchar](15) NULL,
	[GuestDOB] [date] NULL,
	[GuestGender] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[GuestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [unique_guest] UNIQUE NONCLUSTERED 
(
	[GuestFirstName] ASC,
	[GuestLastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  View [dbo].[GuestView]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   view [dbo].[GuestView]
as
Select GuestID,GuestFirstName+' '+GuestLastName as [Full Name] from Guests
GO

/****** Object:  Table [dbo].[Rooms]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[RoomNumber] [int] IDENTITY(1,1) NOT NULL,
	[RoomType] [varchar](7) NOT NULL,
	[Floor] [int] NOT NULL,
	[Status] [varchar](9) NOT NULL,
	[RoomPrice] [decimal](7, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Booking]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[BookingID] [int] IDENTITY(1,1) NOT NULL,
	[RoomNumber] [int] NULL,
	[GuestID] [int] NULL,
	[BookingDate] [datetime] NULL,
	[TotalPrice] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[BookingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Book_unique] UNIQUE NONCLUSTERED 
(
	[RoomNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Book_unique2] UNIQUE NONCLUSTERED 
(
	[RoomNumber] ASC,
	[GuestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  View [dbo].[BookingView]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Create the BookingView view
CREATE   VIEW [dbo].[BookingView]
AS
SELECT distinct G.GuestFirstName +' '+ G.GuestLastName  AS [Full Name] , DATEDIFF(YEAR,G.GuestDOB,GETDATE()) AS Age,GuestGender as Gender, G.GuestPhone AS [Phone Number], G.CheckInDate AS [Check In Date], G.CheckOutDate AS [Check Out Date], R.RoomNumber AS [Room Number], R.RoomType AS [Room Type], R.Floor, R.RoomPrice AS [Room Price], B.BookingDate AS [Book Date], B.TotalPrice AS [Total Price] FROM Rooms AS R, Guests AS G, Booking AS B
WHERE B.RoomNumber = R.RoomNumber AND B.RoomNumber = G.RoomNumber;
GO

/****** Object:  View [dbo].[RoomsView]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create the RoomsView view
CREATE   VIEW [dbo].[RoomsView]
AS
SELECT * FROM Rooms 
WHERE Status NOT LIKE 'occupied';
GO

/****** Object:  Table [dbo].[UserTbl]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTbl](
	[UId] [int] IDENTITY(1,1) NOT NULL,
	[UName] [varchar](50) NOT NULL,
	[UPhone] [varchar](50) NOT NULL,
	[UPass] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([GuestID])
REFERENCES [dbo].[Guests] ([GuestID])
GO

ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([RoomNumber])
REFERENCES [dbo].[Rooms] ([RoomNumber])
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD FOREIGN KEY([RoomNumber])
REFERENCES [dbo].[Rooms] ([RoomNumber])
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [check_guest] CHECK  (([Status]='pending' OR [Status]='checked out' OR [Status]='checked in'))
GO
ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [check_guest]
GO

ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD CHECK  (([RoomType]='tripple' OR [RoomType]='single' OR [RoomType]='twin'))
GO

ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD CHECK  (([Status]='occupied' OR [Status]='available'))
GO

/****** Object:  StoredProcedure [dbo].[deleteasinglebooking]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[deleteasinglebooking]
@roomnum INT=null,
@fullname VARCHAR(30)
AS
BEGIN
DELETE FROM Booking WHERE RoomNumber=@roomnum
update Guests set status='checked out' where RoomNumber=@roomnum
END;
GO

/****** Object:  StoredProcedure [dbo].[InsertintoBookings]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create the stored procedure InsertintoBookings
CREATE   PROC [dbo].[InsertintoBookings]
@Gname VARCHAR(30),
@GDOB DATE,
@Ggender varchar(6),
@Gphone VARCHAR(13),
@Gcheckindate DATE,
@Gcheckoutdate DATE,
@roomnum INT
AS
BEGIN
DECLARE @FirstName VARCHAR(15), @LastName VARCHAR(15), @guestid INT, @Totalprice MONEY
EXEC usp_SPLITNAME @Gname, @FirstName OUTPUT, @LastName OUTPUT

BEGIN TRY
    BEGIN TRANSACTION

    -- Insert into Guests table
    INSERT INTO Guests(GuestFirstName,GuestLastName,GuestDOB,GuestGender,GuestPhone,CheckInDate,CheckOutDate,RoomNumber,Status) VALUES (@FirstName, @LastName,@GDOB, @Ggender, @Gphone, @Gcheckindate, @Gcheckoutdate, @roomnum,'checked in')
    SET @guestid = SCOPE_IDENTITY()

    -- Calculate total price
    SELECT @Totalprice = DATEDIFF(DAY, @Gcheckindate, @Gcheckoutdate) * RoomPrice FROM Rooms WHERE RoomNumber = @roomnum

    -- Insert into Booking table
    INSERT INTO Booking VALUES (@roomnum, @guestid, @Gcheckindate, @Totalprice)

    COMMIT
END TRY
BEGIN CATCH
    ROLLBACK
END CATCH

END;
GO

/****** Object:  StoredProcedure [dbo].[MODIFYBOOKING]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[MODIFYBOOKING]
@name VARCHAR(30)=null,
@DOB Date=null,
@phone VARCHAR(13)=null,
@gender varchar(6)=null,
@Gcheckindate DATE=null,
@Gcheckoutdate DATE=null,
@roomnum INT
AS
BEGIN
DECLARE @fname VARCHAR(15)=null, @lname VARCHAR(15)=null

IF (@name IS NOT NULL)
BEGIN
    EXEC usp_SPLITNAME @name, @fname OUTPUT, @lname OUTPUT
END

BEGIN TRY
    BEGIN TRANSACTION

    -- Update Guests table
    UPDATE Guests SET
        [GuestFirstName] = COALESCE(@fname, [GuestFirstName]),
        [GuestLastName] = COALESCE(@lname, [GuestLastName]),
        [GuestDOB] = COALESCE(@DOB, [GuestDOB]),
		[GuestGender]=COALESCE(@gender,[GuestGender]),
        [GuestPhone] = COALESCE(@phone, [GuestPhone]),
        [CheckInDate] = CASE WHEN @Gcheckindate IS NOT NULL THEN @Gcheckindate ELSE [CheckInDate] END,
        [CheckOutDate] = CASE WHEN @Gcheckoutdate IS NOT NULL THEN @Gcheckoutdate ELSE [CheckOutDate] END
    WHERE RoomNumber = @roomnum

    DECLARE @cout DATE, @cin DATE, @Totalprice MONEY

    SELECT @cout = [CheckOutDate], @cin = [CheckInDate] FROM Guests WHERE RoomNumber = @roomnum

    -- Calculate total price
    IF (@Gcheckindate IS NOT NULL AND @Gcheckoutdate IS NOT NULL)
    BEGIN
        SET @Totalprice = DATEDIFF(DAY, @Gcheckindate, @Gcheckoutdate) * (SELECT RoomPrice FROM Rooms WHERE RoomNumber = @roomnum)
    END
    ELSE IF (@Gcheckindate IS NOT NULL AND @Gcheckoutdate IS NULL)
    BEGIN
        SET @Totalprice = DATEDIFF(DAY, @Gcheckindate, @cout) * (SELECT RoomPrice FROM Rooms WHERE RoomNumber = @roomnum)
    END
    ELSE IF (@Gcheckindate IS NULL AND @Gcheckoutdate IS NOT NULL)
    BEGIN
        SET @Totalprice = DATEDIFF(DAY, @cin, @Gcheckoutdate) * (SELECT RoomPrice FROM Rooms WHERE RoomNumber = @roomnum)
    END

    -- Update Booking table
    UPDATE Booking SET
        BookingDate = COALESCE(@cin, BookingDate),
        TotalPrice = COALESCE(@Totalprice, TotalPrice)
    WHERE RoomNumber = @roomnum

    COMMIT
END TRY
BEGIN CATCH
    ROLLBACK
END CATCH

END;
GO



/****** Object:  StoredProcedure [dbo].[usp_SPLITNAME]    Script Date: 5/11/2024 11:02:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[usp_SPLITNAME]
@FullName VARCHAR(30),
@FirstName VARCHAR(15) OUTPUT,
@LastName VARCHAR(15) OUTPUT
AS
BEGIN
DECLARE @SpaceIndex INT

-- Find the index of the first space
SET @SpaceIndex = CHARINDEX(' ', @FullName)

-- If there's no space, assume only the first name is provided
IF @SpaceIndex = 0
BEGIN
    SET @FirstName = @FullName
    SET @LastName = ''
END
ELSE
BEGIN
    -- Extract the first name
    SET @FirstName = SUBSTRING(@FullName, 1, @SpaceIndex - 1)
    
    -- Extract the last name
    SET @LastName = SUBSTRING(@FullName, @SpaceIndex + 1, LEN(@FullName))
END

END;
GO
