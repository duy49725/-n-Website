using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnWebNangCao.Migrations
{
    public partial class usernameorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Order");
        }
    }
}
