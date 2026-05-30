namespace proyectofinalQ2.DTOs;

public class CreateLabNoteDto
{
    public string Title { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; } 
    public bool IsPublic { get; set; }
    public string Tags { get; set; } = string.Empty; 
}