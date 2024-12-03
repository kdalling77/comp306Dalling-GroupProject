using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _301247589_301276375_bright_aid_API.Migrations.Donor
{
    /// <inheritdoc />
    public partial class AddedSoftDeleteToDonorTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Donors",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Donors");
        }
    }
}
