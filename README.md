CREATE DATABASE MemberEligibility
USE MemberEligibility

GO
CREATE TABLE TechnologyEntity (
TechnologyID INT IDENTITY(1,1) PRIMARY KEY , 
TechnologyName VARCHAR(50)
)
GO
PRINT 'Technology entity table created successfully'


GO
CREATE TABLE MemberEntity (
MemberID INT IDENTITY(1,1) PRIMARY KEY , 
MemberName NVARCHAR(50),
TechnologyID INT CONSTRAINT fk_MemberEntity_TechnologyID FOREIGN KEY REFERENCES TechnologyEntity(TechnologyID),
DateOfBirth DATETIME, 
Qualification VARCHAR(50), 
YearsOfExperience DECIMAL(18,2) NOT NULL CHECK (YearsOfExperience >= 3)
)
GO
PRINT 'Member entity table created successfully'

GO
INSERT INTO MEMBERENTITY VALUES('MEMBER 1',1,GETDATE(),'B.TECH',4)
GO
PRINT 'Member entity records inserted successfully'
GO
INSERT INTO TECHNOLOGYENTITY VALUES('C#')
INSERT INTO TECHNOLOGYENTITY VALUES('JQUERY')
GO
PRINT 'Technologies Entity records inserted successfully'
