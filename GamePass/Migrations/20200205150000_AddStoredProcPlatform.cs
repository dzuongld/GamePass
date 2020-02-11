using Microsoft.EntityFrameworkCore.Migrations;

namespace GamePass.Migrations
{
    public partial class AddStoredProcPlatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC usp_GetPlatforms 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM   dbo.Platforms 
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_GetPlatform 
                                    @Id int 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM   dbo.Platforms  WHERE  (Id = @Id) 
                                    END ");

            migrationBuilder.Sql(@"CREATE PROC usp_UpdatePlatform
	                                @Id int,
	                                @Name varchar(100)
                                    AS 
                                    BEGIN 
                                     UPDATE dbo.Platforms
                                     SET  Name = @Name
                                     WHERE  Id = @Id
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_DeletePlatform
	                                @Id int
                                    AS 
                                    BEGIN 
                                     DELETE FROM dbo.Platforms
                                     WHERE  Id = @Id
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_CreatePlatform
                                   @Name varchar(100)
                                   AS 
                                   BEGIN 
                                    INSERT INTO dbo.Platforms(Name)
                                    VALUES (@Name)
                                   END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetPlatforms");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetPlatform");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_UpdatePlatform");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_DeletePlatform");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_CreatePlatform");
        }
    }
}
