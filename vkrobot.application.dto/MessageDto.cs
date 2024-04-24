namespace vkborot.application.dto;

public class MessageDto
{
    public Guid? Id { get; set; }
    public string? UserId { get; set; }
    public string? GroupId { get; set; }
    public string? MessageText { get; set; }
    public DateTime? MessageDate { get; set; }
}