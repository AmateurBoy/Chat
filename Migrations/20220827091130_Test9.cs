using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatMarchenkoIlya.Migrations
{
    public partial class Test9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Chats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Chats");
        }
    }
}
