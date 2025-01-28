using UnityEngine;
using UnityEngine.UI;

public class UI_healthbar_dynamic : MonoBehaviour
{
    [SerializeField]
    public GameObject playerobj;
    private PlayerHealth HealthManager;
    private int maxHealth;
    private int currentHealth;

    [Header("UI References")]
    [SerializeField]
    public GameObject heartPrefab;
    [SerializeField]
    public RectTransform heartContainer;

    public bool game_end = false; // boolean for ending game/updating UI

    void Awake()
    {
        // Ensure heartContainer is assigned
        if (heartContainer == null)
        {
            // Find the HeartContainer GameObject in the scene
            GameObject heartContainerObject = GameObject.Find("HEALTH");
            if (heartContainerObject == null)
            {
                Debug.LogError("HeartContainer GameObject not found in the scene.");
                return;
            }
            heartContainer = heartContainerObject.GetComponent<RectTransform>();
        }

        // Ensure heartPrefab is assigned
        if (heartPrefab == null)
        {
            heartPrefab = GameObject.Find("heart_sprite");
            if (heartPrefab == null)
            {
                Debug.LogError("HeartPrefab GameObject not found in the scene.");
                return;
            }
        }

        // Position the heart container in the left center side of the screen
        heartContainer.anchoredPosition = new Vector2(50, Screen.height / 2 - heartContainer.rect.height / 2);
    }

    void Start()
    {
        if (playerobj == null)
        {
            //Debug.LogError("Player object is not assigned.");
            return;
        }

        HealthManager = playerobj.GetComponent<PlayerHealth>();
        if (HealthManager == null)
        {
            Debug.LogError("PlayerHealth component not found on player object.");
            return;
        }

        maxHealth = (int)HealthManager.health;
        currentHealth = maxHealth;
        InitializeHearts();
    }

    void InitializeHearts()
    {
        // Delete any existing hearts
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }

        // Create heart UI elements
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            RectTransform heartRect = heart.GetComponent<RectTransform>();

            heart.transform.localScale = new Vector3(1.62f, 3.39208484f, 3.39208484f); // configured for near-1080p based on heartcontainer's deforms
            heartRect.anchoredPosition = new Vector2(i * 50, 0);
            heartRect.anchorMin = new Vector2(0, 1);
            heartRect.anchorMax = new Vector2(0, 1);

            // Get the RawImage component and tint red based on index
            RawImage heartImage = heart.GetComponent<RawImage>();
            if (heartImage != null)
            {
                float tintFactor = 1.0f - (i / (float)maxHealth);
                heartImage.color = new Color(1.0f, tintFactor, tintFactor);
            }
        }
    }

    void Update()
    {
        if (HealthManager != null)
        {
            int newHealth = (int)HealthManager.health;
            if (newHealth != currentHealth)
            {
                Debug.Log($"Health changed from {currentHealth} to {newHealth}");
                UpdateHearts(newHealth);
            }
        }
    }

    void UpdateHearts(int health)
    {
        // Ensure health doesn't go below zero
        currentHealth = Mathf.Clamp(health, 0, maxHealth);

        // Delete any existing hearts
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }

        // Create heart UI elements based on current health
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            RectTransform heartRect = heart.GetComponent<RectTransform>();

            heart.transform.localScale = new Vector3(1.62f, 3.39208484f, 3.39208484f); // configured for near-1080p based on heartcontainer's deforms
            heartRect.anchoredPosition = new Vector2(i * 50, 0);
            heartRect.anchorMin = new Vector2(0, 1);
            heartRect.anchorMax = new Vector2(0, 1);

            // Get the RawImage component and tint red based on index
            RawImage heartImage = heart.GetComponent<RawImage>();
            if (heartImage != null)
            {
                float tintFactor = 1.0f - (i / (float)maxHealth);
                heartImage.color = new Color(1.0f, tintFactor, tintFactor);
            }
        }
    }
}