namespace Medieval_Internship.Models;

public class StatCardModel
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Subtext { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string AccentColor { get; set; } = "#2563EB";
    public string LightBgColor { get; set; } = "#EFF6FF";
    public string BadgeText { get; set; } = string.Empty;
}

public class QuickActionModel
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = "#1E3A5F";
    public string LightColor { get; set; } = "#F1F5F9";
    public string ActionKey { get; set; } = string.Empty;
}

public class StudentItemModel
{
    public string Name { get; set; } = string.Empty;
    public string NisnOrId { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string AttendancePercent { get; set; } = string.Empty;
    public string LastJournalStatus { get; set; } = string.Empty;
    public string StatusBadgeColor { get; set; } = "#10B981";
    public string StatusLightColor { get; set; } = "#ECFDF5";
    public double Progress { get; set; } = 0.5;
}

public class JournalItemModel
{
    public string Title { get; set; } = string.Empty;
    public string DateFormatted { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = "#10B981";
    public string StatusBgColor { get; set; } = "#ECFDF5";
    public string StudentName { get; set; } = string.Empty;
}

public class DocumentItemModel
{
    public string DocumentTitle { get; set; } = string.Empty;
    public string TargetParty { get; set; } = string.Empty;
    public string DateFormatted { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = "#2563EB";
    public string StatusBgColor { get; set; } = "#EFF6FF";
    public string ActionLabel { get; set; } = "Lihat Berkas";
}

public class ActivityLogModel
{
    public string Title { get; set; } = string.Empty;
    public string TimeAgo { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string Icon { get; set; } = "📌";
    public string Tag { get; set; } = string.Empty;
    public string TagColor { get; set; } = "#64748B";
}
