namespace Guide_Project.Core.DTOs;

public class ReportFileDto
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string GetCreatedDate => CreatedDate.HasValue ? CreatedDate.Value.ToShortDateString() : "-";
}