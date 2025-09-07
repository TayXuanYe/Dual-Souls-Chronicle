using Godot;
using System;

public class CardDto
{
    public string Name { get; set; }
    public Texture2D ImageTexture { get; set; }
    public string Describe;

    public CardDto(string name, Texture2D imageTexture, string describe)
    {
        Name = name;
        ImageTexture = imageTexture;
        Describe = describe;
    }
}