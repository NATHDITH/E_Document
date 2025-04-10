using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace E_Document.Models;

public partial class AutoPdfContext : DbContext
{
    public AutoPdfContext()
    {
    }

    public AutoPdfContext(DbContextOptions<AutoPdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Approval> Approvals { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentDetail> DocumentDetails { get; set; }

    public virtual DbSet<Signature> Signatures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PC_NATDIT\\SQLEXPRESS;Database=AutoPDF;User Id=sa;Password=123456;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Approval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC07D8301055");

            entity.Property(e => e.ApprovedAt).HasColumnType("datetime");
            entity.Property(e => e.LastApprover).IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Approver).WithMany(p => p.Approvals)
                .HasForeignKey(d => d.ApproverId)
                .HasConstraintName("FK__Approvals__Appro__656C112C");

            entity.HasOne(d => d.Document).WithMany(p => p.Approvals)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK__Approvals__Docum__6477ECF3");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07A81A7B0C");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.UploadedBy)
                .HasConstraintName("FK__Documents__Uploa__5FB337D6");
        });

        modelBuilder.Entity<DocumentDetail>(entity =>
        {
            entity.HasKey(e => e.ProjectCode).HasName("PK_DocumentDetail2");

            entity.ToTable("DocumentDetail");

            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Accumulatedmoneytext)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.DocNo)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Location)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.Maintenancemoneytext)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Othermoneytext)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.ProjectName)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e._1universityactivities).HasColumnName("1universityactivities");
            entity.Property(e => e._1universityactivitiesSt).HasColumnName("1universityactivitiesST");
            entity.Property(e => e._211publicspiritactivities).HasColumnName("211Publicspiritactivities");
            entity.Property(e => e._211publicspiritactivitiesSt).HasColumnName("211PublicspiritactivitiesST");
            entity.Property(e => e._212moral).HasColumnName("212Moral");
            entity.Property(e => e._212moralSt).HasColumnName("212MoralST");
            entity.Property(e => e._221competencybuildingactivities).HasColumnName("221Competencybuildingactivities");
            entity.Property(e => e._221competencybuildingactivitiesSt).HasColumnName("221CompetencybuildingactivitiesST");
            entity.Property(e => e._222itskills).HasColumnName("222ITskills");
            entity.Property(e => e._222itskillsSt).HasColumnName("222ITskillsST");
            entity.Property(e => e._223developing).HasColumnName("223Developing");
            entity.Property(e => e._223developingSt).HasColumnName("223DevelopingST");
            entity.Property(e => e._231democratic).HasColumnName("231democratic");
            entity.Property(e => e._231democraticSt).HasColumnName("231democraticST");
            entity.Property(e => e._232relationships).HasColumnName("232relationships");
            entity.Property(e => e._232relationshipsSt).HasColumnName("232relationshipsST");
            entity.Property(e => e._24health).HasColumnName("24Health");
            entity.Property(e => e._24healthSt).HasColumnName("24HealthST");
            entity.Property(e => e._31activities).HasColumnName("31activities");
            entity.Property(e => e._31activitiesSt).HasColumnName("31activitiesST");
            entity.Property(e => e._32social).HasColumnName("32Social");
            entity.Property(e => e._32socialSt).HasColumnName("32SocialST");
        });

        modelBuilder.Entity<Signature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Signatur__3214EC0709B905A0");

            entity.Property(e => e.SignaturePath).HasMaxLength(255);
            entity.Property(e => e.SignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Approver).WithMany(p => p.Signatures)
                .HasForeignKey(d => d.ApproverId)
                .HasConstraintName("FK__Signature__Appro__693CA210");

            entity.HasOne(d => d.Document).WithMany(p => p.Signatures)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK__Signature__Docum__6A30C649");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC078E43E8BF");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4BB11A8C1").IsUnique();

            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
