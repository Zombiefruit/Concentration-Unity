using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using Unity.AppUI.Core;
using Unity.AppUI.UI;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using AnimationMode = Unity.AppUI.UI.AnimationMode;
using Avatar = Unity.AppUI.UI.Avatar;
using Button = Unity.AppUI.UI.Button;
using Toggle = Unity.AppUI.UI.Toggle;
using Dropdown = Unity.AppUI.UI.Dropdown;
using TextField = Unity.AppUI.UI.TextField;
using EnumField = Unity.AppUI.UI.EnumField;
using MenuItem = UnityEditor.MenuItem;
using IntegerField = Unity.AppUI.UI.IntField;
// using ObjectField = Unity.AppUI.UI.UnityObject;

public class CardBuilderEditor : EditorWindow
{
    private const string k_DefaultTheme = "Packages/com.unity.dt.app-ui/PackageResources/Styles/Themes/App UI.tss";
    private string m_CurrentTheme = "dark";

    private string m_CurrentScale = "medium";
    private Dir m_CurrentLayoutDirection;
    private string m_CurrentLocaleId = "en";

    // UI elements
    private TextField _cardNameField;
    private EnumField _suitField;
    private IntegerField _rankField;
    private ObjectField _frontSpriteField;
    private ObjectField _backSpriteField;
    private Button _saveButton;

    private DeckData _deckData; // Reference to the deck of cards
    private System.Action<CardData> _onCardSavedCallback; // Callback to return saved card

    // Launch Card Builder as a standalone window
    [MenuItem("Tools/Concentration/Card Builder")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<CardBuilderEditor>();
        wnd.titleContent = new GUIContent("Card Builder");
        wnd.Show();
    }

    // Launch Card Builder from DeckBuilder with a callback
    public static void ShowWindow(DeckData deckData, System.Action<CardData> onCardSavedCallback)
    {
        var wnd = GetWindow<CardBuilderEditor>();
        wnd.titleContent = new GUIContent("Card Builder");
        wnd._deckData = deckData;
        wnd._onCardSavedCallback = onCardSavedCallback;
        wnd.Show();
    }

    void CreateGUI()
    {
        const string defaultTheme = "Packages/com.unity.dt.app-ui/PackageResources/Styles/Themes/App UI.tss";
        rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(defaultTheme));
        rootVisualElement.AddToClassList("unity-editor"); // Enable Editor related styles
    }

    private void OnEnable()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CardBuilder.uxml");
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/CardBuilder.uss");
        var theme = AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(k_DefaultTheme);
        Debug.Log(styleSheet);
        var root = rootVisualElement;
        // Panel panel = new Panel
        // {
        //     theme = m_CurrentTheme,
        //     scale = m_CurrentScale,
        //     layoutDirection = m_CurrentLayoutDirection,
        //     lang = m_CurrentLocaleId
        // };
        // panel.AddToClassList("unity-editor");
        root.AddToClassList("unity-editor");

        // root.Add(panel);

        // VisualElement visualElement = base.rootVisualElement;

        // root.Add(visualElement);


        // Clear existing content
        // root.Clear();

        // // Create App UI Panel as the root container - this provides theme context
        // var appPanel = new Panel();
        // appPanel.name = "app-ui-root";
        // appPanel.style.flexGrow = 1;
        // root.Add(appPanel);
        root.styleSheets.Add(
                    AssetDatabase.LoadAssetAtPath<StyleSheet>(AssetDatabase.GUIDToAssetPath("b763f743b4824058b4e329c1a2592529")));

        // Load and apply styles
        if (visualTree != null)
        {
            visualTree.CloneTree(root);
        }

        if (styleSheet != null)
        {
            root.styleSheets.Add(styleSheet);
        }



        // Get references to UI elements
        _cardNameField = root.Q<TextField>("CardNameField");
        _suitField = root.Q<EnumField>("SuitField");
        _rankField = root.Q<IntegerField>("RankField");
        _frontSpriteField = root.Q<ObjectField>("FrontSpriteField");
        _backSpriteField = root.Q<ObjectField>("BackSpriteField");
        _saveButton = root.Q<Button>("SaveCardButton");

        // Set EnumField default value
        // _suitField.defaultValue = Suit.Hearts;
        _suitField.value = Suit.Hearts;

        // Configure ObjectFields for sprite selection
        _frontSpriteField.objectType = typeof(Sprite);
        _backSpriteField.objectType = typeof(Sprite);



        // Set up save button click handler
        _saveButton.clicked += OnSaveCardClicked;
    }

    private void OnSaveCardClicked()
    {
        // Get selected sprites
        var frontSprite = _frontSpriteField.value as Sprite;
        var backSprite = _backSpriteField.value as Sprite;

        if (frontSprite == null || backSprite == null)
        {
            EditorUtility.DisplayDialog("Missing Sprites", "Please assign both a front and back sprite.", "OK");
            return;
        }

        // Create a new card
        var newCard = CreateInstance<CardData>();
        newCard.CardName = _cardNameField.value;
        newCard.Suit = (Suit)_suitField.value;
        newCard.Rank = _rankField.value;
        newCard.CardFront = frontSprite;
        newCard.CardBack = backSprite;

        // Ask the user for a save path
        string path = EditorUtility.SaveFilePanelInProject(
            "Save New Card",
            $"{newCard.CardName}.asset",
            "asset",
            "Save your new card asset."
        );

        if (string.IsNullOrEmpty(path))
        {
            // User canceled the save dialog
            return;
        }

        // Save the card as an asset
        AssetDatabase.CreateAsset(newCard, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Add the new card to the deck
        _deckData.Cards.Add(newCard);

        // Mark the deck as dirty and save the asset
        EditorUtility.SetDirty(_deckData);

        // If there's a callback, call it with the new card
        _onCardSavedCallback?.Invoke(newCard);

        // Close the Card Builder window
        Close();
    }

}
