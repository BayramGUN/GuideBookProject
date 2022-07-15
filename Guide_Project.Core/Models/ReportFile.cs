using System.ComponentModel.DataAnnotations.Schema;

namespace Guide_Project.Core.Models;

public class ReportFile
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set;}
    public int UserId { get; set;}
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool FileStatus { get; set; }  
}