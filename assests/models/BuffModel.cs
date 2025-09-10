using System;

public class BuffModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Describe { get; set; }
    public int Duration { get; set; }
    public Status CurrentStatus { get; set; }
    public Action<CharacterModel> OnApply { get; set; }
    public Action<CharacterModel> OnRemove { get; set; }

    public enum Status
    {
        Triggered,
        NotTriggered
    }

    public BuffModel(string id, string name, string describe, int duration, Status currentStatus, Action<CharacterModel> onApply, Action<CharacterModel> onRemove)
    {
        Id = id;
        Name = name;
        Describe = describe;
        Duration = duration;
        CurrentStatus = currentStatus;
        OnApply = onApply;
        OnRemove = onRemove;
    }
}