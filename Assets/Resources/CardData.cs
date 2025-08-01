using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Concentration/CardData", order = 1)]
public class CardData : ScriptableObject
{
    public string CardName;
    public Sprite CardFront; // The front image of the card
    public Sprite CardBack; // The shared back image
    public Suit Suit;
    public int Rank;
}