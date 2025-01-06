﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentResponseAPI.Migrations
{
    /// <inheritdoc />
    public partial class IncidentsModelModifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Incidents");
        }
    }
}
