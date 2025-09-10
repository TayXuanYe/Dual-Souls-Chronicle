using Godot;
using System;

public class CardModel
{
    public string Id { get; set; }
    public string CarryData { get; set; }
    public string CardName { get; set; }
    public Texture2D ImageTexture { get; set; }
    public string Describe;

    public CardModel(string id, string carryData,string name, Texture2D imageTexture, string describe)
    {
        Id = id;
        CarryData = carryData;
        CardName = name;
        ImageTexture = imageTexture;
        Describe = describe;
    }
}