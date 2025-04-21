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

    public virtual DbSet<NameApprover> NameApprovers { get; set; }

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
            entity.Property(e => e._1universityactivities)
                .HasDefaultValue(0)
                .HasColumnName("1universityactivities");
            entity.Property(e => e._1universityactivitiesSt)
                .HasDefaultValue(0)
                .HasColumnName("1universityactivitiesSt");
            entity.Property(e => e._211publicspiritactivities)
                .HasDefaultValue(0)
                .HasColumnName("211Publicspiritactivities");
            entity.Property(e => e._211publicspiritactivitiesSt)
                .HasDefaultValue(0)
                .HasColumnName("211PublicspiritactivitiesSt");
            entity.Property(e => e._212moral)
                .HasDefaultValue(0)
                .HasColumnName("212Moral");
            entity.Property(e => e._212moralSt)
                .HasDefaultValue(0)
                .HasColumnName("212MoralSt");
            entity.Property(e => e._221competencybuildingactivities)
                .HasDefaultValue(0)
                .HasColumnName("221Competencybuildingactivities");
            entity.Property(e => e._221competencybuildingactivitiesSt)
                .HasDefaultValue(0)
                .HasColumnName("221CompetencybuildingactivitiesSt");
            entity.Property(e => e._222itskills)
                .HasDefaultValue(0)
                .HasColumnName("222ITskills");
            entity.Property(e => e._222itskillsSt)
                .HasDefaultValue(0)
                .HasColumnName("222ITskillsSt");
            entity.Property(e => e._223developing)
                .HasDefaultValue(0)
                .HasColumnName("223Developing");
            entity.Property(e => e._223developingSt)
                .HasDefaultValue(0)
                .HasColumnName("223DevelopingSt");
            entity.Property(e => e._231democratic)
                .HasDefaultValue(0)
                .HasColumnName("231democratic");
            entity.Property(e => e._231democraticSt)
                .HasDefaultValue(0)
                .HasColumnName("231democraticSt");
            entity.Property(e => e._232relationships)
                .HasDefaultValue(0)
                .HasColumnName("232relationships");
            entity.Property(e => e._232relationshipsSt)
                .HasDefaultValue(0)
                .HasColumnName("232relationshipsSt");
            entity.Property(e => e._24health)
                .HasDefaultValue(0)
                .HasColumnName("24Health");
            entity.Property(e => e._24healthSt)
                .HasDefaultValue(0)
                .HasColumnName("24HealthSt");
            entity.Property(e => e._31activities)
                .HasDefaultValue(0)
                .HasColumnName("31activities");
            entity.Property(e => e._31activitiesSt)
                .HasDefaultValue(0)
                .HasColumnName("31activitiesSt");
            entity.Property(e => e._32social)
                .HasDefaultValue(0)
                .HasColumnName("32Social");
            entity.Property(e => e._32socialSt)
                .HasDefaultValue(0)
                .HasColumnName("32SocialSt");
        });

        modelBuilder.Entity<NameApprover>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Name_App__3214EC07DB7575EC");

            entity.ToTable("Name_Approver");

            entity.Property(e => e._10Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("10_Approve");
            entity.Property(e => e._11Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("11_Approve");
            entity.Property(e => e._1Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("1_Approve");
            entity.Property(e => e._2Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("2_Approve");
            entity.Property(e => e._3Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("3_Approve");
            entity.Property(e => e._4Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("4_Approve");
            entity.Property(e => e._5Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("5_Approve");
            entity.Property(e => e._6Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("6_Approve");
            entity.Property(e => e._7Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("7_Approve");
            entity.Property(e => e._8Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("8_Approve");
            entity.Property(e => e._9Approve)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("9_Approve");
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
