using Medieval_Internship.Views.Modules;

namespace Medieval_Internship
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(AbsensiModulePage), typeof(AbsensiModulePage));
            Routing.RegisterRoute(nameof(JurnalHarianModulePage), typeof(JurnalHarianModulePage));
            Routing.RegisterRoute(nameof(MonitoringKunjunganModulePage), typeof(MonitoringKunjunganModulePage));
            Routing.RegisterRoute(nameof(PengumumanModulePage), typeof(PengumumanModulePage));
            Routing.RegisterRoute(nameof(EksporLaporanModulePage), typeof(EksporLaporanModulePage));
            Routing.RegisterRoute(nameof(LaporanAkhirModulePage), typeof(LaporanAkhirModulePage));
            Routing.RegisterRoute(nameof(MasterDataModulePage), typeof(MasterDataModulePage));
            Routing.RegisterRoute(nameof(PenempatanPklModulePage), typeof(PenempatanPklModulePage));
            Routing.RegisterRoute(nameof(ManajemenAplikasiModulePage), typeof(ManajemenAplikasiModulePage));
        }
    }
}
