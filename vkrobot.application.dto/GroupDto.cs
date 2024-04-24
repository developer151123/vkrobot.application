namespace vkborot.application.dto;

public class GroupDto
{
    public Guid? Id { get; set; }
    public string? GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    public DateTime? LastScan { get; set; }
    public string? ErrorText { get; set; }
    public bool? Private { get; set; }
}

public class ImportGroupDto
{
    public string? GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    public bool? Private { get; set; }
}
