using UnityEngine;
using TMPro;

public class DirtTile : MonoBehaviour
{
    public GameObject waterEffect;
    public GameObject waterEffectPrefab;
    public AudioSource wateringSound;
    public GameObject waterTimerUIPrefab;

    private GameObject plantInstance;
    private GameObject seedVisualInstance;
    private GameObject waterTimerUIInstance;
    private TextMeshProUGUI  timerText;

    private float growTime = 5f;
    private float growthTimer = 0f;
    private float waterTimer = 60f;

    private bool isPlanted = false;
    private bool isWatered = false;
    private bool isHarvestable = false;
    private bool isDead = false;
    private bool hasTimedOut = false;

    public bool IsHarvestable => isHarvestable;

    public void PlantSeed(GameObject plantPrefab, GameObject seedVisualPrefab, int seedIndex)
    {
        if (isPlanted) return;

        isPlanted = true;
        isWatered = false;
        isHarvestable = false;;
        hasTimedOut = false;

        // Place seed to this position
        Vector3 seedPos = transform.position + new Vector3(1f, 0.1f, 0f);
        seedVisualInstance = Instantiate(seedVisualPrefab, seedPos, Quaternion.identity, transform);
        Vector3 spawnPos = transform.position + new Vector3(1f, 0f, 0f);
        plantInstance = Instantiate(plantPrefab, spawnPos, Quaternion.identity, transform);
        plantInstance.transform.localScale = Vector3.zero;

        Debug.Log("Planted seed index: " + seedIndex);

        StartWaterTimerUI();
    }

    // Timer UI
    private void StartWaterTimerUI()
    {
        if (waterTimerUIPrefab != null)
        {
            Vector3 uiPos = transform.position + new Vector3(1.5f, 2f, .5f);
            waterTimerUIInstance = Instantiate(waterTimerUIPrefab, uiPos, Quaternion.identity, transform);
            Transform timerTextTransform = waterTimerUIInstance.transform.Find("TimerText");
            if (timerTextTransform != null)
            {
                timerText = timerTextTransform.GetComponent<TextMeshProUGUI>();
                if (timerText != null)
                    timerText.text = "60";
                else
                    Debug.LogWarning("Text component not found on TimerText");
            }
            else
            {
                Debug.LogWarning("TimerText child not found under WaterTimerUIInstance.");
            }

            if (timerText != null)
            {
                timerText.text = "60";
                Debug.Log("Water timer UI started.");
            }
            else
            {
                Debug.LogWarning("No Text component found in WaterTimerUIPrefab.");
            }
        }
        else
        {
            Debug.LogWarning("WaterTimerUIPrefab is not assigned.");
        }
    }

    public void Water()
    {
        if (!isPlanted || isWatered || isDead) return;

        isWatered = true;

        if (wateringSound != null)
            wateringSound.Play();

        if (waterEffectPrefab != null)
        {
            Vector3 fxPos = transform.position + new Vector3(1f, 0.1f, 0f);
            GameObject fx = Instantiate(waterEffectPrefab, fxPos, Quaternion.identity);
            Destroy(fx, 2f);
        }

        if (seedVisualInstance != null)
        {
            Destroy(seedVisualInstance);
            seedVisualInstance = null;
        }

        if (waterTimerUIInstance != null)
        {
            Destroy(waterTimerUIInstance);
            waterTimerUIInstance = null;
        }

        Debug.Log("Watered plant on tile: " + name);
    }

        void Update()
    {
        // Grow plant after watering
        if (isPlanted && isWatered && !isHarvestable && plantInstance != null)
        {
            growthTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(growthTimer / growTime);
            plantInstance.transform.localScale = Vector3.one * progress;

            if (progress >= 1f)
                isHarvestable = true;
        }

        // Countdown water timer before plant dies
        if (isPlanted && !isWatered && !hasTimedOut)
        {
            waterTimer -= Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = Mathf.CeilToInt(waterTimer).ToString();
                Debug.Log($"[Countdown] {gameObject.name}: {waterTimer:F2} seconds remaining");
            }

            if (waterTimer <= 0f)
            {
                hasTimedOut = true;

                if (plantInstance) Destroy(plantInstance);
                if (seedVisualInstance) Destroy(seedVisualInstance);
                if (waterTimerUIInstance) Destroy(waterTimerUIInstance);

                ResetTile();
                Debug.Log($"Plant died due to no water.");
            }
        }
    }


    [System.Obsolete]
    public void Harvest()
    {
        if (!isHarvestable) return;

        Destroy(plantInstance);
        ResetTile();

        GameManager.instance.IncrementHarvestCount();
        Debug.Log("Harvested a plant! Total harvests: " + GameManager.instance.HarvestCount);
    }

    private void ResetTile()
    {
        isPlanted = false;
        isWatered = false;
        isHarvestable = false;
        isDead = false;
        hasTimedOut = false;
        growthTimer = 0f;
        waterTimer = 60f;

        if (plantInstance) Destroy(plantInstance);
        if (seedVisualInstance) Destroy(seedVisualInstance);
        if (waterTimerUIInstance) Destroy(waterTimerUIInstance);

        plantInstance = null;
        seedVisualInstance = null;
        waterTimerUIInstance = null;
        timerText = null;
    }
}
