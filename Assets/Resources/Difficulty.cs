using UnityEngine;

[CreateAssetMenu(fileName = "NewGameDifficulty", menuName = "Concentration/Game Data/Difficulty", order = 3)]
public class GameDifficulty : ScriptableObject
{
    public string DifficultyName;
    public int NumberOfCards; // must be even
    public float TimerDuration;
}