using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDeckData", menuName = "Concentration/Deck", order = 2)]
public class DeckData : ScriptableObject
{
    public string DeckName;
    public List<CardData> Cards = new List<CardData>();
}