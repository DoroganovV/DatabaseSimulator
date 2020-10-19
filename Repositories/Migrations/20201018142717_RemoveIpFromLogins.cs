using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
{
    public partial class RemoveIpFromLogins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HostLogin",
                table: "LoginHistorys",
                newName: "Host");

            migrationBuilder.DropColumn(
                name: "IpLogin",
                table: "LoginHistorys");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpLogin",
                table: "LoginHistorys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "Host",
                table: "LoginHistorys",
                newName: "HostLogin");
        }
    }
}
