using System.Text;
using Medieval_Internship.Models;

namespace Medieval_Internship.Services;

public class PklDataService
{
    private static PklDataService? _instance;
    public static PklDataService Instance => _instance ??= new PklDataService();

    // Data Collections
    private readonly List<AttendanceRecord> _attendances = [];
    private readonly List<DailyJournalRecord> _journals = [];
    private readonly List<SupervisionVisitRecord> _visits = [];
    private readonly List<AnnouncementRecord> _announcements = [];
    private readonly List<FinalReportRecord> _finalReports = [];
    private readonly List<MasterStudentModel> _students = [];
    private readonly List<CompanyPartnerModel> _companies = [];
    private readonly List<DepartmentClassModel> _classes = [];
    private readonly List<PklPeriodModel> _periods = [];
    private SchoolConfigModel _schoolConfig = new();

    public event Action? DataChanged;

    public PklDataService()
    {
        SeedAllData();
    }

    private void SeedAllData()
    {
        // 1. Periods
        _periods.AddRange(new[]
        {
            new PklPeriodModel
            {
                Id = "GEL-1-2026",
                Name = "Gelombang 1 - Semester Ganjil TA 2026/2027",
                StartDate = new DateTime(2026, 7, 1),
                EndDate = new DateTime(2026, 9, 30),
                IsActive = true
            },
            new PklPeriodModel
            {
                Id = "GEL-2-2026",
                Name = "Gelombang 2 - Semester Genap TA 2026/2027",
                StartDate = new DateTime(2027, 1, 5),
                EndDate = new DateTime(2027, 4, 10),
                IsActive = false
            }
        });

        // 2. Department & Classes
        _classes.AddRange(new[]
        {
            new DepartmentClassModel { MajorName = "Rekayasa Perangkat Lunak (RPL)", ClassName = "XII RPL 1", HomeroomTeacher = "Drs. Bambang Hidayat, M.Kom", TotalStudents = 36 },
            new DepartmentClassModel { MajorName = "Rekayasa Perangkat Lunak (RPL)", ClassName = "XII RPL 2", HomeroomTeacher = "Ratna Sari, S.Pd", TotalStudents = 35 },
            new DepartmentClassModel { MajorName = "Teknik Komputer dan Jaringan (TKJ)", ClassName = "XII TKJ 1", HomeroomTeacher = "Ahmad Faisal, S.T", TotalStudents = 34 },
            new DepartmentClassModel { MajorName = "Desain Komunikasi Visual (DKV)", ClassName = "XII DKV 1", HomeroomTeacher = "Maya Indah, M.Sn", TotalStudents = 36 },
            new DepartmentClassModel { MajorName = "Sistem Informatika Jaringan dan Aplikasi (SIJA)", ClassName = "XIII SIJA 1", HomeroomTeacher = "Budi Hartono, S.Kom", TotalStudents = 32 }
        });

        // 3. Company Partners (DUDI)
        _companies.AddRange(new[]
        {
            new CompanyPartnerModel
            {
                Id = "COMP-001",
                Name = "PT Telkom Indonesia (Persero) Tbk",
                IndustrySector = "Telekomunikasi & Layanan Digital Cloud",
                Address = "Telkom Landmark Tower Lt. 28, Jl. Jend. Gatot Subroto Kav. 52, Jakarta Selatan",
                ContactPerson = "Hendro Wicaksono, S.T. (Senior Lead Engineer)",
                Phone = "+62 812-9988-7766",
                Email = "pkl.telkom@telkom.co.id",
                Quota = 15,
                Occupied = 4,
                MouStatus = "Aktif (MoU No. 421/SMK1/MOU/2025)"
            },
            new CompanyPartnerModel
            {
                Id = "COMP-002",
                Name = "PT Bank Mandiri (Persero) Tbk - Digital Banking Lab",
                IndustrySector = "Fintech & Perbankan Digital",
                Address = "Mandiri Digital Hub, Plaza Mandiri, Jl. Jend. Sudirman Kav. 54-55, Jakarta",
                ContactPerson = "Aris Munandar, M.Sc (Head of Talent IT)",
                Phone = "+62 813-2233-4455",
                Email = "pkl.talent@bankmandiri.co.id",
                Quota = 10,
                Occupied = 3,
                MouStatus = "Aktif (MoU No. 422/SMK1/MOU/2025)"
            },
            new CompanyPartnerModel
            {
                Id = "COMP-003",
                Name = "PT Astra International Tbk",
                IndustrySector = "Otomotif & Manufaktur Terintegrasi",
                Address = "Menara Astra, Jl. Jend. Sudirman Kav. 5-6, Jakarta Pusat",
                ContactPerson = "Gunawan Santoso (HR Operations)",
                Phone = "+62 811-3344-5566",
                Email = "internship@astra.co.id",
                Quota = 12,
                Occupied = 4,
                MouStatus = "Aktif (MoU No. 423/SMK1/MOU/2025)"
            },
            new CompanyPartnerModel
            {
                Id = "COMP-004",
                Name = "PT Media Kreasi Visual (Creative Studio)",
                IndustrySector = "Multimedia, Animasi & UI/UX Design",
                Address = "Green Office Park BSD, Tangerang",
                ContactPerson = "Diana Puspita (Creative Director)",
                Phone = "+62 815-7788-9900",
                Email = "studio@mediakreasi.co.id",
                Quota = 8,
                Occupied = 3,
                MouStatus = "Aktif (MoU No. 424/SMK1/MOU/2025)"
            }
        });

        // 4. Master Students
        _students.AddRange(new[]
        {
            new MasterStudentModel
            {
                Id = "USR-001",
                Nisn = "0067829102",
                FullName = "Rizky Pratama",
                ClassName = "XII RPL 1",
                Major = "Rekayasa Perangkat Lunak",
                CompanyId = "COMP-001",
                CompanyName = "PT Telkom Indonesia",
                InternalMentorId = "USR-002",
                InternalMentorName = "Drs. Bambang Hidayat, M.Kom",
                ExternalMentorId = "USR-003",
                ExternalMentorName = "Hendro Wicaksono, S.T.",
                PeriodId = "GEL-1-2026",
                StatusPkl = "Aktif Magang",
                AttendanceRate = 96.5,
                JournalCount = 42
            },
            new MasterStudentModel
            {
                Id = "USR-010",
                Nisn = "0068910231",
                FullName = "Anisa Rahmawati",
                ClassName = "XII RPL 2",
                Major = "Rekayasa Perangkat Lunak",
                CompanyId = "COMP-002",
                CompanyName = "PT Bank Mandiri (Digital Lab)",
                InternalMentorId = "USR-002",
                InternalMentorName = "Drs. Bambang Hidayat, M.Kom",
                ExternalMentorId = "USR-011",
                ExternalMentorName = "Aris Munandar, M.Sc",
                PeriodId = "GEL-1-2026",
                StatusPkl = "Aktif Magang",
                AttendanceRate = 98.2,
                JournalCount = 44
            },
            new MasterStudentModel
            {
                Id = "USR-012",
                Nisn = "0071239845",
                FullName = "Dimas Surya Anggara",
                ClassName = "XII TKJ 1",
                Major = "Teknik Komputer dan Jaringan",
                CompanyId = "COMP-003",
                CompanyName = "PT Astra International",
                InternalMentorId = "USR-002",
                InternalMentorName = "Drs. Bambang Hidayat, M.Kom",
                ExternalMentorId = "USR-013",
                ExternalMentorName = "Gunawan Santoso",
                PeriodId = "GEL-1-2026",
                StatusPkl = "Aktif Magang",
                AttendanceRate = 91.0,
                JournalCount = 38
            },
            new MasterStudentModel
            {
                Id = "USR-014",
                Nisn = "0064567891",
                FullName = "Salsa Nabila Putri",
                ClassName = "XII DKV 1",
                Major = "Desain Komunikasi Visual",
                CompanyId = "COMP-004",
                CompanyName = "PT Media Kreasi Visual",
                InternalMentorId = "USR-002",
                InternalMentorName = "Drs. Bambang Hidayat, M.Kom",
                ExternalMentorId = "USR-015",
                ExternalMentorName = "Diana Puspita",
                PeriodId = "GEL-1-2026",
                StatusPkl = "Aktif Magang",
                AttendanceRate = 100.0,
                JournalCount = 45
            }
        });

        // 5. Attendance Records
        _attendances.AddRange(new[]
        {
            new AttendanceRecord
            {
                StudentId = "USR-001",
                StudentName = "Rizky Pratama",
                ClassName = "XII RPL 1",
                CompanyName = "PT Telkom Indonesia",
                Date = DateTime.Today,
                CheckInTime = "07:45 WIB",
                CheckOutTime = "17:05 WIB",
                Status = AttendanceStatus.Hadir,
                LocationName = "Telkom Landmark Tower (Radius 12m)",
                Coordinates = "-6.22972, 106.81648",
                Notes = "Hadir tepat waktu. Melanjutkan sprint tugas microservice.",
                IndustriValidation = ValidationStatus.Disetujui,
                IndustriValidatorName = "Hendro Wicaksono, S.T.",
                IndustriValidationNotes = "Kehadiran valid di kantor sesuai jadwal shift.",
                IndustriValidatedAt = DateTime.Today,
                GuruValidation = ValidationStatus.Disetujui,
                GuruValidatorName = "Drs. Bambang Hidayat, M.Kom",
                GuruValidationNotes = "Tervalidasi sesuai rekap.",
                GuruValidatedAt = DateTime.Today
            },
            new AttendanceRecord
            {
                StudentId = "USR-001",
                StudentName = "Rizky Pratama",
                ClassName = "XII RPL 1",
                CompanyName = "PT Telkom Indonesia",
                Date = DateTime.Today.AddDays(-1),
                CheckInTime = "07:50 WIB",
                CheckOutTime = "17:15 WIB",
                Status = AttendanceStatus.Hadir,
                LocationName = "Telkom Landmark Tower",
                Coordinates = "-6.22970, 106.81640",
                Notes = "Mengerjakan deployment container staging.",
                IndustriValidation = ValidationStatus.Disetujui,
                IndustriValidatorName = "Hendro Wicaksono, S.T.",
                IndustriValidationNotes = "Hadir full time.",
                IndustriValidatedAt = DateTime.Today.AddDays(-1),
                GuruValidation = ValidationStatus.Disetujui,
                GuruValidatorName = "Drs. Bambang Hidayat, M.Kom",
                GuruValidationNotes = "OK.",
                GuruValidatedAt = DateTime.Today.AddDays(-1)
            },
            new AttendanceRecord
            {
                StudentId = "USR-010",
                StudentName = "Anisa Rahmawati",
                ClassName = "XII RPL 2",
                CompanyName = "PT Bank Mandiri (Digital Lab)",
                Date = DateTime.Today,
                CheckInTime = "07:35 WIB",
                CheckOutTime = "-",
                Status = AttendanceStatus.Hadir,
                LocationName = "Mandiri Digital Hub",
                Coordinates = "-6.22550, 106.81120",
                Notes = "Slicing wireframe UI Figma Mandiri Livin.",
                IndustriValidation = ValidationStatus.Menunggu,
                GuruValidation = ValidationStatus.Menunggu
            },
            new AttendanceRecord
            {
                StudentId = "USR-012",
                StudentName = "Dimas Surya Anggara",
                ClassName = "XII TKJ 1",
                CompanyName = "PT Astra International",
                Date = DateTime.Today.AddDays(-2),
                CheckInTime = "-",
                CheckOutTime = "-",
                Status = AttendanceStatus.Izin,
                LocationName = "Rumah",
                Coordinates = "-",
                Notes = "Surat izin sakit terlampir (Flu & demam 1 hari).",
                IndustriValidation = ValidationStatus.Disetujui,
                IndustriValidatorName = "Gunawan Santoso",
                IndustriValidationNotes = "Surat dokter terlampir dan disetujui.",
                IndustriValidatedAt = DateTime.Today.AddDays(-2),
                GuruValidation = ValidationStatus.Disetujui,
                GuruValidatorName = "Drs. Bambang Hidayat, M.Kom",
                GuruValidationNotes = "Izin diakui.",
                GuruValidatedAt = DateTime.Today.AddDays(-2)
            }
        });

        // 6. Daily Journals
        _journals.AddRange(new[]
        {
            new DailyJournalRecord
            {
                StudentId = "USR-001",
                StudentName = "Rizky Pratama",
                CompanyName = "PT Telkom Indonesia",
                Date = DateTime.Today,
                ActivityTitle = "Konfigurasi Container & Pod Kubernetes pada Server Staging",
                WorkDescription = "Melakukan migrasi docker compose ke cluster minikube pod, menguji helm chart untuk service authentication microservice.",
                ToolsUsed = "Kubernetes, Docker, Helm, Linux Ubuntu Server 22.04",
                Obstacles = "Ingress controller sempat mengalami crash loop back off karena konflik port 443.",
                Solutions = "Mengubah anotasi ingress ssl-passthrough dan mengatur ulang cert-manager cert secret.",
                Status = JournalStatus.MenungguReview,
                AttachmentProof = "k8s_staging_dashboard.png"
            },
            new DailyJournalRecord
            {
                StudentId = "USR-001",
                StudentName = "Rizky Pratama",
                CompanyName = "PT Telkom Indonesia",
                Date = DateTime.Today.AddDays(-1),
                ActivityTitle = "Implementasi CI/CD Pipeline Otomatis dengan GitLab Runner",
                WorkDescription = "Setup stages build, unit test, and containerize image ke private registry Telkom. Seluruh tahapan pipeline berhasil hijau.",
                ToolsUsed = "GitLab CI/CD, Docker, Bash Scripting",
                Obstacles = "Runner kehabisan disk space saat caching gradle dependencies.",
                Solutions = "Membersihkan cache runner lama dan menambahkan disk prune cron job mingguan.",
                Status = JournalStatus.DisetujuiPenuh,
                MentorFeedback = "Implementasi pipeline rapi dan mematuhi standard SOP DevOps Telkom.",
                GuruFeedback = "Sangat relevan dengan capaian kurikulum RPL tingkat lanjut.",
                ReviewedAt = DateTime.Today.AddDays(-1),
                AttachmentProof = "gitlab_runner_success.png"
            },
            new DailyJournalRecord
            {
                StudentId = "USR-010",
                StudentName = "Anisa Rahmawati",
                CompanyName = "PT Bank Mandiri (Digital Lab)",
                Date = DateTime.Today.AddDays(-1),
                ActivityTitle = "Pembuatan Prototipe UI/UX Fitur E-Wallet",
                WorkDescription = "Menyelesaikan wireframe high-fidelity di Figma berdasarkan user flow transaksi QRIS dan verifikasi biometrik.",
                ToolsUsed = "Figma, Design System Mandiri Livin",
                Obstacles = "Konsistensi margin pada layar mobile rasio 20:9.",
                Solutions = "Menerapkan auto layout dan constraint responsif.",
                Status = JournalStatus.DisetujuiIndustri,
                MentorFeedback = "Komponen sudah sesuai design token Mandiri.",
                ReviewedAt = DateTime.Today.AddDays(-1),
                AttachmentProof = "figma_wallet_prototype.png"
            }
        });

        // 7. Supervision Visits
        _visits.AddRange(new[]
        {
            new SupervisionVisitRecord
            {
                GuruId = "USR-002",
                GuruName = "Drs. Bambang Hidayat, M.Kom",
                CompanyId = "COMP-001",
                CompanyName = "PT Telkom Indonesia (Landmark Tower)",
                VisitDate = DateTime.Today.AddDays(-7),
                VisitType = "Onsite (Kunjungan Langsung)",
                StudentsMet = "Rizky Pratama, Farhan Maulana, Kevin Jonathan",
                SupervisionNotes = "Seluruh siswa magang aktif berkontribusi dalam sprint engineering. Mentor industri sangat kooperatif.",
                IndustryFeedback = "Hendro Wicaksono: 'Siswa SMK 1 memiliki kedisiplinan dan pemahaman logic pemrograman yang sangat solid.'",
                FollowUpRecommendations = "Diberikan pendalaman tambahan tentang arsitektur event-driven sebelum evaluasi akhir.",
                OverallAssessment = "Sangat Baik (A)",
                DocumentationProof = "dokumentasi_visit_telkom.jpg"
            },
            new SupervisionVisitRecord
            {
                GuruId = "USR-002",
                GuruName = "Drs. Bambang Hidayat, M.Kom",
                CompanyId = "COMP-002",
                CompanyName = "PT Bank Mandiri (Digital Lab)",
                VisitDate = DateTime.Today.AddDays(-14),
                VisitType = "Onsite (Kunjungan Langsung)",
                StudentsMet = "Anisa Rahmawati",
                SupervisionNotes = "Supervisi mengenai progress tugas pembuatan wireframe UI dan etika kerja di perbankan.",
                IndustryFeedback = "Aris Munandar: 'Anisa teliti dan cepat beradaptasi dengan tim designer senior.'",
                FollowUpRecommendations = "Pertahankan komunikasi harian dan dokumentasi design token.",
                OverallAssessment = "Sangat Baik (A)",
                DocumentationProof = "dokumentasi_visit_mandiri.jpg"
            }
        });

        // 8. Official Announcements
        _announcements.AddRange(new[]
        {
            new AnnouncementRecord
            {
                Title = "Panduan Penyusunan Laporan Akhir PKL & Jadwal Ujian Sidang",
                Content = "Diberitahukan kepada seluruh siswa peserta PKL TA 2026/2027 bahwa draft Bab 1 s/d Bab 5 wajib diunggah ke sistem paling lambat 20 Oktober 2026. Format template resmi dapat diunduh pada portal.",
                AuthorName = "Drs. H. Mulyadi, M.M (Koordinator Hubin)",
                AuthorRole = "Bursa Kerja Khusus & Hubin",
                CreatedDate = DateTime.Today.AddDays(-2),
                TargetAudience = "Semua",
                Priority = AnnouncementPriority.Penting,
                IsPinned = true,
                AttachmentName = "Template_Laporan_PKL_Vokasi_2026.docx"
            },
            new AnnouncementRecord
            {
                Title = "Pengingat Batas Akhir Validasi Jurnal Mingguan bagi Pembimbing",
                Content = "Bapak/Ibu Guru Pendamping dan Mentor Industri dimohon untuk menyelesaikan verifikasi absensi dan persetujuan jurnal harian peserta magang sebelum hari Jumat pukul 17:00 WIB.",
                AuthorName = "Siti Nurhaliza, S.Kom (Operator Sistem)",
                AuthorRole = "Operator Sekolah",
                CreatedDate = DateTime.Today.AddDays(-4),
                TargetAudience = "Guru",
                Priority = AnnouncementPriority.Pengingat,
                IsPinned = false,
                AttachmentName = "Edaran_Monev_Pembimbing.pdf"
            },
            new AnnouncementRecord
            {
                Title = "Pemberitahuan Audit Lapangan Tim Monev Dinas Pendidikan",
                Content = "Tim Pengawas Monitoring dan Evaluasi dari Dinas Pendidikan Provinsi DKI Jakarta akan melaksanakan sampling supervisi lapangan ke 15 mitra industri terpilih mulai 1 Oktober 2026.",
                AuthorName = "Dr. Hj. Sri Wahyuni, M.Pd (Pengawas Monev)",
                AuthorRole = "Tim Monitoring & Evaluasi",
                CreatedDate = DateTime.Today.AddDays(-6),
                TargetAudience = "Monitor",
                Priority = AnnouncementPriority.Info,
                IsPinned = false,
                AttachmentName = "Jadwal_Sampling_Audit_Monev.pdf"
            }
        });

        // 9. Final Reports & Assessments
        _finalReports.AddRange(new[]
        {
            new FinalReportRecord
            {
                StudentId = "USR-001",
                StudentName = "Rizky Pratama",
                ClassName = "XII RPL 1",
                CompanyName = "PT Telkom Indonesia",
                ReportTitle = "Rancang Bangun Otomasi CI/CD dan Container Orchestration pada Microservice Telkom",
                Abstract = "Laporan ini membahas perancangan arsitektur pipeline otomatisasi testing dan container orchestration pada platform microservice staging.",
                FileName = "Laporan_PKL_Rizky_Pratama_XII_RPL_1.pdf",
                PresentationLink = "https://slide.smkn1.sch.id/sidang-rizky-telkom",
                UploadDate = DateTime.Today.AddDays(-3),
                Status = ReportStatus.MenungguReview,
                NilaiTeknisIndustri = 94.0,
                NilaiSoftSkillIndustri = 96.0,
                NilaiDisiplinIndustri = 95.0,
                CatatanIndustri = "Rizky menunjukkan kapabilitas setara junior engineer industri. Sangat direkomendasikan.",
                NilaiLaporanGuru = 91.0,
                NilaiSidangGuru = 93.0,
                CatatanGuru = "Dokumentasi kode dan diagram arsitektur Bab 3 sangat komprehensif.",
                RevisionNotes = "Perbaiki daftar pustaka sesuai standar IEEE format."
            },
            new FinalReportRecord
            {
                StudentId = "USR-010",
                StudentName = "Anisa Rahmawati",
                ClassName = "XII RPL 2",
                CompanyName = "PT Bank Mandiri (Digital Lab)",
                ReportTitle = "Redesain Antarmuka Transaksi QRIS pada Aplikasi Perbankan Mandiri Berbasis Figma",
                Abstract = "Analisis heuristik usability dan implementasi perbaikan alur registrasi dompet digital bagi nasabah baru.",
                FileName = "Laporan_PKL_Anisa_Rahmawati.pdf",
                PresentationLink = "https://slide.smkn1.sch.id/sidang-anisa-mandiri",
                UploadDate = DateTime.Today.AddDays(-5),
                Status = ReportStatus.LulusSidang,
                NilaiTeknisIndustri = 96.0,
                NilaiSoftSkillIndustri = 98.0,
                NilaiDisiplinIndustri = 97.0,
                CatatanIndustri = "Hasil desain telah diuji coba pada 100 sampel user internal bank dengan hasil optimal.",
                NilaiLaporanGuru = 94.0,
                NilaiSidangGuru = 95.0,
                CatatanGuru = "Penyampaian presentasi sidang sangat percaya diri dan argumentasi ilmiah kuat."
            }
        });
    }

    // ==========================================
    // MODUL ABSENSI HARIAN
    // ==========================================

    public List<AttendanceRecord> GetAttendanceHistory(string studentId)
    {
        return _attendances
            .Where(a => a.StudentId == studentId || string.IsNullOrEmpty(studentId))
            .OrderByDescending(a => a.Date)
            .ToList();
    }

    public List<AttendanceRecord> GetPendingAttendanceForGuru(string guruId)
    {
        return _attendances
            .Where(a => a.GuruValidation == ValidationStatus.Menunggu)
            .OrderByDescending(a => a.Date)
            .ToList();
    }

    public List<AttendanceRecord> GetPendingAttendanceForIndustri(string companyName)
    {
        return _attendances
            .Where(a => a.IndustriValidation == ValidationStatus.Menunggu &&
                       (string.IsNullOrEmpty(companyName) || a.CompanyName.Contains(companyName, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(a => a.Date)
            .ToList();
    }

    public (bool Success, string Message, AttendanceRecord Record) RecordCheckIn(
        string studentId,
        string studentName,
        string className,
        string companyName,
        AttendanceStatus status,
        string location,
        string notes)
    {
        var existingToday = _attendances.FirstOrDefault(a => a.StudentId == studentId && a.Date.Date == DateTime.Today);
        if (existingToday != null && existingToday.CheckInTime != "-")
        {
            return (false, "Anda sudah melakukan check-in kehadiran untuk hari ini.", existingToday);
        }

        var nowTimeStr = DateTime.Now.ToString("HH:mm") + " WIB";
        var record = existingToday ?? new AttendanceRecord
        {
            StudentId = studentId,
            StudentName = studentName,
            ClassName = className,
            CompanyName = companyName,
            Date = DateTime.Today
        };

        record.Status = status;
        record.CheckInTime = nowTimeStr;
        record.LocationName = string.IsNullOrWhiteSpace(location) ? "Lokasi Mitra PKL Terdeteksi (GPS Valid)" : location;
        record.Notes = notes;
        record.IndustriValidation = ValidationStatus.Menunggu;
        record.GuruValidation = ValidationStatus.Menunggu;

        if (existingToday == null)
        {
            _attendances.Insert(0, record);
        }

        DataChanged?.Invoke();
        return (true, $"Presensi masuk tercatat pukul {nowTimeStr}.", record);
    }

    public (bool Success, string Message) RecordCheckOut(string studentId, string notes)
    {
        var record = _attendances.FirstOrDefault(a => a.StudentId == studentId && a.Date.Date == DateTime.Today);
        if (record == null || record.CheckInTime == "-")
        {
            return (false, "Anda belum melakukan check-in hari ini, lakukan check-in terlebih dahulu.");
        }

        var nowTimeStr = DateTime.Now.ToString("HH:mm") + " WIB";
        record.CheckOutTime = nowTimeStr;
        if (!string.IsNullOrWhiteSpace(notes))
        {
            record.Notes = string.IsNullOrWhiteSpace(record.Notes) ? notes : $"{record.Notes} | Checkout: {notes}";
        }

        DataChanged?.Invoke();
        return (true, $"Presensi kepulangan berhasil dicatat pukul {nowTimeStr}.");
    }

    public (bool Success, string Message) ValidateAttendance(
        string attendanceId,
        UserRole reviewerRole,
        bool approve,
        string validatorName,
        string notes)
    {
        var record = _attendances.FirstOrDefault(a => a.Id == attendanceId);
        if (record == null)
            return (false, "Data absensi tidak ditemukan.");

        var valStatus = approve ? ValidationStatus.Disetujui : ValidationStatus.Ditolak;

        if (reviewerRole == UserRole.PembimbingIndustri)
        {
            record.IndustriValidation = valStatus;
            record.IndustriValidatorName = validatorName;
            record.IndustriValidationNotes = notes;
            record.IndustriValidatedAt = DateTime.Now;
        }
        else if (reviewerRole == UserRole.GuruPendamping || reviewerRole == UserRole.Admin)
        {
            record.GuruValidation = valStatus;
            record.GuruValidatorName = validatorName;
            record.GuruValidationNotes = notes;
            record.GuruValidatedAt = DateTime.Now;
        }

        DataChanged?.Invoke();
        return (true, $"Validasi absensi berhasil disimpan ({valStatus}).");
    }

    public (bool Eligible, double AttendancePercent, int ApprovedJournals, string EvaluationNotes) CheckStudentCriteriaEligibility(string studentId)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        var attendances = _attendances.Where(a => a.StudentId == studentId).ToList();
        var journals = _journals.Where(j => j.StudentId == studentId).ToList();

        int totalDays = attendances.Count == 0 ? 1 : attendances.Count;
        int presentCount = attendances.Count(a => a.Status == AttendanceStatus.Hadir);
        double attendancePercent = Math.Round(((double)presentCount / totalDays) * 100.0, 1);

        int approvedJournals = journals.Count(j => j.Status == JournalStatus.DisetujuiPenuh ||
                                                   j.Status == JournalStatus.DisetujuiIndustri ||
                                                   j.Status == JournalStatus.DisetujuiGuru);

        bool eligible = attendancePercent >= _schoolConfig.MinAttendancePercent;
        string notes = eligible
            ? $"Memenuhi syarat kelulusan (Kehadiran: {attendancePercent}% >= ambang batas {_schoolConfig.MinAttendancePercent}%)."
            : $"Belum memenuhi syarat kelulusan (Kehadiran: {attendancePercent}% < ambang batas {_schoolConfig.MinAttendancePercent}%).";

        return (eligible, attendancePercent, approvedJournals, notes);
    }

    // ==========================================
    // MODUL JURNAL HARIAN
    // ==========================================

    public List<DailyJournalRecord> GetJournalsByStudent(string studentId)
    {
        return _journals
            .Where(j => j.StudentId == studentId || string.IsNullOrEmpty(studentId))
            .OrderByDescending(j => j.Date)
            .ToList();
    }

    public List<DailyJournalRecord> GetAllJournals()
    {
        return _journals.OrderByDescending(j => j.Date).ToList();
    }

    public (bool Success, string Message, DailyJournalRecord Journal) AddJournal(DailyJournalRecord journal)
    {
        if (string.IsNullOrWhiteSpace(journal.ActivityTitle))
            return (false, "Judul aktivitas jurnal wajib diisi.", journal);

        if (string.IsNullOrWhiteSpace(journal.WorkDescription))
            return (false, "Deskripsi pekerjaan wajib diisi.", journal);

        journal.Id = $"JRN-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        journal.Date = DateTime.Today;
        journal.Status = JournalStatus.MenungguReview;

        _journals.Insert(0, journal);
        DataChanged?.Invoke();
        return (true, "Jurnal harian berhasil dicatat dan diajukan ke pembimbing.", journal);
    }

    public (bool Success, string Message) ReviewJournal(
        string journalId,
        UserRole reviewerRole,
        JournalStatus newStatus,
        string feedback)
    {
        var journal = _journals.FirstOrDefault(j => j.Id == journalId);
        if (journal == null)
            return (false, "Jurnal tidak ditemukan.");

        journal.Status = newStatus;
        if (reviewerRole == UserRole.PembimbingIndustri)
        {
            journal.MentorFeedback = feedback;
        }
        else
        {
            journal.GuruFeedback = feedback;
        }
        journal.ReviewedAt = DateTime.Now;

        DataChanged?.Invoke();
        return (true, $"Status jurnal diperbarui menjadi: {journal.StatusText}.");
    }

    // ==========================================
    // MODUL MONITORING DAN KUNJUNGAN PEMBIMBING
    // ==========================================

    public List<SupervisionVisitRecord> GetAllVisits()
    {
        return _visits.OrderByDescending(v => v.VisitDate).ToList();
    }

    public List<SupervisionVisitRecord> GetVisitsByGuru(string guruId)
    {
        return _visits
            .Where(v => v.GuruId == guruId || string.IsNullOrEmpty(guruId))
            .OrderByDescending(v => v.VisitDate)
            .ToList();
    }

    public (bool Success, string Message) AddVisit(SupervisionVisitRecord visit)
    {
        if (string.IsNullOrWhiteSpace(visit.CompanyName))
            return (false, "Nama perusahaan tujuan monitoring wajib diisi.");

        visit.Id = $"VST-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        _visits.Insert(0, visit);
        DataChanged?.Invoke();
        return (true, "Data kunjungan monitoring berhasil disimpan ke sistem.");
    }

    // ==========================================
    // MODUL INFORMASI DAN PENGUMUMAN
    // ==========================================

    public List<AnnouncementRecord> GetAnnouncementsForRole(UserRole role)
    {
        var roleStr = RoleHelper.ToStringRole(role);
        return _announcements
            .Where(a => a.TargetAudience == "Semua" ||
                        a.TargetAudience.Equals(roleStr, StringComparison.OrdinalIgnoreCase) ||
                        (role == UserRole.Admin || role == UserRole.Operator))
            .OrderByDescending(a => a.IsPinned)
            .ThenByDescending(a => a.CreatedDate)
            .ToList();
    }

    public List<AnnouncementRecord> GetAllAnnouncements()
    {
        return _announcements
            .OrderByDescending(a => a.IsPinned)
            .ThenByDescending(a => a.CreatedDate)
            .ToList();
    }

    public (bool Success, string Message) AddAnnouncement(AnnouncementRecord announcement)
    {
        if (string.IsNullOrWhiteSpace(announcement.Title))
            return (false, "Judul pengumuman tidak boleh kosong.");

        if (string.IsNullOrWhiteSpace(announcement.Content))
            return (false, "Isi pengumuman tidak boleh kosong.");

        announcement.Id = $"ANN-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        announcement.CreatedDate = DateTime.Now;
        _announcements.Insert(0, announcement);
        DataChanged?.Invoke();
        return (true, "Pengumuman resmi berhasil diterbitkan ke seluruh pengguna terkait.");
    }

    public bool DeleteAnnouncement(string id)
    {
        var item = _announcements.FirstOrDefault(a => a.Id == id);
        if (item != null)
        {
            _announcements.Remove(item);
            DataChanged?.Invoke();
            return true;
        }
        return false;
    }

    // ==========================================
    // MODUL LAPORAN AKHIR PKL & PENILAIAN
    // ==========================================

    public FinalReportRecord? GetFinalReportByStudent(string studentId)
    {
        return _finalReports.FirstOrDefault(r => r.StudentId == studentId);
    }

    public List<FinalReportRecord> GetAllFinalReports()
    {
        return _finalReports.OrderByDescending(r => r.UploadDate).ToList();
    }

    public (bool Success, string Message, FinalReportRecord Report) SubmitFinalReport(
        string studentId,
        string studentName,
        string className,
        string companyName,
        string title,
        string abstractText,
        string fileName)
    {
        if (string.IsNullOrWhiteSpace(title))
            return (false, "Judul laporan wajib diisi.", new());

        var existing = _finalReports.FirstOrDefault(r => r.StudentId == studentId);
        var report = existing ?? new FinalReportRecord
        {
            StudentId = studentId,
            StudentName = studentName,
            ClassName = className,
            CompanyName = companyName
        };

        report.ReportTitle = title;
        report.Abstract = abstractText;
        report.FileName = string.IsNullOrWhiteSpace(fileName) ? "Laporan_Akhir_Lengkap.pdf" : fileName;
        report.UploadDate = DateTime.Now;
        report.Status = ReportStatus.MenungguReview;

        if (existing == null)
        {
            _finalReports.Insert(0, report);
        }

        DataChanged?.Invoke();
        return (true, "Laporan akhir berhasil diunggah. Menunggu penilaian dari pembimbing.", report);
    }

    public (bool Success, string Message) GradeReportByIndustri(
        string reportId,
        double teknis,
        double softSkill,
        double disiplin,
        string catatan)
    {
        var report = _finalReports.FirstOrDefault(r => r.Id == reportId);
        if (report == null)
            return (false, "Laporan tidak ditemukan.");

        report.NilaiTeknisIndustri = Math.Clamp(teknis, 0, 100);
        report.NilaiSoftSkillIndustri = Math.Clamp(softSkill, 0, 100);
        report.NilaiDisiplinIndustri = Math.Clamp(disiplin, 0, 100);
        report.CatatanIndustri = catatan;

        DataChanged?.Invoke();
        return (true, $"Penilaian industri berhasil disimpan (Rata-rata DUDI: {report.RataRataIndustri:F1}).");
    }

    public (bool Success, string Message) GradeReportByGuru(
        string reportId,
        double nilaiLaporan,
        double nilaiSidang,
        string catatan)
    {
        var report = _finalReports.FirstOrDefault(r => r.Id == reportId);
        if (report == null)
            return (false, "Laporan tidak ditemukan.");

        report.NilaiLaporanGuru = Math.Clamp(nilaiLaporan, 0, 100);
        report.NilaiSidangGuru = Math.Clamp(nilaiSidang, 0, 100);
        report.CatatanGuru = catatan;
        report.Status = ReportStatus.LulusSidang;

        DataChanged?.Invoke();
        return (true, $"Penilaian sekolah berhasil disimpan (Nilai Akhir PKL: {report.NilaiAkhir:F1} • Predikat {report.Predikat}).");
    }

    public (bool Success, string Message) ReviseReport(string reportId, string revisionNotes)
    {
        var report = _finalReports.FirstOrDefault(r => r.Id == reportId);
        if (report == null)
            return (false, "Laporan tidak ditemukan.");

        report.Status = ReportStatus.PerluRevisi;
        report.RevisionNotes = revisionNotes;

        DataChanged?.Invoke();
        return (true, "Catatan revisi berhasil dikirim ke siswa peserta PKL.");
    }

    // ==========================================
    // MODUL MANAJEMEN MASTER DATA & PENGGUNA
    // ==========================================

    public List<MasterStudentModel> GetStudents() => _students;
    public List<CompanyPartnerModel> GetCompanies() => _companies;
    public List<DepartmentClassModel> GetClasses() => _classes;
    public List<PklPeriodModel> GetPeriods() => _periods;

    public void AddStudent(MasterStudentModel student)
    {
        if (string.IsNullOrEmpty(student.Id))
            student.Id = $"STU-{Guid.NewGuid().ToString()[..6].ToUpper()}";

        _students.Add(student);
        DataChanged?.Invoke();
    }

    public void AddCompany(CompanyPartnerModel company)
    {
        if (string.IsNullOrEmpty(company.Id))
            company.Id = $"COMP-{Guid.NewGuid().ToString()[..6].ToUpper()}";

        _companies.Add(company);
        DataChanged?.Invoke();
    }

    public void AddPeriod(PklPeriodModel period)
    {
        _periods.Add(period);
        DataChanged?.Invoke();
    }

    public (bool Success, string Message) UpdateStudentAssignment(
        string studentId,
        string companyId,
        string guruId,
        string industriMentorName)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId || s.Nisn == studentId);
        if (student == null)
            return (false, "Siswa tidak ditemukan.");

        var company = _companies.FirstOrDefault(c => c.Id == companyId);
        if (company != null)
        {
            student.CompanyId = company.Id;
            student.CompanyName = company.Name;
        }

        if (!string.IsNullOrWhiteSpace(guruId))
        {
            student.InternalMentorId = guruId;
            student.InternalMentorName = "Drs. Bambang Hidayat, M.Kom";
        }

        if (!string.IsNullOrWhiteSpace(industriMentorName))
        {
            student.ExternalMentorName = industriMentorName;
        }

        DataChanged?.Invoke();
        return (true, $"Penetapan siswa {student.FullName} ke {student.CompanyName} berhasil diperbarui.");
    }

    /// <summary>
    /// Impor data Siswa dari format CSV / Teks terstruktur (Format: NISN,Nama,Kelas,Jurusan,Perusahaan)
    /// </summary>
    public (int SuccessCount, int ErrorCount, string Message) ImportStudentsFromCsv(string csvContent)
    {
        if (string.IsNullOrWhiteSpace(csvContent))
            return (0, 0, "Konten data impor kosong.");

        var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int success = 0;
        int errors = 0;

        foreach (var line in lines)
        {
            if (line.StartsWith("NISN", StringComparison.OrdinalIgnoreCase) || line.StartsWith("#"))
                continue; // Skip header

            var parts = line.Split(new[] { ',', ';' });
            if (parts.Length < 3)
            {
                errors++;
                continue;
            }

            var nisn = parts[0].Trim();
            var name = parts[1].Trim();
            var className = parts[2].Trim();
            var major = parts.Length > 3 ? parts[3].Trim() : "Rekayasa Perangkat Lunak";
            var company = parts.Length > 4 ? parts[4].Trim() : "PT Telkom Indonesia";

            if (string.IsNullOrWhiteSpace(nisn) || string.IsNullOrWhiteSpace(name))
            {
                errors++;
                continue;
            }

            var existing = _students.FirstOrDefault(s => s.Nisn == nisn);
            if (existing != null)
            {
                existing.FullName = name;
                existing.ClassName = className;
                existing.Major = major;
                existing.CompanyName = company;
            }
            else
            {
                _students.Add(new MasterStudentModel
                {
                    Id = $"STU-{Guid.NewGuid().ToString()[..6].ToUpper()}",
                    Nisn = nisn,
                    FullName = name,
                    ClassName = className,
                    Major = major,
                    CompanyName = company,
                    InternalMentorName = "Drs. Bambang Hidayat, M.Kom",
                    ExternalMentorName = "Hendro Wicaksono, S.T.",
                    StatusPkl = "Aktif Magang",
                    AttendanceRate = 100.0,
                    JournalCount = 0
                });
            }

            success++;
        }

        DataChanged?.Invoke();
        return (success, errors, $"Berhasil mengimpor {success} data siswa ({errors} baris dilewati/galat).");
    }

    /// <summary>
    /// Impor data Mitra Perusahaan (DUDI) dari CSV / Teks (Format: NamaPerusahaan,Bidang,Alamat,Kontak,Kuota)
    /// </summary>
    public (int SuccessCount, int ErrorCount, string Message) ImportCompaniesFromCsv(string csvContent)
    {
        if (string.IsNullOrWhiteSpace(csvContent))
            return (0, 0, "Konten data impor kosong.");

        var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int success = 0;
        int errors = 0;

        foreach (var line in lines)
        {
            if (line.StartsWith("Nama", StringComparison.OrdinalIgnoreCase) || line.StartsWith("#"))
                continue;

            var parts = line.Split(new[] { ',', ';' });
            if (parts.Length < 2)
            {
                errors++;
                continue;
            }

            var name = parts[0].Trim();
            var sector = parts[1].Trim();
            var address = parts.Length > 2 ? parts[2].Trim() : "DKI Jakarta";
            var contact = parts.Length > 3 ? parts[3].Trim() : "HR Division";
            int quota = parts.Length > 4 && int.TryParse(parts[4].Trim(), out int q) ? q : 10;

            if (string.IsNullOrWhiteSpace(name))
            {
                errors++;
                continue;
            }

            var existing = _companies.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.IndustrySector = sector;
                existing.Address = address;
                existing.ContactPerson = contact;
                existing.Quota = quota;
            }
            else
            {
                _companies.Add(new CompanyPartnerModel
                {
                    Id = $"COMP-{Guid.NewGuid().ToString()[..6].ToUpper()}",
                    Name = name,
                    IndustrySector = sector,
                    Address = address,
                    ContactPerson = contact,
                    Quota = quota,
                    Occupied = 0,
                    MouStatus = "Aktif (MoU 2026/2027)"
                });
            }

            success++;
        }

        DataChanged?.Invoke();
        return (success, errors, $"Berhasil mengimpor {success} mitra industri ({errors} baris dilewati/galat).");
    }

    // ==========================================
    // MODUL DASHBOARD DAN PELAPORAN (PDF / EXCEL EXPORT)
    // ==========================================

    public ExportReportResult GenerateExportReport(string reportType, string format)
    {
        var isExcel = format.Equals("Excel", StringComparison.OrdinalIgnoreCase) || format.Equals("CSV", StringComparison.OrdinalIgnoreCase);
        var ext = isExcel ? ".csv" : ".txt";
        var fileName = $"Laporan_{reportType.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}{ext}";

        var folder = FileSystem.CacheDirectory;
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fullPath = Path.Combine(folder, fileName);
        var sb = new StringBuilder();

        int totalRows = 0;

        if (reportType.Contains("Absensi", StringComparison.OrdinalIgnoreCase))
        {
            if (isExcel)
            {
                sb.AppendLine("ID Presensi,Nama Siswa,Kelas,Mitra DUDI,Tanggal,Jam Masuk,Jam Pulang,Status,Lokasi,Validasi Industri,Validasi Guru");
                foreach (var a in _attendances)
                {
                    sb.AppendLine($"\"{a.Id}\",\"{a.StudentName}\",\"{a.ClassName}\",\"{a.CompanyName}\",\"{a.DateFormatted}\",\"{a.CheckInTime}\",\"{a.CheckOutTime}\",\"{a.Status}\",\"{a.LocationName}\",\"{a.IndustriValidation}\",\"{a.GuruValidation}\"");
                    totalRows++;
                }
            }
            else
            {
                sb.AppendLine("=========================================================================================");
                sb.AppendLine($"                       {_schoolConfig.SchoolName.ToUpper()}");
                sb.AppendLine("                REKAPITULASI LAPORAN KEHADIRAN SISWA PKL");
                sb.AppendLine($"                    Tahun Ajaran {_schoolConfig.ActiveAcademicYear}");
                sb.AppendLine("=========================================================================================");
                sb.AppendLine($"Dicetak pada: {DateTime.Now:dd MMMM yyyy HH:mm} WIB");
                sb.AppendLine("-----------------------------------------------------------------------------------------");
                sb.AppendLine(string.Format("{0,-12} | {1,-18} | {2,-10} | {3,-12} | {4,-8} | {5,-15}", "TANGGAL", "NAMA SISWA", "KELAS", "STATUS", "JAM", "VALIDASI"));
                sb.AppendLine("-----------------------------------------------------------------------------------------");
                foreach (var a in _attendances)
                {
                    sb.AppendLine(string.Format("{0,-12} | {1,-18} | {2,-10} | {3,-12} | {4,-8} | {5,-15}", a.DateFormatted, a.StudentName, a.ClassName, a.Status, a.CheckInTime, a.ValidationSummary));
                    totalRows++;
                }
                sb.AppendLine("=========================================================================================");
                sb.AppendLine($"Total Record: {totalRows} data presensi siswa.");
            }
        }
        else if (reportType.Contains("Jurnal", StringComparison.OrdinalIgnoreCase))
        {
            if (isExcel)
            {
                sb.AppendLine("ID Jurnal,Nama Siswa,Tanggal,Judul Kegiatan,Alat/Teknologi,Status,Feedback Mentor");
                foreach (var j in _journals)
                {
                    sb.AppendLine($"\"{j.Id}\",\"{j.StudentName}\",\"{j.DateFormatted}\",\"{j.ActivityTitle}\",\"{j.ToolsUsed}\",\"{j.StatusText}\",\"{j.MentorFeedback}\"");
                    totalRows++;
                }
            }
            else
            {
                sb.AppendLine("=========================================================================================");
                sb.AppendLine($"                       {_schoolConfig.SchoolName.ToUpper()}");
                sb.AppendLine("                 REKAPITULASI JURNAL HARIAN PESERTA MAGANG");
                sb.AppendLine("=========================================================================================");
                foreach (var j in _journals)
                {
                    sb.AppendLine($"ID JURNAL : {j.Id} | Tanggal: {j.DateFormatted}");
                    sb.AppendLine($"Siswa     : {j.StudentName} ({j.CompanyName})");
                    sb.AppendLine($"Aktivitas : {j.ActivityTitle}");
                    sb.AppendLine($"Rincian   : {j.WorkDescription}");
                    sb.AppendLine($"Kendala   : {j.Obstacles}");
                    sb.AppendLine($"Status    : {j.StatusText} | Reviewer Feedback: {j.MentorFeedback}");
                    sb.AppendLine("-----------------------------------------------------------------------------------------");
                    totalRows++;
                }
            }
        }
        else if (reportType.Contains("Monitoring", StringComparison.OrdinalIgnoreCase) || reportType.Contains("Kunjungan", StringComparison.OrdinalIgnoreCase))
        {
            if (isExcel)
            {
                sb.AppendLine("ID Kunjungan,Guru Pembimbing,Perusahaan Mitra,Tanggal Visit,Jenis Visit,Siswa Ditemui,Evaluasi,Tindak Lanjut");
                foreach (var v in _visits)
                {
                    sb.AppendLine($"\"{v.Id}\",\"{v.GuruName}\",\"{v.CompanyName}\",\"{v.VisitDateFormatted}\",\"{v.VisitType}\",\"{v.StudentsMet}\",\"{v.OverallAssessment}\",\"{v.FollowUpRecommendations}\"");
                    totalRows++;
                }
            }
            else
            {
                sb.AppendLine("=========================================================================================");
                sb.AppendLine($"                       {_schoolConfig.SchoolName.ToUpper()}");
                sb.AppendLine("             LAPORAN KUNJUNGAN SUPERVISI & MONITORING GURU KE DUDI");
                sb.AppendLine("=========================================================================================");
                foreach (var v in _visits)
                {
                    sb.AppendLine($"Mitra Perusahaan: {v.CompanyName} ({v.VisitType})");
                    sb.AppendLine($"Guru Supervisor : {v.GuruName} • Tanggal: {v.VisitDateFormatted}");
                    sb.AppendLine($"Siswa Ditemui   : {v.StudentsMet}");
                    sb.AppendLine($"Catatan Monev   : {v.SupervisionNotes}");
                    sb.AppendLine($"Masukan Industri: {v.IndustryFeedback}");
                    sb.AppendLine($"Rekomendasi     : {v.FollowUpRecommendations}");
                    sb.AppendLine("-----------------------------------------------------------------------------------------");
                    totalRows++;
                }
            }
        }
        else // Nilai & Evaluasi Akhir
        {
            if (isExcel)
            {
                sb.AppendLine("Nama Siswa,Kelas,Tempat PKL,Rata-rata DUDI,Rata-rata Sekolah,Nilai Akhir,Predikat,Status Kelulusan");
                foreach (var r in _finalReports)
                {
                    sb.AppendLine($"\"{r.StudentName}\",\"{r.ClassName}\",\"{r.CompanyName}\",\"{r.RataRataIndustri:F1}\",\"{r.RataRataGuru:F1}\",\"{r.NilaiAkhir:F1}\",\"{r.Predikat}\",\"{r.StatusText}\"");
                    totalRows++;
                }
            }
            else
            {
                sb.AppendLine("=========================================================================================");
                sb.AppendLine($"                       {_schoolConfig.SchoolName.ToUpper()}");
                sb.AppendLine("                 TRANSKRIP REKAPITULASI NILAI AKHIR PKL");
                sb.AppendLine("=========================================================================================");
                sb.AppendLine(string.Format("{0,-18} | {1,-10} | {2,-20} | {3,-10} | {4,-10} | {5,-12}", "NAMA SISWA", "KELAS", "TEMPAT PKL", "NILAI DUDI", "NILAI GURU", "HASIL AKHIR"));
                sb.AppendLine("-----------------------------------------------------------------------------------------");
                foreach (var r in _finalReports)
                {
                    sb.AppendLine(string.Format("{0,-18} | {1,-10} | {2,-20} | {3,-10:F1} | {4,-10:F1} | {5,-12}", r.StudentName, r.ClassName, r.CompanyName, r.RataRataIndustri, r.RataRataGuru, $"{r.NilaiAkhir:F1} ({r.Predikat[..1]})"));
                    totalRows++;
                }
            }
        }

        File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);
        var fileInfo = new FileInfo(fullPath);

        return new ExportReportResult
        {
            Title = reportType,
            Format = isExcel ? "Microsoft Excel (.csv)" : "Dokumen Resmi (.txt / PDF)",
            FilePath = fullPath,
            TotalRows = totalRows,
            FileSizeFormatted = $"{Math.Max(1, fileInfo.Length / 1024)} KB",
            GeneratedAt = DateTime.Now
        };
    }

    // ==========================================
    // MODUL MANAJEMEN APLIKASI
    // ==========================================

    public SchoolConfigModel GetSchoolConfig() => _schoolConfig;

    public void UpdateSchoolConfig(SchoolConfigModel config)
    {
        _schoolConfig = config;
        DataChanged?.Invoke();
    }
}
