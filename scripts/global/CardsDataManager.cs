using Godot;
using System;
using System.Collections.Generic;

public partial class CardsDataManager : Node
{
    private static CardsDataManager _instance;
    public static CardsDataManager Instance => _instance;

    public List<(CardDto card, bool IsSelect)> BuffCards { get; private set; } = new List<(CardDto, bool)>();

    public override void _Ready()
    {
        _instance = this;
        LoadBuffCards();
    }

    private void LoadBuffCards()
    {
        // 1
        string name = "card1";
        string describe = "card1";
        string imagePath = "res://assests/textures/buff/buff_1.png";
        var card1 = new CardDto(0, name, ResourceLoader.Load<Texture2D>(imagePath), describe);
        BuffCards.Add((card1, false));

        //2
        name = "card2";
        describe = "card2";
        imagePath = "res://assests/textures/buff/buff_2.png";
        var card2 = new CardDto(1, name, ResourceLoader.Load<Texture2D>(imagePath), describe);
        BuffCards.Add((card2, false));

        //3
        name = "card3";
        describe = "card3";
        imagePath = "res://assests/textures/buff/buff_3.png";
        var card3 = new CardDto(2, name, ResourceLoader.Load<Texture2D>(imagePath), describe);
        BuffCards.Add((card3, false));

        //4
        name = "card4";
        describe = "card4";
        imagePath = "res://assests/textures/buff/buff_4.png";
        var card4 = new CardDto(3, name, ResourceLoader.Load<Texture2D>(imagePath), describe);
        BuffCards.Add((card4, false));
    }

    public List<CardDto> GetBuffCards(int amount, int seed)
    {
        Random random = new Random(seed);
        HashSet<int> uniqueNumbers = new HashSet<int>();
        if (BuffCards.Count < amount)
        {
            amount = BuffCards.Count;
        }
        
        while (uniqueNumbers.Count < amount)
        {
            int randomNumber = random.Next(0, BuffCards.Count);
            uniqueNumbers.Add(randomNumber);
        }
        List<CardDto> returnValue = new List<CardDto>();
        foreach (int num in uniqueNumbers)
        {
            returnValue.Add(BuffCards[num].card);
        }

        return returnValue;
    }  
}
