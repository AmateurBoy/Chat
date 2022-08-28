using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatMarchenkoIlya.Migrations
{
    public partial class fdsfsdfsdfsdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reply",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reply",
                table: "Messages");
        }
    }
}
