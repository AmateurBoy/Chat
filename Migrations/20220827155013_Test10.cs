using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatMarchenkoIlya.Migrations
{
    public partial class Test10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisplay",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisplay",
                table: "Messages");
        }
    }
}
