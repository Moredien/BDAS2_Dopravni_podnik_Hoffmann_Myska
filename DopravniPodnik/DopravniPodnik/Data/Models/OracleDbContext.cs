using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DopravniPodnik.Data.Models;

public partial class OracleDbContext : DbContext
{
    public OracleDbContext()
    {
    }

    public OracleDbContext(DbContextOptions<OracleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adresy> Adresies { get; set; }

    public virtual DbSet<Foto> Fotos { get; set; }

    public virtual DbSet<Jizdy> Jizdies { get; set; }

    public virtual DbSet<KartyMhd> KartyMhds { get; set; }

    public virtual DbSet<Linky> Linkies { get; set; }

    public virtual DbSet<Logy> Logies { get; set; }

    public virtual DbSet<Platby> Platbies { get; set; }

    public virtual DbSet<Predplatne> Predplatnes { get; set; }

    public virtual DbSet<Ridici> Ridicis { get; set; }

    public virtual DbSet<TypyPredplatneho> TypyPredplatnehos { get; set; }

    public virtual DbSet<TypyUzivatele> TypyUzivateles { get; set; }

    public virtual DbSet<TypyVozidel> TypyVozidels { get; set; }

    public virtual DbSet<Uzivatele> Uzivateles { get; set; }

    public virtual DbSet<Vozidla> Vozidlas { get; set; }

    public virtual DbSet<Zakaznici> Zakaznicis { get; set; }

    public virtual DbSet<Zamestnanci> Zamestnancis { get; set; }

    public virtual DbSet<Zastaveni> Zastavenis { get; set; }

    public virtual DbSet<Zastavky> Zastavkies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=st67028;Password=SQLP455w0rd;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=FEI-SQL3.UPCEUCEBNY.CZ)(PORT=1521))(CONNECT_DATA=(SID=BDAS)));");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("ST67028")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Adresy>(entity =>
        {
            entity.HasKey(e => e.IdAdresy).HasName("ADRESA_PK");

            entity.ToTable("ADRESY");

            entity.Property(e => e.IdAdresy)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ADRESY");
            entity.Property(e => e.CisloPopisne)
                .HasPrecision(5)
                .HasColumnName("CISLO_POPISNE");
            entity.Property(e => e.Mesto)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("MESTO");
            entity.Property(e => e.Ulice)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("ULICE");
        });

        modelBuilder.Entity<Foto>(entity =>
        {
            entity.HasKey(e => e.IdFoto).HasName("FOTO_PK");

            entity.ToTable("FOTO");

            entity.HasIndex(e => e.IdKarty, "FOTO__IDX").IsUnique();

            entity.Property(e => e.IdFoto)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_FOTO");
            entity.Property(e => e.Data)
                .HasColumnType("BLOB")
                .HasColumnName("DATA");
            entity.Property(e => e.DatumPridani)
                .HasColumnType("DATE")
                .HasColumnName("DATUM_PRIDANI");
            entity.Property(e => e.IdKarty)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_KARTY");
            entity.Property(e => e.IdUzivatele)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_UZIVATELE");
            entity.Property(e => e.JmenoSouboru)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("JMENO_SOUBORU");

            entity.HasOne(d => d.IdKartyNavigation).WithOne(p => p.Foto)
                .HasForeignKey<Foto>(d => d.IdKarty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FOTO_KARTA_MHD_FK");

            entity.HasOne(d => d.IdUzivateleNavigation).WithMany(p => p.Fotos)
                .HasForeignKey(d => d.IdUzivatele)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FOTO_UZIVATELE_FK");
        });

        modelBuilder.Entity<Jizdy>(entity =>
        {
            entity.HasKey(e => e.IdJizdy).HasName("JIZDA_PK");

            entity.ToTable("JIZDY");

            entity.Property(e => e.IdJizdy)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_JIZDY");
            entity.Property(e => e.IdLinky)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_LINKY");
            entity.Property(e => e.IdVozidla)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_VOZIDLA");
            entity.Property(e => e.Konec)
                .HasColumnType("DATE")
                .HasColumnName("KONEC");
            entity.Property(e => e.Zacatek)
                .HasColumnType("DATE")
                .HasColumnName("ZACATEK");

            entity.HasOne(d => d.IdLinkyNavigation).WithMany(p => p.Jizdies)
                .HasForeignKey(d => d.IdLinky)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("JIZDA_LINKA_FK");

            entity.HasOne(d => d.IdVozidlaNavigation).WithMany(p => p.Jizdies)
                .HasForeignKey(d => d.IdVozidla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("JIZDA_VOZIDLO_FK");
        });

        modelBuilder.Entity<KartyMhd>(entity =>
        {
            entity.HasKey(e => e.IdKarty).HasName("KARTA_MHD_PK");

            entity.ToTable("KARTY_MHD");

            entity.HasIndex(e => e.IdFoto, "KARTA_MHD__IDX").IsUnique();

            entity.Property(e => e.IdKarty)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_KARTY");
            entity.Property(e => e.IdFoto)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_FOTO");
            entity.Property(e => e.IdZakaznika)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ZAKAZNIKA");
            entity.Property(e => e.PlatnostDo)
                .HasColumnType("DATE")
                .HasColumnName("PLATNOST_DO");
            entity.Property(e => e.PlatnostOd)
                .HasColumnType("DATE")
                .HasColumnName("PLATNOST_OD");
            entity.Property(e => e.Zustatek)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("ZUSTATEK");

            entity.HasOne(d => d.IdFotoNavigation).WithOne(p => p.KartyMhd)
                .HasForeignKey<KartyMhd>(d => d.IdFoto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("KARTA_MHD_FOTO_FK");

            entity.HasOne(d => d.IdZakaznikaNavigation).WithMany(p => p.KartyMhds)
                .HasForeignKey(d => d.IdZakaznika)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("KARTA_MHD_ZAKAZNIK_FK");
        });

        modelBuilder.Entity<Linky>(entity =>
        {
            entity.HasKey(e => e.IdLinky).HasName("LINKA_PK");

            entity.ToTable("LINKY");

            entity.Property(e => e.IdLinky)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_LINKY");
            entity.Property(e => e.CisloLinky)
                .HasPrecision(5)
                .HasColumnName("CISLO_LINKY");
            entity.Property(e => e.Jmeno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("JMENO");
        });

        modelBuilder.Entity<Logy>(entity =>
        {
            entity.HasKey(e => e.IdLogu).HasName("LOG_PK");

            entity.ToTable("LOGY");

            entity.Property(e => e.IdLogu)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_LOGU");
            entity.Property(e => e.Cas)
                .HasColumnType("DATE")
                .HasColumnName("CAS");
            entity.Property(e => e.NovaHodnota)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("NOVA_HODNOTA");
            entity.Property(e => e.Operace)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OPERACE");
            entity.Property(e => e.StaraHodnota)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("STARA_HODNOTA");
            entity.Property(e => e.Tabulka)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("TABULKA");
            entity.Property(e => e.Uzivatel)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("UZIVATEL");
        });

        modelBuilder.Entity<Platby>(entity =>
        {
            entity.HasKey(e => e.IdPlatby).HasName("PLATBA_PK");

            entity.ToTable("PLATBY");

            entity.Property(e => e.IdPlatby)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_PLATBY");
            entity.Property(e => e.CasPlatby)
                .HasColumnType("DATE")
                .HasColumnName("CAS_PLATBY");
            entity.Property(e => e.IdZakaznika)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ZAKAZNIKA");
            entity.Property(e => e.VysePlatby)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("VYSE_PLATBY");

            entity.HasOne(d => d.IdZakaznikaNavigation).WithMany(p => p.Platbies)
                .HasForeignKey(d => d.IdZakaznika)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PLATBA_ZAKAZNIK_FK");
        });

        modelBuilder.Entity<Predplatne>(entity =>
        {
            entity.HasKey(e => e.IdPredplatneho).HasName("PREDPLATNE_PK");

            entity.ToTable("PREDPLATNE");

            entity.Property(e => e.IdPredplatneho)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_PREDPLATNEHO");
            entity.Property(e => e.Do)
                .HasColumnType("DATE")
                .HasColumnName("DO");
            entity.Property(e => e.IdKarty)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_KARTY");
            entity.Property(e => e.IdTypPredplatneho)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_TYP_PREDPLATNEHO");
            entity.Property(e => e.Od)
                .HasColumnType("DATE")
                .HasColumnName("OD");

            entity.HasOne(d => d.IdKartyNavigation).WithMany(p => p.Predplatnes)
                .HasForeignKey(d => d.IdKarty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PREDPLATNE_KARTA_MHD_FK");

            entity.HasOne(d => d.IdTypPredplatnehoNavigation).WithMany(p => p.Predplatnes)
                .HasForeignKey(d => d.IdTypPredplatneho)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PREDPLATNE_TYP_PREDPLATNEHO_FK");
        });

        modelBuilder.Entity<Ridici>(entity =>
        {
            entity.HasKey(e => e.IdRidice).HasName("RIDIC_PK");

            entity.ToTable("RIDICI");

            entity.Property(e => e.IdRidice)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_RIDICE");
            entity.Property(e => e.Jmeno)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("JMENO");
            entity.Property(e => e.Prijmeni)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("PRIJMENI");

            entity.HasMany(d => d.IdJizdies).WithMany(p => p.IdRidices)
                .UsingEntity<Dictionary<string, object>>(
                    "JizdyRidicu",
                    r => r.HasOne<Jizdy>().WithMany()
                        .HasForeignKey("IdJizdy")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("JIZDY_RIDICU_JIZDA_FK"),
                    l => l.HasOne<Ridici>().WithMany()
                        .HasForeignKey("IdRidice")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("JIZDY_RIDICU_RIDIC_FK"),
                    j =>
                    {
                        j.HasKey("IdRidice", "IdJizdy").HasName("JIZDY_RIDICU");
                        j.ToTable("JIZDY_RIDICU");
                        j.IndexerProperty<decimal>("IdRidice")
                            .HasColumnType("NUMBER(32)")
                            .HasColumnName("ID_RIDICE");
                        j.IndexerProperty<decimal>("IdJizdy")
                            .HasColumnType("NUMBER(32)")
                            .HasColumnName("ID_JIZDY");
                    });
        });

        modelBuilder.Entity<TypyPredplatneho>(entity =>
        {
            entity.HasKey(e => e.IdTypPredplatneho).HasName("TYP_PREDPLATNEHO_PK");

            entity.ToTable("TYPY_PREDPLATNEHO");

            entity.Property(e => e.IdTypPredplatneho)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_TYP_PREDPLATNEHO");
            entity.Property(e => e.Cena)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("CENA");
            entity.Property(e => e.Jmeno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("JMENO");
        });

        modelBuilder.Entity<TypyUzivatele>(entity =>
        {
            entity.HasKey(e => e.IdTypUzivatele).HasName("TYP_UZIVATELE_PK");

            entity.ToTable("TYPY_UZIVATELE");

            entity.Property(e => e.IdTypUzivatele)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_TYP_UZIVATELE");
            entity.Property(e => e.Nazev)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NAZEV");
        });

        modelBuilder.Entity<TypyVozidel>(entity =>
        {
            entity.HasKey(e => e.IdTypVozidla).HasName("TYP_VOZIDLA_PK");

            entity.ToTable("TYPY_VOZIDEL");

            entity.Property(e => e.IdTypVozidla)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_TYP_VOZIDLA");
            entity.Property(e => e.Nazev)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAZEV");
            entity.Property(e => e.Znacka)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ZNACKA");
        });

        modelBuilder.Entity<Uzivatele>(entity =>
        {
            entity.HasKey(e => e.IdUzivatele).HasName("UZIVATEL_PK");

            entity.ToTable("UZIVATELE");

            entity.HasIndex(e => e.UzivatelskeJmeno, "UZIVATELE__UN").IsUnique();

            entity.Property(e => e.IdUzivatele)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_UZIVATELE");
            entity.Property(e => e.CasZalozeni)
                .HasColumnType("DATE")
                .HasColumnName("CAS_ZALOZENI");
            entity.Property(e => e.DatumNarozeni)
                .HasColumnType("DATE")
                .HasColumnName("DATUM_NAROZENI");
            entity.Property(e => e.Heslo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("HESLO");
            entity.Property(e => e.IdAdresy)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ADRESY");
            entity.Property(e => e.IdTypUzivatele)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_TYP_UZIVATELE");
            entity.Property(e => e.Jmeno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("JMENO");
            entity.Property(e => e.Prijmeni)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PRIJMENI");
            entity.Property(e => e.UzivatelskeJmeno)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("UZIVATELSKE_JMENO");

            entity.HasOne(d => d.IdAdresyNavigation).WithMany(p => p.Uzivateles)
                .HasForeignKey(d => d.IdAdresy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UZIVATEL_ADRESA_FK");

            entity.HasOne(d => d.IdTypUzivateleNavigation).WithMany(p => p.Uzivateles)
                .HasForeignKey(d => d.IdTypUzivatele)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UZIVATEL_TYP_UZIVATELE_FK");
        });

        modelBuilder.Entity<Vozidla>(entity =>
        {
            entity.HasKey(e => e.IdVozidla).HasName("VOZIDLO_PK");

            entity.ToTable("VOZIDLA");

            entity.Property(e => e.IdVozidla)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_VOZIDLA");
            entity.Property(e => e.IdTypVozidla)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_TYP_VOZIDLA");

            entity.HasOne(d => d.IdTypVozidlaNavigation).WithMany(p => p.Vozidlas)
                .HasForeignKey(d => d.IdTypVozidla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VOZIDLO_TYP_VOZIDLA_FK");
        });

        modelBuilder.Entity<Zakaznici>(entity =>
        {
            entity.HasKey(e => e.IdZakaznika).HasName("ZAKAZNIK_PK");

            entity.ToTable("ZAKAZNICI");

            entity.HasIndex(e => e.IdUzivatele, "ZAKAZNIK__IDX").IsUnique();

            entity.Property(e => e.IdZakaznika)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ZAKAZNIKA");
            entity.Property(e => e.IdUzivatele)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_UZIVATELE");

            entity.HasOne(d => d.IdUzivateleNavigation).WithOne(p => p.Zakaznici)
                .HasForeignKey<Zakaznici>(d => d.IdUzivatele)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ZAKAZNIK_UZIVATEL_FK");
        });

        modelBuilder.Entity<Zamestnanci>(entity =>
        {
            entity.HasKey(e => e.IdZamestnance).HasName("ZAMESTNANEC_PK");

            entity.ToTable("ZAMESTNANCI");

            entity.HasIndex(e => e.IdUzivatele, "ZAMESTNANEC__IDX").IsUnique();

            entity.Property(e => e.IdZamestnance)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ZAMESTNANCE");
            entity.Property(e => e.IdNadrizeneho)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_NADRIZENEHO");
            entity.Property(e => e.IdUzivatele)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_UZIVATELE");
            entity.Property(e => e.Plat)
                .HasPrecision(6)
                .HasColumnName("PLAT");
            entity.Property(e => e.PlatnostUvazkuDo)
                .HasColumnType("DATE")
                .HasColumnName("PLATNOST_UVAZKU_DO");

            entity.HasOne(d => d.IdNadrizenehoNavigation).WithMany(p => p.InverseIdNadrizenehoNavigation)
                .HasForeignKey(d => d.IdNadrizeneho)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ZAMESTNANEC_ZAMESTNANEC_FK");

            entity.HasOne(d => d.IdUzivateleNavigation).WithOne(p => p.Zamestnanci)
                .HasForeignKey<Zamestnanci>(d => d.IdUzivatele)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ZAMESTNANEC_UZIVATEL_FK");
        });

        modelBuilder.Entity<Zastaveni>(entity =>
        {
            entity.HasKey(e => e.IdZastaveni).HasName("ZASTAVENI_PK");

            entity.ToTable("ZASTAVENI");

            entity.Property(e => e.IdZastaveni)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ZASTAVENI");
            entity.Property(e => e.IdLinky)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_LINKY");
            entity.Property(e => e.IdZastavky)
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ZASTAVKY");
            entity.Property(e => e.Odjezd)
                .HasColumnType("DATE")
                .HasColumnName("ODJEZD");

            entity.HasOne(d => d.IdLinkyNavigation).WithMany(p => p.Zastavenis)
                .HasForeignKey(d => d.IdLinky)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ZASTAVENI_LINKA_FK");

            entity.HasOne(d => d.IdZastavkyNavigation).WithMany(p => p.Zastavenis)
                .HasForeignKey(d => d.IdZastavky)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ZASTAVENI_ZASTAVKA_FK");
        });

        modelBuilder.Entity<Zastavky>(entity =>
        {
            entity.HasKey(e => e.IdZastavky).HasName("ZASTAVKA_PK");

            entity.ToTable("ZASTAVKY");

            entity.Property(e => e.IdZastavky)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(32)")
                .HasColumnName("ID_ZASTAVKY");
            entity.Property(e => e.Jmeno)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("JMENO");
        });
        modelBuilder.HasSequence("ADRESY_ID_ADRESY_SEQ");
        modelBuilder.HasSequence("FOTO_ID_FOTO_SEQ");
        modelBuilder.HasSequence("JIZDY_ID_JIZDY_SEQ");
        modelBuilder.HasSequence("KARTY_ID_KARTY_SEQ");
        modelBuilder.HasSequence("LINKY_ID_LINKY_SEQ");
        modelBuilder.HasSequence("LOGY_ID_LOGU_SEQ");
        modelBuilder.HasSequence("PLATBY_ID_PLATBY_SEQ");
        modelBuilder.HasSequence("PREDPLATNE_ID_PREDPLATNEHO_SEQ");
        modelBuilder.HasSequence("RIDICI_ID_RIDICE_SEQ");
        modelBuilder.HasSequence("TYPY_PREDPLATNEHO_ID_TYP_PREDP");
        modelBuilder.HasSequence("TYPY_UZIVATELE_ID_TYP_UZIVATEL");
        modelBuilder.HasSequence("TYPY_VOZIDEL_ID_TYP_VOZIDLA_SE");
        modelBuilder.HasSequence("UZIVATELE_ID_UZIVATELE_SEQ");
        modelBuilder.HasSequence("VOZDILA_ID_VOZIDLA_SEQ");
        modelBuilder.HasSequence("ZAKAZNICI_ID_ZAKAZNIKA_SEQ");
        modelBuilder.HasSequence("ZAMESTANACI_ID_ZAMESTNANCE_SEQ");
        modelBuilder.HasSequence("ZASTAVENI_ID_ZASTAVENI_SEQ");
        modelBuilder.HasSequence("ZASTAVKY_ID_ZASTAVKY_SEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
