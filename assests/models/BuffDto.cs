public class BuffDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Describe { get; set; }
    public int Duration { get; set; }
    public Status CurrentStatus { get; set; }
    public delegate void BuffEffect(CharacterDto target);
    public enum Status
    {
        Triggered,
        NotTriggered,
        Triggering
    }

    public BuffDto(string id, string name, string describe, int duration, Status currentStatus)
    {
        Id = id;
        Name = name;
        Describe = describe;
        Duration = duration;
        CurrentStatus = currentStatus;
    }    
}