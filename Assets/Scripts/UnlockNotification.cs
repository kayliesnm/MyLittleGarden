using UnityEngine;
using TMPro;
using System.Collections;
public class UnlockNotification : MonoBehaviour
{
    public GameObject UnLockPanel;
    public TextMeshProUGUI messageText;
    public float displayDuration = 3f;

    public static UnlockNotification instance;

    void Awake()
    {
        instance = this;
        UnLockPanel.SetActive(false); 
    }

    public void ShowUnlockMessage(string seedName)
    {
        Debug.Log("Showing unlock notification for: " + seedName);
        messageText.text = $"You've unlocked {seedName} seed!";
        UnLockPanel.SetActive(true);
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        UnLockPanel.SetActive(false);
    }

}
