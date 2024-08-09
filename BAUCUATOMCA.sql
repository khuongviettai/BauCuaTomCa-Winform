CREATE DATABASE BAUCUATOMCA;

USE BAUCUATOMCA;




CREATE TABLE USERS (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Money DECIMAL(18, 2) DEFAULT 0 CHECK (Money >= 0)
);



CREATE TABLE Deposits (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255),
    Status NVARCHAR(255) DEFAULT 'Pending',
    Options NVARCHAR(255),
    Seri INT,
    Number INT,
    Money DECIMAL(18, 2),
	CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Username) REFERENCES USERS(Username)
);



CREATE TABLE Withdraws (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255),
    Status NVARCHAR(255),
    Phone INT,
    Name  NVARCHAR(255),
    Money DECIMAL(18, 2),
	CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Username) REFERENCES USERS(Username)
);

ALTER TABLE Deposits
ADD CONSTRAINT FK_Deposits_Users
FOREIGN KEY (Username) REFERENCES USERS(Username);

ALTER TABLE Withdraws
ADD CONSTRAINT FK_Withdraws_Users
FOREIGN KEY (Username) REFERENCES USERS(Username);



CREATE TRIGGER trg_AfterUpdateDeposits
ON Deposits
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Status)
    BEGIN
        DECLARE @UpdatedUsername NVARCHAR(255);
        DECLARE @UpdatedStatus NVARCHAR(255);
        DECLARE @PreviousStatus NVARCHAR(255);

        SELECT @UpdatedUsername = i.Username, @UpdatedStatus = i.Status, @PreviousStatus = d.Status
        FROM INSERTED i
        JOIN DELETED d ON i.ID = d.ID;

        IF @UpdatedStatus = 'Success' AND @PreviousStatus <> 'Success'
        BEGIN
            UPDATE USERS
            SET Money = Money + (SELECT Money FROM INSERTED)
            WHERE Username = @UpdatedUsername;
        END
    END
END;



CREATE TRIGGER trg_AfterUpdateWithdraws
ON Withdraws
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Status)
    BEGIN
        DECLARE @UpdatedUsername NVARCHAR(255);
        DECLARE @UpdatedStatus NVARCHAR(255);
        DECLARE @PreviousStatus NVARCHAR(255);

        SELECT @UpdatedUsername = i.Username, @UpdatedStatus = i.Status, @PreviousStatus = d.Status
        FROM INSERTED i
        JOIN DELETED d ON i.ID = d.ID;


        IF @UpdatedStatus = 'Success' AND @PreviousStatus <> 'Success'
        BEGIN
            UPDATE USERS
            SET Money = Money - (SELECT Money FROM INSERTED)
            WHERE Username = @UpdatedUsername;
        END
    END
END;


Select * from USERS;

Select * from Deposits;

Select * from Withdraws;

UPDATE Withdraws
SET Status = 'Success'
WHERE Username = 'admin' AND Id = 2;



UPDATE Deposits
SET Status = 'Success'
WHERE Username = 'admin' AND Id = 3;
