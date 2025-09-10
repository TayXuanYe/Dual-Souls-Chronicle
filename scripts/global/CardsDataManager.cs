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
			string id = $"card_{buffData.Value.Id}";
			string name = buffData.Value.Name;
			string describe = buffData.Value.Describe;
			string imagePath = buffData.Value.ImagePath;
			var card1 = new CardModel(id, name, ResourceLoader.Load<Texture2D>(imagePath), describe);
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
	public List<CardModel> GetCharacterCards(int amount)
	{
		List<CardModel> returnValue = new List<CardModel>();
		returnValue.Add(ConvertCharacterModelToCardModel(CharacterDataManager.Warrior));
		returnValue.Add(ConvertCharacterModelToCardModel(CharacterDataManager.Mage));
		returnValue.Add(ConvertCharacterModelToCardModel(CharacterDataManager.ShieldGuard));
		return null;
	}
	private CardModel ConvertCharacterModelToCardModel(CharacterModel characterModel)
	{
		string cardId = $"card_{characterModel.Id}";
		string name = characterModel.CharacterName;
		string describe = characterModel.Describe;
		string imagePath = characterModel.ImagePath;
		Texture2D imageTexture = ResourceLoader.Load<Texture2D>(imagePath);

		CardModel cardModel = new CardModel(cardId, name, imageTexture, describe);
		return cardModel;
	}
	private string parentGroup1SelectBuffCardId;
	private string parentGroup2SelectBuffCardId;
	public void OnSelectBuffSignalReceipt(string buffId, string parentGroupName)
	{
		switch (parentGroupName)
		{
			case "IsInViewport1":
				parentGroup1SelectBuffCardId = $"card_{buffId}";
				break;
			case "IsInViewport2":
				parentGroup2SelectBuffCardId = $"card_{buffId}";
				break;
		}
		
		if (string.IsNullOrEmpty(parentGroup1SelectBuffCardId) || string.IsNullOrEmpty(parentGroup2SelectBuffCardId))
		{
			return;
		}

		if (parentGroup1SelectBuffCardId == parentGroup2SelectBuffCardId)
		{
			parentGroup1SelectBuffCardId = null;
			parentGroup2SelectBuffCardId = null;
			return;
		}
		
		for (int i = 0; i < BuffCards.Count; i++)
		{
			if (BuffCards[i].card.Id == $"card_{buffId}")
			{
				// signal add buff
				SignalManager.Instance.EmitAddBuffCharacterSignal(buffId, parentGroupName);
				BuffCards[i] = (BuffCards[i].card, true);
				break;
			}
		}
	}
}
