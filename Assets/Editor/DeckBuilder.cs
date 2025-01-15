using UnityEditor;
using UnityEngine;

public class DeckBuilderWindow : EditorWindow
{
    private DeckData deckData;

    [MenuItem("Tools/Concentration/Deck Builder")]
    public static void ShowWindow()
    {
        GetWindow<DeckBuilderWindow>("Deck Builder");
    }

    private void OnGUI()
    {
        // Select or create a DeckData ScriptableObject
        deckData = (DeckData)EditorGUILayout.ObjectField("Deck Data", deckData, typeof(DeckData), false);

        if (deckData == null)
        {
            EditorGUILayout.HelpBox("Select or create a DeckData asset to edit.", MessageType.Info);
            if (GUILayout.Button("Create New Deck"))
            {
                CreateNewDeck();
            }
            return;
        }

        // Display and manage cards in the deck
        EditorGUILayout.LabelField("Deck: " + deckData.DeckName, EditorStyles.boldLabel);

        if (deckData.Cards != null && deckData.Cards.Count > 0)
        {
            for (int i = 0; i < deckData.Cards.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                deckData.Cards[i] = (CardData)EditorGUILayout.ObjectField(deckData.Cards[i], typeof(CardData), false);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    deckData.Cards.RemoveAt(i);
                    EditorUtility.SetDirty(deckData); // Mark as dirty after removal
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No cards in this deck. Click 'Add Card' to start adding cards.", MessageType.Info);
        }

        if (GUILayout.Button("Add Card"))
        {
            // Open the Card Builder to create a new card
            CardBuilderEditor.ShowWindow(deckData, (cardData) =>
            {
                // After a card is saved, add it to the deck and mark the deck as dirty
                // deckData.Cards.Add(cardData);
                EditorUtility.SetDirty(deckData); // Mark the asset as dirty for saving
                AssetDatabase.SaveAssets(); // Ensure changes are saved
            });
        }

        // Optionally add a shuffle button to shuffle cards in the deck
        if (GUILayout.Button("Shuffle Deck"))
        {
            ShuffleDeck(deckData);
        }
    }

    private void CreateNewDeck()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save New Deck", "NewDeckData", "asset", "Save your deck asset.");
        if (!string.IsNullOrEmpty(path))
        {
            DeckData newDeck = CreateInstance<DeckData>();
            AssetDatabase.CreateAsset(newDeck, path);
            AssetDatabase.SaveAssets();
            deckData = newDeck;
        }
    }

    private void ShuffleDeck(DeckData deck)
    {
        for (int i = deck.Cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            var temp = deck.Cards[i];
            deck.Cards[i] = deck.Cards[randomIndex];
            deck.Cards[randomIndex] = temp;
        }
    }
}
