--- DB Creation

USE master
GO
DROP DATABASE IF EXISTS seawolfhr
GO

CREATE DATABASE seawolfhr
GO 
USE seawolfhr
GO 

--- User Creation

USE master;
GO
CREATE LOGIN [sw_dbuser] WITH PASSWORD=N'Testpass123!', CHECK_EXPIRATION=OFF, CHECK_POLICY=ON;
GO
USE seawolfhr;
GO
CREATE USER [sw_dbuser] FOR LOGIN [sw_dbuser];
GO
EXEC sp_addrolemember N'db_owner', [sw_dbuser];
GO
