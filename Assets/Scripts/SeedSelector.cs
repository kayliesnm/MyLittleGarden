using UnityEngine;
using UnityEngine.UI;

public class SeedSelector : MonoBehaviour
{
    public GameObject seedPanel;
    public Button[] seedButtons;
    public Button selectButton;
    public Button cancelButton;
    public GameObject[] plantPrefabs;
    public GameObject[] seedVisualPrefabs;
    public string[] seedNames;


    private int selectedSeedIndex = -1;
    private DirtTile currentTile;

    void Awake()
    {
        seedNames = new string[] { "Carrot", "Daisy", "Tulip", "Tomato" };
    }

    private void Start()
    {
        seedPanel.SetActive(false);

        // Disable all seed buttons except the first one
        for (int i = 0; i < seedButtons.Length; i++)
        {
            int index = i;
            seedButtons[i].onClick.AddListener(() => SelectSeed(index));
            seedButtons[i].interactable = (i == 0); // Unlock only the first seed
        }

        selectButton.onClick.AddListener(ConfirmSelection);
        cancelButton.onClick.AddListener(CancelSelection);
    }

    public void OpenSeedMenu(DirtTile tile)
    {
        currentTile = tile;
        selectedSeedIndex = -1;
        seedPanel.SetActive(true);
    }

    void SelectSeed(int index)
    {
        selectedSeedIndex = index;
        Debug.Log("Selected seed index: " + index);
    }

    void ConfirmSelection()
    {
        if (selectedSeedIndex != -1 && currentTile != null)
        {
            if (selectedSeedIndex < Player.instance.seedPrefabs.Length &&
                selectedSeedIndex < Player.instance.seedVisualPrefabs.Length)
            {
                currentTile.PlantSeed(
                    Player.instance.seedPrefabs[selectedSeedIndex],
                    Player.instance.seedVisualPrefabs[selectedSeedIndex],
                    selectedSeedIndex
                );
            }
            else
            {
                Debug.LogWarning("Selected seed index is out of range of seed arrays!");
            }
        }
        else
        {
            Debug.LogWarning("No seed selected or no dirt tile targeted.");
        }

        seedPanel.SetActive(false);
    }

    void CancelSelection()
    {
        selectedSeedIndex = -1;
        currentTile = null;
        seedPanel.SetActive(false);
    }

    public void UnlockSeed(int index)
    {
        if (index >= 0 && index < seedButtons.Length)
        {
            seedButtons[index].interactable = true;
            Debug.Log("Seed unlocked at index: " + index);
        }
    }

    // Get names for notification
    public string GetSeedName(int index)
    {
        if (index >= 0 && index < seedNames.Length)
            return seedNames[index];
        return "New";
    }
}
