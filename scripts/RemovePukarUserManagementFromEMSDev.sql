/*
  Removes Pukar.Usermanagement EF objects from a database (e.g. EMSDev).
  This module uses schema [um]: Users, RefreshTokens, Roles, UserRoles.
  Run in SSMS or sqlcmd against the database you want to clean (e.g. EMSDev).

  Order respects FKs: children first, then parents.
  Idempotent: uses IF OBJECT_ID checks.

  After this, point your app at PukarUsers only:
    ConnectionStrings:UserManagement -> Database=PukarUsers

  Optional: remove migration history rows for this module (dbo.__EFMigrationsHistory).
*/

SET NOCOUNT ON;

-- Child tables first
IF OBJECT_ID(N'um.RefreshTokens', N'U') IS NOT NULL
    DROP TABLE um.RefreshTokens;

IF OBJECT_ID(N'um.UserRoles', N'U') IS NOT NULL
    DROP TABLE um.UserRoles;

IF OBJECT_ID(N'um.Users', N'U') IS NOT NULL
    DROP TABLE um.Users;

IF OBJECT_ID(N'um.Roles', N'U') IS NOT NULL
    DROP TABLE um.Roles;

-- EF history for this context is usually in dbo (see your DB; um.__EFMigrationsHistory if used)
IF OBJECT_ID(N'um.__EFMigrationsHistory', N'U') IS NOT NULL
    DROP TABLE um.__EFMigrationsHistory;

-- Remove only this module's migration rows from dbo history (safe if other apps share EMSDev)
IF OBJECT_ID(N'dbo.__EFMigrationsHistory', N'U') IS NOT NULL
BEGIN
    DELETE FROM dbo.__EFMigrationsHistory
    WHERE MigrationId IN (
        N'20260406124106_InitialUserManagement',
        N'20260409120611_AddRbacRoles',
        N'20260411075412_AddEmailConfirmationToUsers'
    );
END

PRINT N'Pukar user-management objects removed from current database (where present).';

/*
  Note: This module stores users in [um].[Users]. If you see a stray [dbo].[Users]
  from an old test, do not drop it unless you are sure nothing else uses it.
*/
