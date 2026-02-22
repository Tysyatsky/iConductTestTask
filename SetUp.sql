CREATE TABLE dbo.Employee
(
    Id        INT           NOT NULL PRIMARY KEY,
    Name      VARCHAR(100)  NOT NULL,
    ManagerId INT           NULL,
    Enable    BIT           NOT NULL
);

INSERT INTO dbo.Employee (Id, Name, ManagerId, Enable) VALUES
(1,'Andrey',NULL, 1),
(2,'Alexa',1, 1),
(3,'Roman',2, 1),
(5,'Test Employee',3, 0),
(6,'Second Test Employee',3, 0);

CREATE INDEX IX_Employees_ManagerId ON Employee(ManagerId);

select * from dbo.Employee;