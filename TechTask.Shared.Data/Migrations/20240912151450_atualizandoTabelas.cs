using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTask.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class atualizandoTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Tarefas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Tarefas");
        }
    }
}
