using System;

public class BuffModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Describe { get; set; }
    public Status CurrentStatus { get; set; }
    public Action<CharacterModel> OnApply { get; set; }
    public Action<CharacterModel> OnRemove { get; set; }
    public string ImagePath { get; set; }

    public enum Status
    {
        Triggered,
        NotTriggered
    }

    public BuffModel(string id, string name, string describe, Action<CharacterModel> onApply, Action<CharacterModel> onRemove, string imagePath)
    {
        Id = id;
        Name = name;
        Describe = describe;
        CurrentStatus = Status.NotTriggered;
        OnApply = onApply;
        OnRemove = onRemove;
        ImagePath = imagePath;
    }
}