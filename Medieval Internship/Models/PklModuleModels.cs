namespace Medieval_Internship.Models;

public enum AttendanceStatus
{
    Hadir,
    Izin,
    Sakit,
    Alpa
}

public enum ValidationStatus
{
    Menunggu,
    Disetujui,
    Ditolak
}

public class AttendanceRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
    public string CheckInTime { get; set; } = "-";
    public string CheckOutTime { get; set; } = "-";
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Hadir;
    public string LocationName { get; set; } = "PT Telkom Indonesia Landmark Tower";
    public string Coordinates { get; set; } = "-6.2297, 106.8164 (Valid GPS)";
    public string Notes { get; set; } = string.Empty;
    public string PhotoEvidence { get; set; } = "selfie_presensi.jpg";
    
    // Validasi Pembimbing
    public ValidationStatus IndustriValidation { get; set; } = ValidationStatus.Menunggu;
    public string IndustriValidatorName { get; set; } = string.Empty;
    public string IndustriValidationNotes { get; set; } = string.Empty;
    public DateTime? IndustriValidatedAt { get; set; }

    public ValidationStatus GuruValidation { get; set; } = ValidationStatus.Menunggu;
    public string GuruValidatorName { get; set; } = string.Empty;
    public string GuruValidationNotes { get; set; } = string.Empty;
    public DateTime? GuruValidatedAt { get; set; }

    public string DateFormatted => Date.ToString("dd MMM yyyy");
    public string StatusBadgeColor => Status switch
    {
        AttendanceStatus.Hadir => "#10B981",
        AttendanceStatus.Izin => "#3B82F6",
        AttendanceStatus.Sakit => "#F59E0B",
        AttendanceStatus.Alpa => "#EF4444",
        _ => "#6B7280"
    };

    public string ValidationSummary
    {
        get
        {
            if (IndustriValidation == ValidationStatus.Disetujui && GuruValidation == ValidationStatus.Disetujui)
                return "Disetujui Penuh (Industri & Guru)";
            if (IndustriValidation == ValidationStatus.Disetujui)
                return "Disetujui Industri • Menunggu Guru";
            if (GuruValidation == ValidationStatus.Disetujui)
                return "Disetujui Guru • Menunggu Industri";
            if (IndustriValidation == ValidationStatus.Ditolak || GuruValidation == ValidationStatus.Ditolak)
                return "Ditolak Pembimbing";
            return "Menunggu Validasi";
        }
    }
}

public enum JournalStatus
{
    MenungguReview,
    DisetujuiIndustri,
    DisetujuiGuru,
    DisetujuiPenuh,
    PerluRevisi,
    Ditolak
}

public class DailyJournalRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
    public string ActivityTitle { get; set; } = string.Empty;
    public string WorkDescription { get; set; } = string.Empty;
    public string ToolsUsed { get; set; } = string.Empty;
    public string Obstacles { get; set; } = string.Empty;
    public string Solutions { get; set; } = string.Empty;
    public string AttachmentProof { get; set; } = "dokumentasi_kegiatan.png";
    public JournalStatus Status { get; set; } = JournalStatus.MenungguReview;
    public string MentorFeedback { get; set; } = string.Empty;
    public string GuruFeedback { get; set; } = string.Empty;
    public DateTime? ReviewedAt { get; set; }

    public string DateFormatted => Date.ToString("dd MMM yyyy");
    public string StatusText => Status switch
    {
        JournalStatus.MenungguReview => "Menunggu Review",
        JournalStatus.DisetujuiIndustri => "Disetujui Industri",
        JournalStatus.DisetujuiGuru => "Disetujui Guru",
        JournalStatus.DisetujuiPenuh => "Disetujui Penuh",
        JournalStatus.PerluRevisi => "Perlu Revisi",
        JournalStatus.Ditolak => "Ditolak",
        _ => "Draft"
    };

    public string StatusBadgeColor => Status switch
    {
        JournalStatus.DisetujuiPenuh or JournalStatus.DisetujuiIndustri or JournalStatus.DisetujuiGuru => "#10B981",
        JournalStatus.MenungguReview => "#F59E0B",
        JournalStatus.PerluRevisi => "#6366F1",
        JournalStatus.Ditolak => "#EF4444",
        _ => "#6B7280"
    };
}

public class SupervisionVisitRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string GuruId { get; set; } = string.Empty;
    public string GuruName { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; } = DateTime.Today;
    public string VisitType { get; set; } = "Onsite (Kunjungan Langsung)"; // Onsite / Daring
    public string StudentsMet { get; set; } = string.Empty;
    public string SupervisionNotes { get; set; } = string.Empty;
    public string IndustryFeedback { get; set; } = string.Empty;
    public string FollowUpRecommendations { get; set; } = string.Empty;
    public string DocumentationProof { get; set; } = "foto_kunjungan_monev.jpg";
    public string OverallAssessment { get; set; } = "Sangat Baik (A)";

    public string VisitDateFormatted => VisitDate.ToString("dd MMMM yyyy");
}

public enum AnnouncementPriority
{
    Info,
    Penting,
    Pengingat,
    Mendesak
}

public class AnnouncementRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = "Koordinator Hubin SMK";
    public string AuthorRole { get; set; } = "Hubungan Industri";
    public DateTime CreatedDate { get; set; } = DateTime.Today;
    public string TargetAudience { get; set; } = "Semua"; // Semua, Siswa, Guru, Industri, Monitor
    public AnnouncementPriority Priority { get; set; } = AnnouncementPriority.Info;
    public string AttachmentName { get; set; } = string.Empty;
    public bool IsPinned { get; set; } = false;

    public string DateFormatted => CreatedDate.ToString("dd MMM yyyy");
    public string PriorityBadgeColor => Priority switch
    {
        AnnouncementPriority.Mendesak => "#DC2626",
        AnnouncementPriority.Penting => "#EA580C",
        AnnouncementPriority.Pengingat => "#2563EB",
        _ => "#0D9488"
    };
}

public enum ReportStatus
{
    MenungguReview,
    Tahan,
    Setuju,
    Revisi,
    PerluRevisi = Revisi,
    DisetujuiPembimbing,
    LulusSidang
}

public class FinalReportRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string Nisn { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string MentorName { get; set; } = string.Empty;
    public string ReportTitle { get; set; } = string.Empty;
    public string Abstract { get; set; } = string.Empty;
    public string FileName { get; set; } = "Laporan_Akhir_PKL_Lengkap.pdf";
    public string PresentationLink { get; set; } = "https://slide.smkn1.sch.id/sidang-pkl";
    public DateTime UploadDate { get; set; } = DateTime.Today;
    public ReportStatus Status { get; set; } = ReportStatus.MenungguReview;

    // Status Pembimbing Industri (FR-RPT-03: Tahan, Setuju, Revisi)
    public string IndustriStatus { get; set; } = "Menunggu";
    public string CatatanIndustri { get; set; } = "Kemampuan problem solving cepat dan adaptif dengan stack cloud tim.";
    public double NilaiTeknisIndustri { get; set; } = 92.0;
    public double NilaiSoftSkillIndustri { get; set; } = 95.0;
    public double NilaiDisiplinIndustri { get; set; } = 94.0;
    public double RataRataIndustri => (NilaiTeknisIndustri + NilaiSoftSkillIndustri + NilaiDisiplinIndustri) / 3.0;

    // Nilai Guru Pembimbing (0-100)
    public double NilaiLaporanGuru { get; set; } = 90.0;
    public double NilaiSidangGuru { get; set; } = 91.0;
    public string CatatanGuru { get; set; } = "Sistematika penulisan Bab 4 sesuai kaidah ilmiah vokasi.";
    public string RevisionNotes { get; set; } = string.Empty;
    public double RataRataGuru => (NilaiLaporanGuru + NilaiSidangGuru) / 2.0;

    // Nilai Akhir Gabungan (Bobot 60% Industri, 40% Sekolah)
    public double NilaiAkhir => Math.Round((RataRataIndustri * 0.6) + (RataRataGuru * 0.4), 1);
    public string Predikat => NilaiAkhir >= 90 ? "A (Sangat Baik)" : NilaiAkhir >= 80 ? "B (Baik)" : "C (Cukup)";

    public string StatusText => Status switch
    {
        ReportStatus.Setuju => "Setuju (Industri)",
        ReportStatus.Tahan => "Tahan (Industri)",
        ReportStatus.Revisi => "Revisi (Industri)",
        ReportStatus.DisetujuiPembimbing => "Disetujui Pembimbing",
        ReportStatus.LulusSidang => "Lulus Sidang PKL",
        _ => "Menunggu Review"
    };

    public string StatusColor => Status switch
    {
        ReportStatus.LulusSidang or ReportStatus.Setuju => "#10B981",
        ReportStatus.DisetujuiPembimbing => "#059669",
        ReportStatus.Revisi => "#DC2626",
        ReportStatus.Tahan => "#6366F1",
        _ => "#D97706"
    };
}

// FR-MST-03: Jurusan (Vocational Major)
public class VocationalMajorModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string MajorCode { get; set; } = string.Empty; // Mandatory, Unique
    public string MajorName { get; set; } = string.Empty; // Mandatory, Unique
    public string Description { get; set; } = string.Empty; // Nullable
    public int Status { get; set; } = 1; // 1 = Active, 0 = Inactive
    public string StatusText => Status == 1 ? "Active" : "Inactive";
}

// FR-MST-02: Kelas (Classroom)
public class DepartmentClassModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string VocationalId { get; set; } = string.Empty; // Mandatory
    public string MajorName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty; // Unique per TA
    public string AcademicYear { get; set; } = "2026/2027";
    public string HomeroomTeacher { get; set; } = string.Empty;
    public int Capacity { get; set; } = 36;
    public int TotalStudents { get; set; } = 36;
}

// FR-MST-04: Perusahaan (Company Partner)
public class CompanyPartnerModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string Name { get; set; } = string.Empty;
    public string IndustrySector { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; } = -6.2297;
    public double Longitude { get; set; } = 106.8164;
    public string ContactPerson { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = "logo_telkom.png";
    public int Status { get; set; } = 1; // 1 = Active, 0 = Inactive
    public string StatusText => Status == 1 ? "Active" : "Inactive";
    public int Quota { get; set; } = 10;
    public int Occupied { get; set; } = 4;
    public string MouStatus { get; set; } = "Aktif (Hingga 2028)";
}

// FR-MST-01: Siswa (Student)
public class MasterStudentModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string UserId { get; set; } = string.Empty; // Unique
    public string ClassroomId { get; set; } = string.Empty; // Mandatory
    public string CompanyId { get; set; } = string.Empty; // Mandatory
    public string InternalMentorId { get; set; } = string.Empty; // Mandatory
    public string ExternalMentorId { get; set; } = string.Empty; // Mandatory
    public string Nisn { get; set; } = string.Empty; // Unique
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = "L"; // L / P
    public DateTime BirthDate { get; set; } = new(2008, 5, 12);
    public string Address { get; set; } = "Jakarta Pusat";
    public string PhoneNumber { get; set; } = "081234567890"; // Unique
    public DateTime StartDate { get; set; } = new(2026, 7, 1);
    public DateTime EndDate { get; set; } = new(2026, 9, 30);
    public string Status { get; set; } = "Active"; // "Active", "Internship", "Completed", "Inactive"

    // Helper display properties
    public string ClassName { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string InternalMentorName { get; set; } = string.Empty;
    public string ExternalMentorName { get; set; } = string.Empty;
    public string PeriodId { get; set; } = "GEL-1-2026";
    public string StatusPkl { get; set; } = "Active";
    public double AttendanceRate { get; set; } = 96.5;
    public int JournalCount { get; set; } = 42;
}

// FR-MST-05: Pembimbing (Mentor)
public class MasterMentorModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
    public string MentorName { get; set; } = string.Empty;
    public string Nip { get; set; } = string.Empty;
    public string MentorType { get; set; } = "Internal"; // Internal (Guru) / External (Industri)
    public string CompanyName { get; set; } = string.Empty;
    public string AssignedStudentName { get; set; } = string.Empty;
    public string AssignedStudentNisn { get; set; } = string.Empty;
    public string AssignedClassName { get; set; } = string.Empty;
    public string AssignedMajorCode { get; set; } = string.Empty;
}

public class PklPeriodModel
{
    public string Id { get; set; } = "GEL-1-2026";
    public string Name { get; set; } = "Gelombang 1 - TA 2026/2027";
    public DateTime StartDate { get; set; } = new(2026, 7, 1);
    public DateTime EndDate { get; set; } = new(2026, 9, 30);
    public bool IsActive { get; set; } = true;

    public string DateRangeFormatted => $"{StartDate:dd MMM yyyy} s/d {EndDate:dd MMM yyyy}";
}

// FR-APK-01: Application Settings
public class SchoolConfigModel
{
    public string ApplicationName { get; set; } = "PKL Monitor Vokasi";
    public string ApplicationDescription { get; set; } = "Sistem Monitoring dan Pelaporan Praktik Kerja Lapangan Vokasi Terintegrasi";
    public string SchoolName { get; set; } = "SMK Negeri 1 Jakarta";
    public string Npsn { get; set; } = "20101234";
    public string Accreditation { get; set; } = "A (Unggul)";
    public string Address { get; set; } = "Jl. Budi Utomo No. 7, Pasar Baru, Sawah Besar, Jakarta Pusat";
    public string PrincipalName { get; set; } = "Dra. Hj. Nurlaela, M.Pd";
    public string HubinCoordinator { get; set; } = "Drs. H. Mulyadi, M.M";
    public string Phone { get; set; } = "(021) 3813630";
    public string Email { get; set; } = "info@smkn1jakarta.sch.id";
    public string Website { get; set; } = "https://smkn1jakarta.sch.id";
    public string Instagram { get; set; } = "@smkn1_jakarta";
    public string Youtube { get; set; } = "SMKN 1 Jakarta Official";
    public string HelpdeskContact { get; set; } = "+62 812-3456-7890 (Helpdesk PKL Hubin)";
    
    // FR-APK-01 specific settings
    public string Logo { get; set; } = "logo_smk.png";
    public string Favicon { get; set; } = "favicon.ico";
    public string ColorTheme { get; set; } = "#1E3A5F";
    public string MaintenanceStatus { get; set; } = "Normal (Berjalan)";
    public int AttendanceRadiusMeters { get; set; } = 100;
    public string ActiveAcademicYear { get; set; } = "2026/2027";
    public double MinAttendancePercent { get; set; } = 85.0;
    public string WorkStartTime { get; set; } = "08:00 WIB";
    public string WorkEndTime { get; set; } = "17:00 WIB";
    public int SessionTimeoutMinutes { get; set; } = 30; // 30 minutes per FR-ATH-01
}

public class ExportReportResult
{
    public string Title { get; set; } = string.Empty;
    public string Format { get; set; } = "PDF"; // PDF or Excel
    public string FilePath { get; set; } = string.Empty;
    public int TotalRows { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
    public string FileSizeFormatted { get; set; } = "142 KB";
}
