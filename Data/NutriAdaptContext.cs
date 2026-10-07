using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NutriAdapt.Models;

namespace NutriAdapt.Data;

public partial class NutriAdaptContext : DbContext
{
    public NutriAdaptContext(DbContextOptions<NutriAdaptContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alimentos> Alimentos { get; set; }

    public virtual DbSet<GruposAlimenticios> GruposAlimenticios { get; set; }

    public virtual DbSet<Nutriologos> Nutriologos { get; set; }

    public virtual DbSet<Pacientes> Pacientes { get; set; }

    public virtual DbSet<PlanPorciones> PlanPorciones { get; set; }

    public virtual DbSet<PlanesNutricionales> PlanesNutricionales { get; set; }

    public virtual DbSet<RecetaAdaptadaDetalle> RecetaAdaptadaDetalle { get; set; }

    public virtual DbSet<RecetaIngredientes> RecetaIngredientes { get; set; }

    public virtual DbSet<Recetas> Recetas { get; set; }

    public virtual DbSet<RecetasAdaptadas> RecetasAdaptadas { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alimentos>(entity =>
        {
            entity.HasKey(e => e.AlimentoId).HasName("PK__Alimento__7047A2B94873F778");

            entity.HasIndex(e => e.GrupoId, "IX_Alimentos_GrupoId");

            entity.Property(e => e.GramosPorPorcion).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.MedidaCasera).HasMaxLength(100);
            entity.Property(e => e.NombreAlimento).HasMaxLength(150);

            entity.HasOne(d => d.Grupo).WithMany(p => p.Alimentos)
                .HasForeignKey(d => d.GrupoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alimentos_Grupos");
        });

        modelBuilder.Entity<GruposAlimenticios>(entity =>
        {
            entity.HasKey(e => e.GrupoId).HasName("PK__GruposAl__556BF040C7E56820");

            entity.HasIndex(e => e.NombreGrupo, "UQ__GruposAl__53EBC7DFF9C00D46").IsUnique();

            entity.Property(e => e.HidratosPorPorcion).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.KcalPorPorcion).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.LipidosPorPorcion).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.NombreGrupo).HasMaxLength(50);
            entity.Property(e => e.ProteinaPorPorcion).HasColumnType("decimal(6, 2)");
        });

        modelBuilder.Entity<Nutriologos>(entity =>
        {
            entity.HasKey(e => e.NutriologoId).HasName("PK__Nutriolo__FF0146A6F24FA2E6");

            entity.HasIndex(e => e.UsuarioId, "UQ__Nutriolo__2B3DE7B9503E5328").IsUnique();

            entity.Property(e => e.CedulaProfesional).HasMaxLength(20);
            entity.Property(e => e.Especialidad).HasMaxLength(100);

            entity.HasOne(d => d.Usuario).WithOne(p => p.Nutriologos)
                .HasForeignKey<Nutriologos>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nutriologos_Usuarios");
        });

        modelBuilder.Entity<Pacientes>(entity =>
        {
            entity.HasKey(e => e.PacienteId).HasName("PK__Paciente__9353C01FBAC531BF");

            entity.HasIndex(e => e.NutriologoId, "IX_Pacientes_NutriologoId");

            entity.HasIndex(e => e.UsuarioId, "UQ__Paciente__2B3DE7B9DB508D53").IsUnique();

            entity.Property(e => e.ObjetivoGeneral).HasMaxLength(200);
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Nutriologo).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.NutriologoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pacientes_Nutriologos");

            entity.HasOne(d => d.Usuario).WithOne(p => p.Pacientes)
                .HasForeignKey<Pacientes>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pacientes_Usuarios");
        });

        modelBuilder.Entity<PlanPorciones>(entity =>
        {
            entity.HasKey(e => e.PlanPorcionId).HasName("PK__PlanPorc__6FDCDB507A5ED57F");

            entity.HasIndex(e => e.PlanId, "IX_PlanPorciones_PlanId");

            entity.HasIndex(e => new { e.PlanId, e.GrupoId, e.TiempoComida }, "UQ_PlanPorciones").IsUnique();

            entity.Property(e => e.CantidadPorciones).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TiempoComida).HasMaxLength(20);

            entity.HasOne(d => d.Grupo).WithMany(p => p.PlanPorciones)
                .HasForeignKey(d => d.GrupoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlanPorciones_Grupos");

            entity.HasOne(d => d.Plan).WithMany(p => p.PlanPorciones)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlanPorciones_Planes");
        });

        modelBuilder.Entity<PlanesNutricionales>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__PlanesNu__755C22B7A63F39BE");

            entity.HasIndex(e => e.PacienteId, "IX_Planes_PacienteId");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Activo");
            entity.Property(e => e.KcalObjetivo).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.Nutriologo).WithMany(p => p.PlanesNutricionales)
                .HasForeignKey(d => d.NutriologoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Planes_Nutriologos");

            entity.HasOne(d => d.Paciente).WithMany(p => p.PlanesNutricionales)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Planes_Pacientes");
        });

        modelBuilder.Entity<RecetaAdaptadaDetalle>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PK__RecetaAd__6E19D6DAAA5A935F");

            entity.HasIndex(e => e.RecetaAdaptadaId, "IX_Detalle_RecetaAdaptadaId");

            entity.Property(e => e.CantidadAjustada).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.FactorEscala).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.UnidadMedida).HasMaxLength(30);

            entity.HasOne(d => d.Alimento).WithMany(p => p.RecetaAdaptadaDetalle)
                .HasForeignKey(d => d.AlimentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalle_Alimentos");

            entity.HasOne(d => d.RecetaAdaptada).WithMany(p => p.RecetaAdaptadaDetalle)
                .HasForeignKey(d => d.RecetaAdaptadaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalle_RecetasAdaptadas");
        });

        modelBuilder.Entity<RecetaIngredientes>(entity =>
        {
            entity.HasKey(e => e.RecetaIngredienteId).HasName("PK__RecetaIn__790578798BB7E8B3");

            entity.HasIndex(e => e.AlimentoId, "IX_RecetaIngredientes_AlimentoId");

            entity.HasIndex(e => e.RecetaId, "IX_RecetaIngredientes_RecetaId");

            entity.Property(e => e.CantidadOriginal).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.PorcionesGrupoOriginal).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnidadMedida).HasMaxLength(30);

            entity.HasOne(d => d.Alimento).WithMany(p => p.RecetaIngredientes)
                .HasForeignKey(d => d.AlimentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecetaIngredientes_Alimentos");

            entity.HasOne(d => d.Receta).WithMany(p => p.RecetaIngredientes)
                .HasForeignKey(d => d.RecetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecetaIngredientes_Recetas");
        });

        modelBuilder.Entity<Recetas>(entity =>
        {
            entity.HasKey(e => e.RecetaId).HasName("PK__Recetas__03D077D840DB64BE");

            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.PorcionesRendimientoOriginal)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TipoPlatillo).HasMaxLength(50);
        });

        modelBuilder.Entity<RecetasAdaptadas>(entity =>
        {
            entity.HasKey(e => e.RecetaAdaptadaId).HasName("PK__RecetasA__6C3BC61B88405966");

            entity.HasIndex(e => e.PacienteId, "IX_RecetasAdaptadas_PacienteId");

            entity.Property(e => e.FechaGeneracion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GeneradaPor).HasMaxLength(20);
            entity.Property(e => e.TiempoComida).HasMaxLength(20);

            entity.HasOne(d => d.Paciente).WithMany(p => p.RecetasAdaptadas)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecetasAdaptadas_Pacientes");

            entity.HasOne(d => d.Plan).WithMany(p => p.RecetasAdaptadas)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_RecetasAdaptadas_Planes");

            entity.HasOne(d => d.Receta).WithMany(p => p.RecetasAdaptadas)
                .HasForeignKey(d => d.RecetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecetasAdaptadas_Recetas");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B80EB6031A");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A193526142F").IsUnique();

            entity.Property(e => e.Contraseña).HasMaxLength(256);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
