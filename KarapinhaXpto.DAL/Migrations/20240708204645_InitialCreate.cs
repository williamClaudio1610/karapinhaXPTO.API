using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarapinhaXpto.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfils", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profissionals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profissionals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servicos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfissionalCategorias",
                columns: table => new
                {
                    ProfissionalId = table.Column<int>(type: "int", nullable: false),
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionalCategorias", x => new { x.ProfissionalId, x.CategoriaID });
                    table.ForeignKey(
                        name: "FK_ProfissionalCategorias_Categorias_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfissionalCategorias_Profissionals_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfissionalHorarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfissionalID = table.Column<int>(type: "int", nullable: false),
                    HorarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionalHorarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfissionalHorarios_Horarios_HorarioID",
                        column: x => x.HorarioID,
                        principalTable: "Horarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfissionalHorarios_Profissionals_ProfissionalID",
                        column: x => x.ProfissionalID,
                        principalTable: "Profissionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marcacaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPagar = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    UtilizadorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcacaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Marcacaos_Utilizadors_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizadors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServicoMarcacaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicoId = table.Column<int>(type: "int", nullable: false),
                    MarcacaoId = table.Column<int>(type: "int", nullable: false),
                    ProfissionalId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    DataAnterior = table.Column<DateOnly>(type: "date", nullable: true),
                    HoraAnterior = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicoMarcacaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicoMarcacaos_Marcacaos_MarcacaoId",
                        column: x => x.MarcacaoId,
                        principalTable: "Marcacaos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicoMarcacaos_Profissionals_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicoMarcacaos_Servicos_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Marcacaos_UtilizadorId",
                table: "Marcacaos",
                column: "UtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalCategorias_CategoriaID",
                table: "ProfissionalCategorias",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalHorarios_HorarioID",
                table: "ProfissionalHorarios",
                column: "HorarioID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalHorarios_ProfissionalID",
                table: "ProfissionalHorarios",
                column: "ProfissionalID");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoMarcacaos_MarcacaoId",
                table: "ServicoMarcacaos",
                column: "MarcacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoMarcacaos_ProfissionalId",
                table: "ServicoMarcacaos",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoMarcacaos_ServicoId",
                table: "ServicoMarcacaos",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_CategoriaId",
                table: "Servicos",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perfils");

            migrationBuilder.DropTable(
                name: "ProfissionalCategorias");

            migrationBuilder.DropTable(
                name: "ProfissionalHorarios");

            migrationBuilder.DropTable(
                name: "ServicoMarcacaos");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "Marcacaos");

            migrationBuilder.DropTable(
                name: "Profissionals");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "Utilizadors");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
