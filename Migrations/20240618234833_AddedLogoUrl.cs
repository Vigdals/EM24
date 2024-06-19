using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchBetting.Migrations
{
    /// <inheritdoc />
    public partial class AddedLogoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayTeamLogoUrl",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamLogoUrl",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamLogoUrl",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeTeamLogoUrl",
                table: "Matches");
        }
    }
}
