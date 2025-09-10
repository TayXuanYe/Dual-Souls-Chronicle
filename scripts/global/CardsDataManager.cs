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
		foreach (var buffData in BuffManager.Instance.Buffs)
		{
			string name = buffData.Value.Name;
			string describe = buffData.Value.Describe;
			string imagePath = buffData.Value.ImagePath;
			var card1 = new CardModel("buff_card1", name, ResourceLoader.Load<Texture2D>(imagePath), describe);
			BuffCards.Add((card1, false));
		}
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

	public void OnSelectBuffSignalReceipt(string cardId, string parentGroupName)
	{
		for (int i = 0; i < BuffCards.Count; i++)
		{
			if (BuffCards[i].card.Id == cardId)
			{
				if (BuffCards[i].IsSelect)
				{
					// signal to show buff destroy effect
					SignalManager.Instance.EmitRemoveBuffCharacterSignal(cardId, parentGroupName);
					break;
				}
				else
				{
					// signal add buff
					SignalManager.Instance.EmitAddBuffCharacterSignal(cardId, parentGroupName);
					BuffCards[i] = (BuffCards[i].card, true);
					break;
				}
			}
		}
	}
}
