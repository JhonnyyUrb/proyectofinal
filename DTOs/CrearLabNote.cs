namespace proyectofinalQ2.DTOs;

public class CrearLabNote
{
    public string Title { get; set; } = "";
    public string Observation { get; set; } = "";
    public string Category { get; set; } = "";
    public int Priority { get; set; }
    public bool IsPublic { get; set; }
    public string Tags { get; set; } = "";
}