using Godot;
using System;

public class CardDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Texture2D ImageTexture { get; set; }
    public string Describe;

    public CardDto(string id,string name, Texture2D imageTexture, string describe)
    {
        Id = id;
        Name = name;
        ImageTexture = imageTexture;
        Describe = describe;
    }
}