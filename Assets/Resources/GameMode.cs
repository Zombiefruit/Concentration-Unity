using UnityEngine;

[CreateAssetMenu(fileName = "NewGameMode", menuName = "Concentration/Game Data/Game Mode", order = 4)]
public class GameMode : ScriptableObject
{
    public string ModeName;
    public GameDifficulty[] Difficulties;
    public DeckData Deck;

    // Timer Settings
    public bool UseTimer;
    public bool TimerCountsDown;
    public float DefaultTimeLimit;

    // Scoring Rules
    public int PointsPerMatch;
    public int PenaltyPerMismatch;

    // Victory Condition
    public int MatchesRequiredToWin;
}
