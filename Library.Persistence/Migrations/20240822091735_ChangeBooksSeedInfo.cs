using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBooksSeedInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Books",
                newName: "ImagePath");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Authors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Genre", "ImagePath", "Name" },
                values: new object[] { "Новелла, написанная Александром Пушкиным", "Новелла", "8fb16b40-17d4-43fa-b3fa-20238b342ad3.jpg", "Евгений Онегин" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Genre", "ImagePath" },
                values: new object[] { "Хоррор, написанный Стивеном Кингом", "Хоррор", "theShining.jpg" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Genre", "ImagePath", "Name" },
                values: new object[] { "Философская новелла, написанная Федором Достоевским.", "Философская новелла", "95a036bc205187af0456953a28ccccb1.jpeg", "Преступление и наказание" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Genre", "ImagePath", "Name" },
                values: new object[] { "Философская новелла, написанная Федором Достоевским.", "Философская новелла", "a6d50e17-c422-4c07-b73d-3b9e722fa1bb.jpg", "Братья Карамазовы" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Genre", "ImagePath", "Name" },
                values: new object[] { "Хоррор, написанный Стивеном Кингом", "Хоррор", "i750566.jpg", "Оно" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Books",
                newName: "ImageUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Authors",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Genre", "ImageUrl", "Name" },
                values: new object[] { "A novel in verse by Alexander Pushkin.", "Novel", "https://example.com/eugene_onegin.jpg", "Eugene Onegin" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Genre", "ImageUrl" },
                values: new object[] { "A horror novel by Stephen King.", "Horror", "https://example.com/the_shining.jpg" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Genre", "ImageUrl", "Name" },
                values: new object[] { "A philosophical novel by Fyodor Dostoevsky.", "Philosophical Novel", "https://example.com/crime_and_punishment.jpg", "Crime and Punishment" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Genre", "ImageUrl", "Name" },
                values: new object[] { "A philosophical novel by Fyodor Dostoevsky.", "Philosophical Novel", "https://example.com/brothers_karamazov.jpg", "The Brothers Karamazov" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Genre", "ImageUrl", "Name" },
                values: new object[] { "A horror novel by Stephen King.", "Horror", "https://example.com/it.jpg", "It" });
        }
    }
}
