using UnityEngine;
using UnityEngine.UI;

public class UI_healthbar_dynamic : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;

    [Header("UI References")]
    public GameObject heartPrefab;
    public RectTransform heartContainer;

    [Header("Heart Configurations")]
    public Texture heartTexture;

    public bool game_end = false; // boolean for ending game/updating UI

    // for each time we take damage, update currentHealth variable, 
    // and call updatehearts()
    // handling the health logic/mechanics elsewhere is preferred, this is visual focused
    

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
        // else if(heartPrefab == null){
        heartPrefab = GameObject.Find("heart_sprite");
            // unity is stupid and wont use the assigned objects, so we find once
        // }

        // Position the heart container in the left center side of the screen
        heartContainer.anchoredPosition = new Vector2(50, Screen.height / 2 - heartContainer.rect.height / 2);

        // // Load the heart texture from the Assets/Models/Materials folder
        // heartTexture = Resources.Load<Texture>("Models/Materials/UI_health");
        // if (heartTexture == null)
        // {
        //     Debug.LogError("Heart texture not found in the Assets/Models/Materials folder.");
        // } doesnt work, need to do fancy streamingassets stuff to get , we are using prefabs instead 
    }

    void Start()
    {
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
            // heart.GetComponent<RectTransform>().localScale = Vector3.one; // Maintain the original size of the heart prefab
            // set the anchored type to top left
            heartRect.anchoredPosition = new Vector2(i * 50, 0);
            heartRect.anchorMin = new Vector2(0, 1);
            heartRect.anchorMax = new Vector2(0, 1);
            // heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 60, 0); // Adjust spacing as needed
            // heartRect.sizeDelta = new Vector2(heartRe/ct.sizeDelta.x * 4, heartRect.sizeDelta.y * 4);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        // Ensure health doesn't go below zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Remove hearts based on damage taken
        int heartsToRemove = maxHealth - currentHealth;
        for (int i = 0; i < heartsToRemove; i++)
        {
            if (heartContainer.childCount > 0)
            {
            heartContainer.GetChild(heartContainer.childCount - 1).gameObject.SetActive(false);
            // here we update the health variable (or not if its updated beforehand)
            }
        }
    }
}