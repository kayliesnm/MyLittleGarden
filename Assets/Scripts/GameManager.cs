using UnityEngine;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int harvestCount = 0;
    public int[] unlockThresholds = { 3, 6, 10 }; // Harvests required to unlock next seeds
    public int HarvestCount => harvestCount;
    public AudioSource harvestSound;
    public AudioSource backgroundMusic;
    public TextMeshProUGUI Count;

    
    void Awake()
    {
        instance = this;
    }

    [System.Obsolete]
    public void IncrementHarvestCount()
    {
        harvestCount++;
        Debug.Log("Harvest count increased to: " + harvestCount);

        // Update UI
        if (Count != null)
        {
            Count.text = $"Harvested {harvestCount} Time(s) Total";
        }

        if (harvestSound != null)
        {
            harvestSound.Play();
        }

        if (Player.instance.unlockedSeedCount < 4 && harvestCount >= unlockThresholds[Player.instance.unlockedSeedCount - 1])
        {
            Player.instance.unlockedSeedCount++;
            Debug.Log("Unlocked new seed!");

            SeedSelector selector = FindObjectOfType<SeedSelector>();
            if (selector != null)
            {
                // Unlock seed
                selector.UnlockSeed(Player.instance.unlockedSeedCount - 1); 
            }

            UnlockNotification notifier = FindObjectOfType<UnlockNotification>();
            if (notifier != null)
            {
                string seedName = selector.GetSeedName(Player.instance.unlockedSeedCount - 1);
                notifier.ShowUnlockMessage(seedName);
            }
        }

    }
}
