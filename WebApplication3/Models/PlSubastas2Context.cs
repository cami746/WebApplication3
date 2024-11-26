using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models;

public partial class PlSubastas2Context : DbContext
{
    public PlSubastas2Context()
    {
    }

    public PlSubastas2Context(DbContextOptions<PlSubastas2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaProducto> CategoriaProductos { get; set; }

    public virtual DbSet<ImagenesProducto> ImagenesProductos { get; set; }

    public virtual DbSet<Inscripcion> Inscripcions { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Puja> Pujas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Subastum> Subasta { get; set; }

    public virtual DbSet<SubcategoriaProducto> SubcategoriaProductos { get; set; }

    public virtual DbSet<TipoDeSubastum> TipoDeSubasta { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRol> UsuarioRols { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-D9IF75B\\SQL2019;Database=PlSubastas2;User Id=DESKTOP-D9IF75B\\T403-22;Password='';Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.CategoriaProductoId).HasName("PK__Categori__7C808EBCDF0A59F5");

            entity.ToTable("CategoriaProducto");

            entity.Property(e => e.ImagenUrl).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<ImagenesProducto>(entity =>
        {
            entity.HasKey(e => e.ImagenProductoId).HasName("PK__Imagenes__7A1FE86D89DFD633");

            entity.ToTable("ImagenesProducto");

            entity.Property(e => e.EsPrincipal).HasDefaultValue(false);
            entity.Property(e => e.ImagenUrl).HasMaxLength(255);

            entity.HasOne(d => d.Producto).WithMany(p => p.ImagenesProductos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImagenesP__Produ__25869641");
        });

        modelBuilder.Entity<Inscripcion>(entity =>
        {
            entity.HasKey(e => e.InscripcionId).HasName("PK__Inscripc__168316B9B4D0AEFA");

            entity.ToTable("Inscripcion");

            entity.Property(e => e.FechaInscripcion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Subasta).WithMany(p => p.Inscripcions)
                .HasForeignKey(d => d.SubastaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inscripci__Subas__2A4B4B5E");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Inscripcions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inscripci__Usuar__29572725");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pago__F00B61382613961D");

            entity.ToTable("Pago");

            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Inscripcion).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.InscripcionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pago__Inscripcio__31EC6D26");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AEA327C57AC5");

            entity.ToTable("Producto");

            entity.Property(e => e.ImagenPrincipalUrl).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CategoriaProducto).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Producto__Catego__20C1E124");

            entity.HasOne(d => d.Subasta).WithMany(p => p.Productos)
                .HasForeignKey(d => d.SubastaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Producto__Subast__21B6055D");
        });

        modelBuilder.Entity<Puja>(entity =>
        {
            entity.HasKey(e => e.PujaId).HasName("PK__Puja__0F67A0DC4071D9B3");

            entity.ToTable("Puja");

            entity.Property(e => e.FechaPuja)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Inscripcion).WithMany(p => p.Pujas)
                .HasForeignKey(d => d.InscripcionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puja__Inscripcio__2E1BDC42");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__rol__6ABCB5E092BBF0E7");

            entity.ToTable("rol");

            entity.HasIndex(e => e.Nombre, "UQ__rol__72AFBCC6B66506DE").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Subastum>(entity =>
        {
            entity.HasKey(e => e.SubastaId).HasName("PK__Subasta__46C5CE1ADC56D5B0");

            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.PrecioFinal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PrecioInicial).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.TipoDeSubasta).WithMany(p => p.Subasta)
                .HasForeignKey(d => d.TipoDeSubastaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subasta__TipoDeS__1920BF5C");
        });

        modelBuilder.Entity<SubcategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.SubcategoriaProductoId).HasName("PK__Subcateg__D13D15399F266A5D");

            entity.ToTable("SubcategoriaProducto");

            entity.Property(e => e.ImagenUrl).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.CategoriaProducto).WithMany(p => p.SubcategoriaProductos)
                .HasForeignKey(d => d.CategoriaProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subcatego__Categ__1DE57479");
        });

        modelBuilder.Entity<TipoDeSubastum>(entity =>
        {
            entity.HasKey(e => e.TipoDeSubastaId).HasName("PK__TipoDeSu__9D692E09D0137BB7");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuario__4E3E04AD8C83BF3F");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "UQ__usuario__AB6E6164F6740121").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.UltimoAcceso)
                .HasColumnType("datetime")
                .HasColumnName("ultimo_acceso");
        });

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => e.UsuarioRolId).HasName("PK__UsuarioR__C869CDCA86AAEBB6");

            entity.ToTable("UsuarioRol");

            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioRo__IdRol__36B12243");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioRo__IdUsu__35BCFE0A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
