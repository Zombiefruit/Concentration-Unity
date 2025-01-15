using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Concentration/Card Data", order = 1)]
public class CardData : ScriptableObject
{
    public string CardName; // Example: "Ace of Spades"
    public Sprite CardFront; // The front image of the card
    public Sprite CardBack; // The shared back image
    public string Suit; // Example: "Hearts", "Spades"
    public int Rank; // Example: 1 for Ace, 11 for Jack, etc.
}