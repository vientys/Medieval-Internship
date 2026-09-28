using System.Collections.ObjectModel;
using Medieval_Internship.Models;

namespace Medieval_Internship.Services;

public class RoleDashboardData
{
    public string RoleTitle { get; set; } = string.Empty;
    public string RoleSubtitle { get; set; } = string.Empty;
    public string WelcomeBannerTitle { get; set; } = string.Empty;
    public string WelcomeBannerDesc { get; set; } = string.Empty;
    public string ProgressTitle { get; set; } = string.Empty;
    public double ProgressRatio { get; set; } = 0.5;
    public string ProgressText { get; set; } = string.Empty;
    public bool ShowProgressBar { get; set; } = false;

    public List<StatCardModel> StatCards { get; set; } = [];
    public List<QuickActionModel> QuickActions { get; set; } = [];
    public List<StudentItemModel> Students { get; set; } = [];
    public List<JournalItemModel> Journals { get; set; } = [];
    public List<DocumentItemModel> Documents { get; set; } = [];
    public List<ActivityLogModel> Activities { get; set; } = [];
}

public class DashboardDataService
{
    private static DashboardDataService? _instance;
    public static DashboardDataService Instance => _instance ??= new DashboardDataService();

    public RoleDashboardData GetDashboardData(UserRole role, UserModel? user)
    {
        var userName = user?.FullName ?? "Pengguna";

        return role switch
        {
            UserRole.Siswa => GetSiswaData(userName),
            UserRole.GuruPendamping => GetGuruData(userName),
            UserRole.PembimbingIndustri => GetIndustriData(userName),
            UserRole.Admin => GetAdminData(userName),
            UserRole.Operator => GetOperatorData(userName),
            UserRole.Monitor => GetMonitorData(userName),
            _ => GetSiswaData(userName)
        };
    }

    private RoleDashboardData GetSiswaData(string userName)
    {
        return new RoleDashboardData
        {
            RoleTitle = "Siswa Magang (Intern)",
            RoleSubtitle = "PT Telkom Indonesia • Divisi Cloud & Software Lab",
            WelcomeBannerTitle = $"Semangat Pagi, {userName}! 🚀",
            WelcomeBannerDesc = "Hari ke-45 dari 90 hari PKL. Jurnal hari ini belum diisi, jangan lupa presensi dan catat aktivitasmu.",
            ShowProgressBar = true,
            ProgressTitle = "Progres Periode Magang (45 / 90 Hari)",
            ProgressRatio = 0.50,
            ProgressText = "50% Selesai • Sisa 45 Hari Lagi",

            StatCards =
            [
                new()
                {
                    Title = "Kehadiran",
                    Value = "96.5%",
                    Subtext = "42 Hadir • 1 Izin • 0 Alpa",
                    Icon = "📅",
                    AccentColor = "#10B981",
                    LightBgColor = "#ECFDF5",
                    BadgeText = "Disiplin"
                },
                new()
                {
                    Title = "Jurnal Harian",
                    Value = "42/45",
                    Subtext = "40 Disetujui • 2 Menunggu",
                    Icon = "📝",
                    AccentColor = "#2563EB",
                    LightBgColor = "#EFF6FF",
                    BadgeText = "Aktif"
                },
                new()
                {
                    Title = "Nilai Sementara",
                    Value = "92.4",
                    Subtext = "Predikat A (Sangat Baik)",
                    Icon = "⭐",
                    AccentColor = "#F59E0B",
                    LightBgColor = "#FEF3C7",
                    BadgeText = "Grade A"
                },
                new()
                {
                    Title = "Laporan Akhir",
                    Value = "Bab 3",
                    Subtext = "Reviu Pembimbing Sekolah",
                    Icon = "📖",
                    AccentColor = "#8B5CF6",
                    LightBgColor = "#F5F3FF",
                    BadgeText = "Revisi 1"
                }
            ],

            QuickActions =
            [
                new()
                {
                    Title = "Presensi Masuk",
                    Subtitle = "Catat kehadiran hari ini (GPS DUDI)",
                    Icon = "📍",
                    Color = "#10B981",
                    LightColor = "#ECFDF5",
                    ActionKey = "presensi_siswa"
                },
                new()
                {
                    Title = "Tulis Jurnal",
                    Subtitle = "Laporkan pekerjaan harian & bukti",
                    Icon = "✍️",
                    Color = "#2563EB",
                    LightColor = "#EFF6FF",
                    ActionKey = "tulis_jurnal"
                },
                new()
                {
                    Title = "Unggah Laporan",
                    Subtitle = "Upload dokumen Bab 1-5 PKL",
                    Icon = "📤",
                    Color = "#6366F1",
                    LightColor = "#EEF2FF",
                    ActionKey = "upload_laporan"
                },
                new()
                {
                    Title = "Lihat Penilaian",
                    Subtitle = "Rincian nilai DUDI & Sekolah",
                    Icon = "🏆",
                    Color = "#D97706",
                    LightColor = "#FFFBEB",
                    ActionKey = "lihat_nilai"
                },
                new()
                {
                    Title = "Pengumuman Resmi",
                    Subtitle = "Info resmi PKL dari sekolah & Hubin",
                    Icon = "📢",
                    Color = "#0D9488",
                    LightColor = "#F0FDFA",
                    ActionKey = "pengumuman_info"
                },
                new()
                {
                    Title = "Profil & Bantuan",
                    Subtitle = "Kontak helpdesk sekolah & profil aplikasi",
                    Icon = "⚙️",
                    Color = "#475569",
                    LightColor = "#F1F5F9",
                    ActionKey = "bantuan_aplikasi"
                }
            ],

            Journals =
            [
                new()
                {
                    Title = "Konfigurasi Container & Pod Kubernetes pada Server Staging",
                    StudentName = "Rizky Pratama",
                    DateFormatted = "Hari ini, 24 Sep 2026",
                    Summary = "Melakukan migrasi docker compose ke minikube pod, menguji helm chart untuk service authentication microservice.",
                    Status = "Menunggu Verifikasi",
                    StatusColor = "#D97706",
                    StatusBgColor = "#FEF3C7"
                },
                new()
                {
                    Title = "Implementasi CI/CD Pipeline Otomatis dengan GitLab Runner",
                    StudentName = "Rizky Pratama",
                    DateFormatted = "Kemarin, 23 Sep 2026",
                    Summary = "Setup stages build, test, and containerize. Pipeline berhasil lolos 100% tanpa kendala.",
                    Status = "Disetujui Mentor",
                    StatusColor = "#10B981",
                    StatusBgColor = "#ECFDF5"
                },
                new()
                {
                    Title = "Optimasi Query Database PostgreSQL dan Indexing",
                    StudentName = "Rizky Pratama",
                    DateFormatted = "22 Sep 2026",
                    Summary = "Menambahkan indeks pada foreign key transaksi log, meningkatkan latency response API sebesar 40%.",
                    Status = "Disetujui Mentor",
                    StatusColor = "#10B981",
                    StatusBgColor = "#ECFDF5"
                }
            ],

            Activities =
            [
                new()
                {
                    Title = "Presensi Masuk Berhasil Divalidasi",
                    TimeAgo = "07:45 WIB",
                    Detail = "Radius 15m dari Kantor Telkom Landmark Tower",
                    Icon = "📍",
                    Tag = "Presensi",
                    TagColor = "#10B981"
                },
                new()
                {
                    Title = "Catatan Mentor Industri",
                    TimeAgo = "Kemarin 16:30 WIB",
                    Detail = "Hendro Wicaksono: 'Bagus, lanjutkan dokumentasi API ke Swagger doc.'",
                    Icon = "💬",
                    Tag = "Feedback",
                    TagColor = "#2563EB"
                },
                new()
                {
                    Title = "Pengingat Kunjungan Monitoring",
                    TimeAgo = "2 hari lalu",
                    Detail = "Drs. Bambang Hidayat akan melakukan kunjungan pada Kamis depan.",
                    Icon = "🔔",
                    Tag = "Monev",
                    TagColor = "#8B5CF6"
                }
            ]
        };
    }

    private RoleDashboardData GetGuruData(string userName)
    {
        return new RoleDashboardData
        {
            RoleTitle = "Guru Pendamping (Supervisor Sekolah)",
            RoleSubtitle = "Wilayah Bimbingan: Jakarta Pusat & Selatan • 7 Mitra DUDI",
            WelcomeBannerTitle = $"Selamat Bertugas, {userName}! 👨‍🏫",
            WelcomeBannerDesc = "Ada 6 jurnal siswa bimbingan yang menunggu verifikasi Anda dan 2 jadwal kunjungan monitoring pekan ini.",
            ShowProgressBar = false,

            StatCards =
            [
                new()
                {
                    Title = "Siswa Bimbingan",
                    Value = "28 Siswa",
                    Subtext = "Tersebar di 7 Mitra Industri",
                    Icon = "👨‍🎓",
                    AccentColor = "#2563EB",
                    LightBgColor = "#EFF6FF",
                    BadgeText = "Aktif"
                },
                new()
                {
                    Title = "Verifikasi Jurnal",
                    Value = "6 Pending",
                    Subtext = "Perlu paraf & catatan guru",
                    Icon = "📝",
                    AccentColor = "#EF4444",
                    LightBgColor = "#FEF2F2",
                    BadgeText = "Prioritas"
                },
                new()
                {
                    Title = "Kunjungan DUDI",
                    Value = "3 Lokasi",
                    Subtext = "Rencana monitoring pekan ini",
                    Icon = "🚗",
                    AccentColor = "#8B5CF6",
                    LightBgColor = "#F5F3FF",
                    BadgeText = "Terjadwal"
                },
                new()
                {
                    Title = "Rata-rata Disiplin",
                    Value = "97.1%",
                    Subtext = "Tingkat kehadiran siswa bimbingan",
                    Icon = "📊",
                    AccentColor = "#10B981",
                    LightBgColor = "#ECFDF5",
                    BadgeText = "Sangat Baik"
                }
            ],

            QuickActions =
            [
                new()
                {
                    Title = "Validasi Presensi",
                    Subtitle = "Setujui atau tolak kehadiran siswa di DUDI",
                    Icon = "📍",
                    Color = "#10B981",
                    LightColor = "#ECFDF5",
                    ActionKey = "validasi_presensi_guru"
                },
                new()
                {
                    Title = "Verifikasi Jurnal",
                    Subtitle = "Setujui atau beri catatan jurnal siswa",
                    Icon = "✍️",
                    Color = "#2563EB",
                    LightColor = "#EFF6FF",
                    ActionKey = "verif_jurnal_guru"
                },
                new()
                {
                    Title = "Jadwal Monitoring",
                    Subtitle = "Buat surat tugas & agenda kunjungan",
                    Icon = "🚗",
                    Color = "#8B5CF6",
                    LightColor = "#F5F3FF",
                    ActionKey = "jadwal_monev"
                },
                new()
                {
                    Title = "Input Nilai & Laporan",
                    Subtitle = "Form penilaian aspek akademis & sikap",
                    Icon = "⭐",
                    Color = "#D97706",
                    LightColor = "#FFFBEB",
                    ActionKey = "nilai_sekolah"
                },
                new()
                {
                    Title = "Penempatan Bimbingan",
                    Subtitle = "Daftar siswa & mitra penugasan PKL",
                    Icon = "👥",
                    Color = "#059669",
                    LightColor = "#ECFDF5",
                    ActionKey = "penempatan_pkl"
                },
                new()
                {
                    Title = "Buat Pengumuman",
                    Subtitle = "Kirim info resmi ke siswa & mitra DUDI",
                    Icon = "📢",
                    Color = "#0D9488",
                    LightColor = "#F0FDFA",
                    ActionKey = "pengumuman_info"
                },
                new()
                {
                    Title = "Ekspor Rekap Laporan",
                    Subtitle = "Unduh nilai & absensi format PDF/Excel",
                    Icon = "📊",
                    Color = "#6366F1",
                    LightColor = "#EEF2FF",
                    ActionKey = "ekspor_laporan"
                }
            ],

            Students =
            [
                new()
                {
                    Name = "Rizky Pratama",
                    NisnOrId = "NISN: 0067829102",
                    ClassName = "XII RPL 1",
                    CompanyName = "PT Telkom Indonesia",
                    AttendancePercent = "96.5%",
                    LastJournalStatus = "Up to date",
                    StatusBadgeColor = "#10B981",
                    StatusLightColor = "#ECFDF5",
                    Progress = 0.50
                },
                new()
                {
                    Name = "Anisa Rahmawati",
                    NisnOrId = "NISN: 0068910231",
                    ClassName = "XII RPL 2",
                    CompanyName = "PT Bank Mandiri (Digital Lab)",
                    AttendancePercent = "98.2%",
                    LastJournalStatus = "1 Jurnal Pending",
                    StatusBadgeColor = "#D97706",
                    StatusLightColor = "#FEF3C7",
                    Progress = 0.55
                },
                new()
                {
                    Name = "Dimas Surya Anggara",
                    NisnOrId = "NISN: 0071239845",
                    ClassName = "XII TKJ 1",
                    CompanyName = "PT Astra International",
                    AttendancePercent = "91.0%",
                    LastJournalStatus = "Izin 2 hari lalu",
                    StatusBadgeColor = "#6366F1",
                    StatusLightColor = "#EEF2FF",
                    Progress = 0.48
                },
                new()
                {
                    Name = "Salsa Nabila Putri",
                    NisnOrId = "NISN: 0064567891",
                    ClassName = "XII DKV 1",
                    CompanyName = "PT Media Kreasi Visual",
                    AttendancePercent = "100%",
                    LastJournalStatus = "Laporan Selesai",
                    StatusBadgeColor = "#10B981",
                    StatusLightColor = "#ECFDF5",
                    Progress = 0.60
                }
            ],

            Journals =
            [
                new()
                {
                    Title = "Pembuatan Prototipe UI/UX Fitur E-Wallet",
                    StudentName = "Anisa Rahmawati (PT Bank Mandiri)",
                    DateFormatted = "24 Sep 2026",
                    Summary = "Menyelesaikan wireframe high-fidelity di Figma berdasarkan user flow transaksi QRIS.",
                    Status = "Butuh Reviu Guru",
                    StatusColor = "#EF4444",
                    StatusBgColor = "#FEF2F2"
                },
                new()
                {
                    Title = "Pemasangan Access Point Cisco Catalyst di Gedung C",
                    StudentName = "Dimas Surya Anggara (PT Astra)",
                    DateFormatted = "23 Sep 2026",
                    Summary = "Crimping kabel UTP Cat6, konfigurasi VLAN 20, dan pengujian throughput wifi.",
                    Status = "Butuh Reviu Guru",
                    StatusColor = "#EF4444",
                    StatusBgColor = "#FEF2F2"
                }
            ]
        };
    }

    private RoleDashboardData GetIndustriData(string userName)
    {
        return new RoleDashboardData
        {
            RoleTitle = "Pembimbing Industri (Mentor DUDI)",
            RoleSubtitle = "PT Telkom Indonesia • Divisi Cloud & Software Lab",
            WelcomeBannerTitle = $"Selamat Datang, {userName}! 🏢",
            WelcomeBannerDesc = "Anda membimbing 4 siswa magang aktif. Tinjau log aktivitas harian mereka dan berikan evaluasi berkala.",
            ShowProgressBar = false,

            StatCards =
            [
                new()
                {
                    Title = "Siswa Magang",
                    Value = "4 Orang",
                    Subtext = "Divisi Cloud & Software Lab",
                    Icon = "👥",
                    AccentColor = "#6366F1",
                    LightBgColor = "#EEF2FF",
                    BadgeText = "Aktif"
                },
                new()
                {
                    Title = "Approval Jurnal",
                    Value = "3 Jurnal",
                    Subtext = "Menunggu persetujuan Anda",
                    Icon = "📋",
                    AccentColor = "#F59E0B",
                    LightBgColor = "#FEF3C7",
                    BadgeText = "Segera"
                },
                new()
                {
                    Title = "Kehadiran Tim",
                    Value = "97.5%",
                    Subtext = "Rata-rata presensi di kantor",
                    Icon = "🕒",
                    AccentColor = "#10B981",
                    LightBgColor = "#ECFDF5",
                    BadgeText = "Tertib"
                },
                new()
                {
                    Title = "Task Selesai",
                    Value = "84%",
                    Subtext = "Sprint 4 Task PKL",
                    Icon = "🎯",
                    AccentColor = "#2563EB",
                    LightBgColor = "#EFF6FF",
                    BadgeText = "On Track"
                }
            ],

            QuickActions =
            [
                new()
                {
                    Title = "Validasi Kehadiran",
                    Subtitle = "Verifikasi presensi siswa di tempat kerja",
                    Icon = "📍",
                    Color = "#10B981",
                    LightColor = "#ECFDF5",
                    ActionKey = "validasi_presensi_industri"
                },
                new()
                {
                    Title = "Approve Jurnal",
                    Subtitle = "Validasi aktivitas harian siswa bimbingan",
                    Icon = "✅",
                    Color = "#2563EB",
                    LightColor = "#EFF6FF",
                    ActionKey = "approve_jurnal_industri"
                },
                new()
                {
                    Title = "Beri Tugas Proyek",
                    Subtitle = "Bagikan task / tiket sprint magang",
                    Icon = "🎯",
                    Color = "#6366F1",
                    LightColor = "#EEF2FF",
                    ActionKey = "beri_tugas"
                },
                new()
                {
                    Title = "Nilai Kinerja DUDI",
                    Subtitle = "Evaluasi softskill, teknis & inisiatif",
                    Icon = "⭐",
                    Color = "#D97706",
                    LightColor = "#FFFBEB",
                    ActionKey = "nilai_industri"
                },
                new()
                {
                    Title = "Catatan Pembinaan",
                    Subtitle = "Catat feedback & bimbingan siswa",
                    Icon = "💬",
                    Color = "#059669",
                    LightColor = "#ECFDF5",
                    ActionKey = "catatan_pembinaan"
                },
                new()
                {
                    Title = "Pengumuman Industri",
                    Subtitle = "Informasi aturan & jam kerja perusahaan",
                    Icon = "📢",
                    Color = "#0D9488",
                    LightColor = "#F0FDFA",
                    ActionKey = "pengumuman_info"
                }
            ],

            Students =
            [
                new()
                {
                    Name = "Rizky Pratama",
                    NisnOrId = "SMK Negeri 1 Jakarta",
                    ClassName = "Backend & Cloud Intern",
                    CompanyName = "Sprint 4: Kubernetes Setup",
                    AttendancePercent = "96.5%",
                    LastJournalStatus = "Menunggu Approval",
                    StatusBadgeColor = "#D97706",
                    StatusLightColor = "#FEF3C7",
                    Progress = 0.50
                },
                new()
                {
                    Name = "Farhan Maulana",
                    NisnOrId = "SMK Negeri 26 Jakarta",
                    ClassName = "Frontend React Intern",
                    CompanyName = "Sprint 4: UI Dashboard Telkom",
                    AttendancePercent = "98.0%",
                    LastJournalStatus = "Approved",
                    StatusBadgeColor = "#10B981",
                    StatusLightColor = "#ECFDF5",
                    Progress = 0.65
                },
                new()
                {
                    Name = "Kevin Jonathan",
                    NisnOrId = "SMK Telkom Sandhy Putra",
                    ClassName = "QA Automation Intern",
                    CompanyName = "Sprint 4: Cypress Test Suite",
                    AttendancePercent = "100%",
                    LastJournalStatus = "Approved",
                    StatusBadgeColor = "#10B981",
                    StatusLightColor = "#ECFDF5",
                    Progress = 0.70
                }
            ],

            Journals =
            [
                new()
                {
                    Title = "Deploy Microservice Auth ke Cluster Staging",
                    StudentName = "Rizky Pratama",
                    DateFormatted = "Hari ini, 15:40 WIB",
                    Summary = "Setup helm chart, configmap environment, dan ingress route. Perlu dicek pembimbing.",
                    Status = "Menunggu Approval",
                    StatusColor = "#D97706",
                    StatusBgColor = "#FEF3C7"
                },
                new()
                {
                    Title = "Refactoring State Management dengan Redux Toolkit",
                    StudentName = "Farhan Maulana",
                    DateFormatted = "Kemarin, 16:15 WIB",
                    Summary = "Memperbaiki re-render berlebih pada halaman daftar invoice dan telemetry.",
                    Status = "Disetujui Mentor",
                    StatusColor = "#10B981",
                    StatusBgColor = "#ECFDF5"
                }
            ]
        };
    }

    private RoleDashboardData GetAdminData(string userName)
    {
        return new RoleDashboardData
        {
            RoleTitle = "System Administrator (Super Admin)",
            RoleSubtitle = "Pusat Data & Infrastruktur Informasi PKL Vokasi",
            WelcomeBannerTitle = $"Panel Kendali Admin, {userName}! ⚙️",
            WelcomeBannerDesc = "Sistem berjalan optimal (99.9% Uptime). Kelola master data, pengguna, dan hak akses dari satu pusat kendali.",
            ShowProgressBar = false,

            StatCards =
            [
                new()
                {
                    Title = "Total Pengguna",
                    Value = "642 User",
                    Subtext = "Siswa, Guru, DUDI, Operator",
                    Icon = "👥",
                    AccentColor = "#7C3AED",
                    LightBgColor = "#F5F3FF",
                    BadgeText = "All Roles"
                },
                new()
                {
                    Title = "Mitra Industri",
                    Value = "54 DUDI",
                    Subtext = "Terverifikasi & MoU Aktif",
                    Icon = "🏢",
                    AccentColor = "#2563EB",
                    LightBgColor = "#EFF6FF",
                    BadgeText = "Terdaftar"
                },
                new()
                {
                    Title = "Status Database",
                    Value = "Normal",
                    Subtext = "Backup otomatis setiap 00:00",
                    Icon = "🛡️",
                    AccentColor = "#10B981",
                    LightBgColor = "#ECFDF5",
                    BadgeText = "Healthy"
                },
                new()
                {
                    Title = "Log Keamanan",
                    Value = "0 Anomali",
                    Subtext = "Semua koneksi terenkripsi TLS",
                    Icon = "🔒",
                    AccentColor = "#D97706",
                    LightBgColor = "#FFFBEB",
                    BadgeText = "Secure"
                }
            ],

            QuickActions =
            [
                new()
                {
                    Title = "Master Data",
                    Subtitle = "Kelola siswa, guru, dudi, kelas & impor",
                    Icon = "📋",
                    Color = "#7C3AED",
                    LightColor = "#F5F3FF",
                    ActionKey = "admin_user_mgmt"
                },
                new()
                {
                    Title = "Penempatan PKL",
                    Subtitle = "Plotting siswa ke DUDI & periode PKL",
                    Icon = "👥",
                    Color = "#2563EB",
                    LightColor = "#EFF6FF",
                    ActionKey = "admin_periode_mgmt"
                },
                new()
                {
                    Title = "Presensi Seluruh Siswa",
                    Subtitle = "Monitoring & rekap absensi harian",
                    Icon = "📍",
                    Color = "#10B981",
                    LightColor = "#ECFDF5",
                    ActionKey = "presensi_siswa"
                },
                new()
                {
                    Title = "Jurnal Harian",
                    Subtitle = "Monitoring aktivitas harian & approval",
                    Icon = "📝",
                    Color = "#059669",
                    LightColor = "#ECFDF5",
                    ActionKey = "tulis_jurnal"
                },
                new()
                {
                    Title = "Kunjungan Monitoring",
                    Subtitle = "Data visit guru & monev lapangan",
                    Icon = "🚗",
                    Color = "#8B5CF6",
                    LightColor = "#F5F3FF",
                    ActionKey = "jadwal_monev"
                },
                new()
                {
                    Title = "Laporan Akhir Siswa",
                    Subtitle = "Verifikasi bab 1-5 & status kelulusan",
                    Icon = "📑",
                    Color = "#D97706",
                    LightColor = "#FFFBEB",
                    ActionKey = "upload_laporan"
                },
                new()
                {
                    Title = "Pengumuman Terpusat",
                    Subtitle = "Publikasi info resmi ke seluruh role",
                    Icon = "📢",
                    Color = "#0D9488",
                    LightColor = "#F0FDFA",
                    ActionKey = "pengumuman_info"
                },
                new()
                {
                    Title = "Ekspor & Pelaporan",
                    Subtitle = "Generate laporan PDF & Microsoft Excel",
                    Icon = "📊",
                    Color = "#6366F1",
                    LightColor = "#EEF2FF",
                    ActionKey = "ekspor_laporan"
                },
                new()
                {
                    Title = "Manajemen Aplikasi",
                    Subtitle = "Profil sekolah, logo & konfigurasi sistem",
                    Icon = "⚙️",
                    Color = "#475569",
                    LightColor = "#F1F5F9",
                    ActionKey = "admin_audit_log"
                }
            ],

            Activities =
            [
                new()
                {
                    Title = "Registrasi Akun Guru Baru Disetujui",
                    TimeAgo = "10 menit lalu",
                    Detail = "Akun Drs. Bambang Hidayat diverifikasi oleh Admin",
                    Icon = "✅",
                    Tag = "Auth",
                    TagColor = "#10B981"
                },
                new()
                {
                    Title = "Pembaruan Kuota DUDI PT Telkom Indonesia",
                    TimeAgo = "1 jam lalu",
                    Detail = "Kuota kuota ditambah dari 10 menjadi 15 siswa",
                    Icon = "🏢",
                    Tag = "Mitra",
                    TagColor = "#2563EB"
                },
                new()
                {
                    Title = "Backup Database Mingguan Berhasil",
                    TimeAgo = "Kemarin 23:59 WIB",
                    Detail = "Snapshot 42.8 MB disimpan ke cloud storage aman",
                    Icon = "💾",
                    Tag = "System",
                    TagColor = "#7C3AED"
                }
            ]
        };
    }

    private RoleDashboardData GetOperatorData(string userName)
    {
        return new RoleDashboardData
        {
            RoleTitle = "Operator PKL (Bursa Kerja & Hubin)",
            RoleSubtitle = "Koordinator Administrasi, Surat Tugas & Penempatan",
            WelcomeBannerTitle = $"Halo, {userName}! 📋",
            WelcomeBannerDesc = "Ada 13 siswa yang belum ditempatkan ke DUDI dan 8 dokumen berkas surat pengantar siap diterbitkan.",
            ShowProgressBar = true,
            ProgressTitle = "Capaian Penempatan Siswa (172 / 185 Siswa)",
            ProgressRatio = 0.93,
            ProgressText = "93% Siswa Sudah Ditempatkan di Mitra DUDI",

            StatCards =
            [
                new()
                {
                    Title = "Total Peserta",
                    Value = "185 Siswa",
                    Subtext = "Tahun Ajaran 2026/2027",
                    Icon = "🎓",
                    AccentColor = "#D97706",
                    LightBgColor = "#FFFBEB",
                    BadgeText = "Gelombang 1"
                },
                new()
                {
                    Title = "Terpenempatan",
                    Value = "172 Siswa",
                    Subtext = "93% dari total kuota",
                    Icon = "✅",
                    AccentColor = "#10B981",
                    LightBgColor = "#ECFDF5",
                    BadgeText = "93%"
                },
                new()
                {
                    Title = "Belum Ditempatkan",
                    Value = "13 Siswa",
                    Subtext = "Butuh konfirmasi DUDI alternatif",
                    Icon = "⚠️",
                    AccentColor = "#EF4444",
                    LightBgColor = "#FEF2F2",
                    BadgeText = "Perlu Aksi"
                },
                new()
                {
                    Title = "Berkas PKS & MoU",
                    Value = "8 Dokumen",
                    Subtext = "Menunggu tanda tangan kepala sekolah",
                    Icon = "📑",
                    AccentColor = "#2563EB",
                    LightBgColor = "#EFF6FF",
                    BadgeText = "Siap Cetak"
                }
            ],

            QuickActions =
            [
                new()
                {
                    Title = "Matching Siswa - DUDI",
                    Subtitle = "Plotting siswa ke tempat magang mitra",
                    Icon = "🤝",
                    Color = "#D97706",
                    LightColor = "#FFFBEB",
                    ActionKey = "matching_siswa"
                },
                new()
                {
                    Title = "Master Data & Impor",
                    Subtitle = "Data siswa, jurusan & impor berkas CSV",
                    Icon = "📋",
                    Color = "#7C3AED",
                    LightColor = "#F5F3FF",
                    ActionKey = "master_data"
                },
                new()
                {
                    Title = "Cetak Berkas & Surat",
                    Subtitle = "Generate surat tugas & pengantar resmi",
                    Icon = "🖨️",
                    Color = "#2563EB",
                    LightColor = "#EFF6FF",
                    ActionKey = "cetak_surat"
                },
                new()
                {
                    Title = "Rekap Nilai & Laporan",
                    Subtitle = "Ekspor kompilasi nilai DUDI & Sekolah",
                    Icon = "📊",
                    Color = "#10B981",
                    LightColor = "#ECFDF5",
                    ActionKey = "rekap_nilai"
                },
                new()
                {
                    Title = "Rekap Presensi Siswa",
                    Subtitle = "Monitoring kehadiran harian di industri",
                    Icon = "📍",
                    Color = "#059669",
                    LightColor = "#ECFDF5",
                    ActionKey = "presensi_siswa"
                },
                new()
                {
                    Title = "Pengumuman Hubin",
                    Subtitle = "Publikasi edaran resmi PKL & bursa kerja",
                    Icon = "📢",
                    Color = "#0D9488",
                    LightColor = "#F0FDFA",
                    ActionKey = "pengumuman_info"
                },
                new()
                {
                    Title = "Penerbitan Sertifikat",
                    Subtitle = "Generate e-sertifikat PKL berseri nasional",
                    Icon = "📜",
                    Color = "#8B5CF6",
                    LightColor = "#F5F3FF",
                    ActionKey = "cetak_sertifikat"
                }
            ],

            Documents =
            [
                new()
                {
                    DocumentTitle = "Surat Pengantar PKL Batch 2 (PT Telkom)",
                    TargetParty = "PT Telkom Indonesia",
                    DateFormatted = "24 Sep 2026",
                    Status = "Siap Cetak",
                    StatusColor = "#10B981",
                    StatusBgColor = "#ECFDF5",
                    ActionLabel = "Cetak PDF"
                },
                new()
                {
                    DocumentTitle = "Perjanjian Kerjasama (MoU) SMK - Bank Mandiri",
                    TargetParty = "PT Bank Mandiri (Persero) Tbk",
                    DateFormatted = "23 Sep 2026",
                    Status = "Reviu Legal",
                    StatusColor = "#D97706",
                    StatusBgColor = "#FEF3C7",
                    ActionLabel = "Unduh Draft"
                },
                new()
                {
                    DocumentTitle = "Surat Tugas Monitoring Guru Wilayah Jaksel",
                    TargetParty = "Drs. Bambang Hidayat, M.Kom",
                    DateFormatted = "22 Sep 2026",
                    Status = "Ditandatangani",
                    StatusColor = "#2563EB",
                    StatusBgColor = "#EFF6FF",
                    ActionLabel = "Lihat Berkas"
                }
            ]
        };
    }

    private RoleDashboardData GetMonitorData(string userName)
    {
        return new RoleDashboardData
        {
            RoleTitle = "Monitoring & Evaluasi (Monev / Pengawas)",
            RoleSubtitle = "Dinas Pendidikan Provinsi & Komite Mutu Vokasi",
            WelcomeBannerTitle = $"Dashboard Eksekutif Monev, {userName}! 📈",
            WelcomeBannerDesc = "Tinjauan performa makro pelaksanaan PKL SMK se-wilayah. Seluruh indikator berada pada zona hijau.",
            ShowProgressBar = true,
            ProgressTitle = "Indeks Keberhasilan & Kelulusan PKL",
            ProgressRatio = 0.984,
            ProgressText = "98.4% Capaian Sangat Memuaskan (Target 95%)",

            StatCards =
            [
                new()
                {
                    Title = "Kelulusan PKL",
                    Value = "98.4%",
                    Subtext = "Taraf pencapaian kompetensi industri",
                    Icon = "🎯",
                    AccentColor = "#0D9488",
                    LightBgColor = "#F0FDFA",
                    BadgeText = "Target Tercapai"
                },
                new()
                {
                    Title = "Kepuasan DUDI",
                    Value = "4.86 / 5.0",
                    Subtext = "Survei evaluasi dari 54 mitra industri",
                    Icon = "🌟",
                    AccentColor = "#F59E0B",
                    LightBgColor = "#FEF3C7",
                    BadgeText = "Sangat Baik"
                },
                new()
                {
                    Title = "Jam Praktik Vokasi",
                    Value = "14,280 Jam",
                    Subtext = "Akumulasi jam kerja industri siswa",
                    Icon = "⏱️",
                    AccentColor = "#2563EB",
                    LightBgColor = "#EFF6FF",
                    BadgeText = "Sesuai Standar"
                },
                new()
                {
                    Title = "Laporan Insiden",
                    Value = "0 Kasus",
                    Subtext = "Nol insiden keselamatan kerja (Zero K3)",
                    Icon = "🛡️",
                    AccentColor = "#10B981",
                    LightBgColor = "#ECFDF5",
                    BadgeText = "Zero Incident"
                }
            ],

            QuickActions =
            [
                new()
                {
                    Title = "Laporan Eksekutif",
                    Subtitle = "Unduh rangkuman analitik komprehensif",
                    Icon = "📑",
                    Color = "#0D9488",
                    LightColor = "#F0FDFA",
                    ActionKey = "unduh_laporan_monev"
                },
                new()
                {
                    Title = "Analisis Keselarasan",
                    Subtitle = "Link and Match kurikulum dengan kebutuhan DUDI",
                    Icon = "🔗",
                    Color = "#2563EB",
                    LightColor = "#EFF6FF",
                    ActionKey = "analisis_link_match"
                },
                new()
                {
                    Title = "Daftar Audit Mutu",
                    Subtitle = "Hasil checklist verifikasi berkas & lapangan",
                    Icon = "📋",
                    Color = "#8B5CF6",
                    LightColor = "#F5F3FF",
                    ActionKey = "audit_mutu"
                },
                new()
                {
                    Title = "Evaluasi Kemitraan",
                    Subtitle = "Rekomendasi perpanjangan kontrak DUDI",
                    Icon = "🏢",
                    Color = "#D97706",
                    LightColor = "#FFFBEB",
                    ActionKey = "evaluasi_kemitraan"
                },
                new()
                {
                    Title = "Monev Kunjungan Guru",
                    Subtitle = "Catatan & laporan visit guru ke DUDI",
                    Icon = "🚗",
                    Color = "#6366F1",
                    LightColor = "#EEF2FF",
                    ActionKey = "jadwal_monev"
                },
                new()
                {
                    Title = "Rekap Presensi Siswa",
                    Subtitle = "Pantau kedisiplinan dan absensi siswa",
                    Icon = "📍",
                    Color = "#10B981",
                    LightColor = "#ECFDF5",
                    ActionKey = "presensi_siswa"
                },
                new()
                {
                    Title = "Pengumuman Resmi",
                    Subtitle = "Pemberitahuan resmi dan kebijakan PKL",
                    Icon = "📢",
                    Color = "#059669",
                    LightColor = "#ECFDF5",
                    ActionKey = "pengumuman_info"
                }
            ],

            Activities =
            [
                new()
                {
                    Title = "Audit Lapangan PT Telkom Indonesia",
                    TimeAgo = "Hari ini 11:00 WIB",
                    Detail = "Fasilitas bimbingan & SOP keamanan dinilai Sangat Baik (Skor 96/100)",
                    Icon = "🏢",
                    Tag = "Monev Lapangan",
                    TagColor = "#0D9488"
                },
                new()
                {
                    Title = "Survei Kepuasan Industri Tahap 1 Selesai",
                    TimeAgo = "Kemarin 14:20 WIB",
                    Detail = "52 dari 54 DUDI merekomendasikan penerimaan lulusan SMK",
                    Icon = "📊",
                    Tag = "Survei Mutu",
                    TagColor = "#2563EB"
                },
                new()
                {
                    Title = "Sinkronisasi Data dengan Dapodik Vokasi",
                    TimeAgo = "2 hari lalu",
                    Detail = "Rekapitulasi 185 siswa magang telah tersinkron ke portal Kemendikbud",
                    Icon = "☁️",
                    Tag = "Sinkronisasi",
                    TagColor = "#7C3AED"
                }
            ]
        };
    }
}
