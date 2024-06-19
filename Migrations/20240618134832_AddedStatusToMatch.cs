using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchBetting.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MatchStatus",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MatchStatusId",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchStatus",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchStatusId",
                table: "Matches");
        }
    }
}
