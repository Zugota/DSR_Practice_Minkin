using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSRPractice.Services.Migrations
{
    /// <inheritdoc />
    public partial class InitSPGetEmpByID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE CodeSPGetEmpByID
                                @ID int
                                AS
                                Begin
	                                SELECT * FROM Employees WHERE Id = @ID;
                                End";
            migrationBuilder.Sql(procedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE CodeSPGetEmpByID";
            migrationBuilder.Sql(procedure);
        }
    }
}