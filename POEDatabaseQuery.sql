USE master
IF EXISTS	(SELECT * FROM sys.databases WHERE name = 'POEDatabase')


USE POEDatabase

--TABLE CREATION
CREATE TABLE Event1(

EventID INT IDENTITY(1, 1) PRIMARY KEY,
Event_Name NVARCHAR(250) NOT NULL,
Event_Date DateTime NOT NULL,
Description1 NVARCHAR(250) NOT NULL,
VenueID int NULL,
EventTypeID INT NULL,
FOREIGN KEY (VenueID) REFERENCES Venue1(VenueID) ON DELETE NO ACTION,
FOREIGN KEY (EventTypeID) REFERENCES EventType(EventTypeID) ON DELETE NO ACTION
);

CREATE TABLE Venue1(

VenueID INT IDENTITY(1, 1) PRIMARY KEY,
Venue_Name NVARCHAR(250) NOT NULL,
Location1 NVARCHAR(250) NOT NULL,
Capacity int NOT NULL,
ImageURL NVARCHAR(250),
);

CREATE TABLE Bookings1(

BookingID INT IDENTITY(1, 1) PRIMARY KEY,
EventID int NULL,
VenueID int NULL,
Booking_Date DATETime NOT NULL,
FOREIGN KEY (EventID) REFERENCES Event1(EventID) ON DELETE CASCADE,
FOREIGN KEY (VenueID) REFERENCES Venue1(VenueID) ON DELETE CASCADE,
CONSTRAINT UQ_Venue1_Event1 UNIQUE (VenueID, EventID)
);

CREATE TABLE EventType(
EventTypeID INT IDENTITY(1, 1) PRIMARY KEY,
Name NVARCHAR(100) NOT NULL,
);

CREATE UNIQUE INDEX UQ_Venue1_Bookings1 ON Bookings1(VenueID, EventID);

INSERT INTO Venue1(Venue_Name, Location1, Capacity, ImageURL)
VALUES
('Wedding', 'New York', 300, 'https://picsum.photos/100/100'),
('Conference', 'Los Veges', 100, 'https://picsum.photos/100/100'),
('Grand Hull', 'Missiour', 500, 'https://picsum.photos/100/100');

INSERT INTO Event1(Event_Name, Event_Date, Description1, VenueID, EventTypeID)
VALUES
('Marriage', '10/02/2025', 'Wedding party', 1, 2),
('Meeting', '11/02/2025', 'Meeting area', 2, 1),
('Dance', '12/02/2025', 'Dancing party', 3, 3);

INSERT INTO Bookings1(EventID, VenueID, Booking_Date)
VALUES
(2, 1, '10/01/2025'),
(3, 2, '11/01/2025'),
(4, 3, '12/01/2025');

INSERT INTO EventType(Name)
VALUES
('Conference'),
('Wedding'),
('Dance');

SELECT * FROM Venue1;
SELECT * FROM Event1;
SELECT * FROM Bookings1;
SELECT * FROM EventType;

Use POEDatabase;
DROP TABLE Event1;
DROP TABLE Venue1;
DROP TABLE Bookings1;
DROP TABLE EventType;
