﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixVSFsfdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Obed",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Ranajky",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Vecera",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealType",
                table: "Recipes");

            migrationBuilder.AddColumn<bool>(
                name: "Obed",
                table: "Recipes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ranajky",
                table: "Recipes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Vecera",
                table: "Recipes",
                type: "bit",
                nullable: true);
        }
    }
}
