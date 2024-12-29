using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class CharacterData
{
    public string name;
    public GameObject prefab;
    public Sprite icon;
    public int cost;
    public float cooldown;
    [Tooltip("The position offset where this character should spawn relative to click position")]
    public Vector3 spawnOffset = Vector3.zero;
}

public class CharacterSelector : MonoBehaviour
{
    [Header("Character Options")]
    [SerializeField] private List<CharacterData> availableCharacters;
    [SerializeField] private int maxSelections = 3;

    [Header("UI References")]
    [SerializeField] private GameObject selectionScreen;
    [SerializeField] private GameObject characterButtonPrefab;
    [SerializeField] private Transform characterButtonContainer;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TextMeshProUGUI selectionCountText;

    private List<CharacterData> selectedCharacters = new List<CharacterData>();

    private void Start()
    {
        InitializeCharacterButtons();
        UpdateUI();
        startGameButton.onClick.AddListener(StartGame);
    }

    private void InitializeCharacterButtons()
    {
        foreach (var character in availableCharacters)
        {
            GameObject buttonObj = Instantiate(characterButtonPrefab, characterButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI nameText = buttonObj.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            icon.sprite = character.icon;
            nameText.text = character.name;

            button.onClick.AddListener(() => ToggleCharacterSelection(character, buttonObj));
        }
    }

    private void ToggleCharacterSelection(CharacterData character, GameObject buttonObj)
    {
        if (selectedCharacters.Contains(character))
        {
            selectedCharacters.Remove(character);
            buttonObj.GetComponent<Image>().color = Color.white;
        }
        else if (selectedCharacters.Count < maxSelections)
        {
            selectedCharacters.Add(character);
            buttonObj.GetComponent<Image>().color = Color.green;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        selectionCountText.text = $"Selected: {selectedCharacters.Count}/{maxSelections}";
        startGameButton.interactable = selectedCharacters.Count <= maxSelections;
    }

    private void StartGame()
    {
        // Ensure the SelectionScreen has a CanvasGroup
        CanvasGroup canvasGroup = selectionScreen.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("SelectionScreen is missing a CanvasGroup component!");
            return;
        }

        // Disable the selection screen
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        selectionScreen.SetActive(false);

        // Notify the GameManager to start the game
        GameManager.Instance.StartGameWithCharacters(selectedCharacters);
        Time.timeScale = 1;
    }
}
