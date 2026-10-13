using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 700f;
    private Rigidbody rb;
    public GameObject[] seedVisualPrefabs;

    public GameObject[] seedPrefabs; 
    [HideInInspector] public int unlockedSeedCount = 1; // Start with 1 unlocked seed

    private DirtTile currentTile;

    public static Player instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    [System.Obsolete]
    void Update()
    {
        // Basic movement
        Vector3 movement = Vector3.zero;

        if (Keyboard.current.aKey.isPressed)
            movement += Vector3.left * speed;

        if (Keyboard.current.dKey.isPressed)
            movement += Vector3.right * speed;

        if (Keyboard.current.wKey.isPressed)
            movement += Vector3.forward * speed;

        if (Keyboard.current.sKey.isPressed)
            movement += Vector3.back * speed;

        rb.MovePosition(transform.position + movement * Time.deltaTime);

        // Turning body
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Movement limits on x and z
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, 40f, 59f);
        position.z = Mathf.Clamp(position.z, 31f, 39f);
        transform.position = position;

        // Planting, Watering, Harvesting
        if (Keyboard.current.eKey.wasPressedThisFrame && currentTile != null)
        {
            // if (selectedSeed < unlockedSeedCount)
            // {
            //     currentTile.PlantSeed(seedPrefabs[selectedSeed], seedVisualPrefabs[selectedSeed], selectedSeed);
            // }
            FindObjectOfType<SeedSelector>().OpenSeedMenu(currentTile);
        }

        if (Keyboard.current.qKey.wasPressedThisFrame && currentTile != null)
        {
            currentTile.Water();
        }

        if (Keyboard.current.fKey.wasPressedThisFrame && currentTile != null && currentTile.IsHarvestable)
        {
            currentTile.Harvest();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DirtTile>(out DirtTile tile))
        {
            currentTile = tile;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<DirtTile>(out DirtTile tile) && tile == currentTile)
        {
            currentTile = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dirt"))
        {
            Debug.Log("Collided with: " + collision.gameObject.name);
        }
    }
}

