﻿// <auto-generated />
using System;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataBase.Migrations
{
    [DbContext(typeof(BDContext))]
    partial class BDContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataBase.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Autor", (string)null);
                });

            modelBuilder.Entity("DataBase.CategoriaLibro", b =>
                {
                    b.Property<int>("IdCategoriaLibro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategoriaLibro"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCategoriaLibro");

                    b.ToTable("CategoriaLibro", (string)null);
                });

            modelBuilder.Entity("DataBase.DetalleLibroAutor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<int>("LibroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("LibroId");

                    b.ToTable("DetalleLibroAutor", (string)null);
                });

            modelBuilder.Entity("DataBase.DetallePrestamoLibro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LibroId")
                        .HasColumnType("int");

                    b.Property<int>("PrestamoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LibroId");

                    b.HasIndex("PrestamoId");

                    b.ToTable("DetallePrestamoLibro", (string)null);
                });

            modelBuilder.Entity("DataBase.Libro", b =>
                {
                    b.Property<int>("IdLibro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLibro"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaPublicacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumeroPaginas")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdLibro");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Libro", (string)null);
                });

            modelBuilder.Entity("DataBase.NivelUsuario", b =>
                {
                    b.Property<int>("IdNivelUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdNivelUsuario"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdNivelUsuario");

                    b.ToTable("NivelUsuario", (string)null);
                });

            modelBuilder.Entity("DataBase.Prestamo", b =>
                {
                    b.Property<int>("IdPrestamo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPrestamo"));

                    b.Property<bool>("EstadoPrestamo")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FechaDevolucion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaPrestamo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("HoraDevolucion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("HoraPrestamo")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("IdPrestamo");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Prestamo", (string)null);
                });

            modelBuilder.Entity("DataBase.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DNI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<int>("NivelUsuarioId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Telefono")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NivelUsuarioId");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("DataBase.DetalleLibroAutor", b =>
                {
                    b.HasOne("DataBase.Autor", "Autor")
                        .WithMany("DetalleLibroAutores")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataBase.Libro", "Libro")
                        .WithMany("DetalleLibroAutores")
                        .HasForeignKey("LibroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Libro");
                });

            modelBuilder.Entity("DataBase.DetallePrestamoLibro", b =>
                {
                    b.HasOne("DataBase.Libro", "Libro")
                        .WithMany("DetallePrestamoLibros")
                        .HasForeignKey("LibroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataBase.Prestamo", "Prestamo")
                        .WithMany("DetallePrestamoLibros")
                        .HasForeignKey("PrestamoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Libro");

                    b.Navigation("Prestamo");
                });

            modelBuilder.Entity("DataBase.Libro", b =>
                {
                    b.HasOne("DataBase.CategoriaLibro", "Categoria")
                        .WithMany("Libros")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("DataBase.Prestamo", b =>
                {
                    b.HasOne("DataBase.Usuario", "Usuario")
                        .WithMany("Prestamos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("DataBase.Usuario", b =>
                {
                    b.HasOne("DataBase.NivelUsuario", "NivelUsuario")
                        .WithMany("Usuarios")
                        .HasForeignKey("NivelUsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NivelUsuario");
                });

            modelBuilder.Entity("DataBase.Autor", b =>
                {
                    b.Navigation("DetalleLibroAutores");
                });

            modelBuilder.Entity("DataBase.CategoriaLibro", b =>
                {
                    b.Navigation("Libros");
                });

            modelBuilder.Entity("DataBase.Libro", b =>
                {
                    b.Navigation("DetalleLibroAutores");

                    b.Navigation("DetallePrestamoLibros");
                });

            modelBuilder.Entity("DataBase.NivelUsuario", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("DataBase.Prestamo", b =>
                {
                    b.Navigation("DetallePrestamoLibros");
                });

            modelBuilder.Entity("DataBase.Usuario", b =>
                {
                    b.Navigation("Prestamos");
                });
#pragma warning restore 612, 618
        }
    }
}
