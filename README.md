# ContractsTrackingASP

To Create Datebase, please run these queries in order
******************************************QUERY 1:
CREATE DATABASE ContractsTracking;

******************************************QUERY 2:
CREATE TABLE ContractsTracking.dbo.Contracts (
	customerName VARCHAR(255) NOT NULL,
	status VARCHAR(255) NOT NULL,
	contractNumber VARCHAR(255) PRIMARY KEY,
	dealerName VARCHAR(255),
	dealerNumber VARCHAR(255),
	dealerNotes VARCHAR(255),
	amountFinanced decimal(38,2),
	assignedUser VARCHAR(255)
);
CREATE TABLE ContractsTracking.dbo.PendingIssues (
	contractNumber VARCHAR(255) NOT NULL,
	name VARCHAR(255) NOT NULL,
	assignedGroup VARCHAR(255) NOT NULL,
	status VARCHAR(255) NOT NULL,
	timeReported datetime NOT NULL
);
CREATE TABLE ContractsTracking.dbo.ProblemLabels (
	problemName VARCHAR(255) PRIMARY KEY,
	defaultGroup VARCHAR(255) NOT NULL
);
CREATE TABLE ContractsTracking.dbo.Users (
	DisplayName VARCHAR(255) NOT NULL,
	role VARCHAR(255) NOT NULL,
	username VARCHAR(50) PRIMARY KEY
);

******************************************QUERY 3:
INSERT INTO ContractsTracking.dbo.Contracts (customerName, status, contractNumber, dealerName, dealerNumber, dealerNotes, amountFinanced, assignedUser)
	VALUES
		('Nicholas Goodwin','Docs Received','FA-1234','HLAVINKA','034132.9900','Insert Notes Here',42675.98,'goodwinn'),
		('Nicholas Goodwin','Uploaded','FA-1235','KIOTO','037562.5200','Insert Notes Here',42675.98,'mccroryd'),
		('Nicholas Goodwin','Accepted','FA-14756','Generic Dealer Name, Inc.','000000.00','Insert Notes Here',42675.98,'goodwinn');
    
INSERT INTO ContractsTracking.dbo.PendingIssues (contractNumber, name, assignedGroup, status, timeReported)
	VALUES
		('FA-1234','Address Is Incorrect','Sales','In Progress', GETDATE()),
		('FA-1234','Documents are missing','Retail Credit','New', GETDATE()),
		('FA-1235','Perfect','Contract Admin','Resolved',GETDATE()),
		('FA-14756','Perfect','Contract Admin','Resolved',GETDATE());
    
INSERT INTO ContractsTracking.dbo.ProblemLabels (problemName, defaultGroup)
	VALUES 
		('Address Is Incorrect','Sales'),
		('Asset Is ALready Financed','Contract Admin'),
		('Documents are missing','Retail Credit'),
		('Future Delivery','Inside Sales');

INSERT INTO ContractsTracking.dbo.Users (DisplayName, role, username)
	VALUES
		('Nicholas Goodwin','Retail Credit','goodwinn'),
		('Doug McCrory','Inventory Finance','mccroryd'),
		('Collin Bakkie','Sales','bakkiec'),
		('Amanda Steele','Contract Admin Manager','steelea');
