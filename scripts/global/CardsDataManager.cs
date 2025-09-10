using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CardsDataManager : Node
{
    private static CardsDataManager _instance;
    public static CardsDataManager Instance => _instance;

    public List<(CardModel card, bool IsSelect)> BuffCards { get; private set; } = new List<(CardModel, bool)>();

    public override void _Ready()
    {
        _instance = this;
        LoadBuffCards();
        SignalManager.Instance.SelectBuff += OnSelectBuffSignalReceipt;
    }

    private void LoadBuffCards()
    {
        // 1
        string name = "card1";
        string describe = "card1";
        string imagePath = "res://assests/textures/buff/buff_1.png";
        var card1 = new CardModel("buff_card1", name, ResourceLoader.Load<Texture2D>(imagePath), describe);
        BuffCards.Add((card1, false));

        //2
        name = "card2";
        describe = "card2";
        imagePath = "res://assests/textures/buff/buff_2.png";
        var card2 = new CardModel("buff_card2", name, ResourceLoader.Load<Texture2D>(imagePath), describe);
        BuffCards.Add((card2, false));

        //3
        name = "card3";
        describe = "card3";
        imagePath = "res://assests/textures/buff/buff_3.png";
        var card3 = new CardModel("buff_card3", name, ResourceLoader.Load<Texture2D>(imagePath), describe);
        BuffCards.Add((card3, false));

        //4
        name = "card4";
        describe = "card4";
        imagePath = "res://assests/textures/buff/buff_4.png";
        var card4 = new CardModel("buff_card4", name, ResourceLoader.Load<Texture2D>(imagePath), describe);
        BuffCards.Add((card4, false));
    }

    public List<CardModel> GetBuffCards(int amount, int seed)
    {
        Random random = new Random(seed);
        var unselectedCards = BuffCards
            .Select((item, index) => new { item.card, item.IsSelect, index })
            .Where(x => !x.IsSelect)
            .ToList();

        if (unselectedCards.Count < amount)
        {
            amount = unselectedCards.Count;
        }

        HashSet<int> uniqueNumbers = new HashSet<int>();
        while (uniqueNumbers.Count < amount)
        {
            int randomNumber = random.Next(0, unselectedCards.Count);
            uniqueNumbers.Add(randomNumber);
        }

        List<CardModel> returnValue = new List<CardModel>();
        foreach (int num in uniqueNumbers)
        {
            returnValue.Add(unselectedCards[num].card);
        }

        return returnValue;
    }
    public List<CardModel> GetCharacterCards(int amount, int seed)
    {

        return null;
    }

        public void OnSelectBuffSignalReceipt(string cardId, int id)
        {
            for (int i = 0; i < BuffCards.Count; i++)
            {
                if (BuffCards[i].card.Id == cardId)
                {
                    if (BuffCards[i].IsSelect)
                    {
                        // signal to show buff destroy effect
                        break;
                    }
                    else
                    {
                        // signal add buff
                        BuffCards[i] = (BuffCards[i].card, true);
                        
                        break;
                    }
                }
            }
        }
}
