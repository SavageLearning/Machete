INSERT INTO dbo.Applications (ApplicationName, ApplicationId, Description)
SELECT ApplicationName, ApplicationId, Description FROM aspnetdb.dbo.aspnet_Applications
GO
 
INSERT INTO dbo.Roles (ApplicationId, RoleId, RoleName, Description)
SELECT ApplicationId, RoleId, RoleName, Description FROM aspnetdb.dbo.aspnet_Roles
GO
 
INSERT INTO dbo.Users (ApplicationId, UserId, UserName, IsAnonymous, LastActivityDate)
SELECT ApplicationId, UserId, UserName, IsAnonymous, LastActivityDate FROM aspnetdb.dbo.aspnet_Users
GO
 
INSERT INTO dbo.Memberships (ApplicationId, UserId, Password, 
PasswordFormat, PasswordSalt, Email, PasswordQuestion, PasswordAnswer, 
IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, 
LastLockoutDate, FailedPasswordAttemptCount, 
FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, 
FailedPasswordAnswerAttemptWindowsStart, Comment) 
SELECT ApplicationId, UserId, Password, 
PasswordFormat, PasswordSalt, Email, PasswordQuestion, PasswordAnswer, 
IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, 
LastLockoutDate, FailedPasswordAttemptCount, 
FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, 
FailedPasswordAnswerAttemptWindowStart, Comment FROM aspnetdb.dbo.aspnet_Membership
GO
 
INSERT INTO dbo.UsersInRoles SELECT * FROM aspnetdb.dbo.aspnet_UsersInRoles
GO