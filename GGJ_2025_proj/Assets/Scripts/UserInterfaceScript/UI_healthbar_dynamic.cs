using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;
    public GameObject heartPrefab;
    public Transform heartsParent;
    public Camera mainCamera; // Reference to the main camera
    public Vector3 offset; // Offset from the camera

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    void FixedUpdate()
    {
        // For testing purposes, decrease health with space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth--;
            UpdateHearts();
        }
    }

    void Update()
    {
        // Update the position of the health bar to follow the camera
        if (mainCamera != null)
        {
            transform.position = mainCamera.transform.position + offset;
        }
    }

    void UpdateHearts()
    {
        // Clear existing hearts
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // Check if heartPrefab and heartsParent are assigned
        if (heartPrefab == null || heartsParent == null)
        {
            Debug.LogError("Heart Prefab or Hearts Parent is not assigned.");
            return;
        }

        // Create new hearts
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsParent);
            newHeart.transform.localPosition = new Vector3(i * 30, 0, 0); // Adjust spacing as needed
            if (i >= currentHealth)
            {
                newHeart.GetComponent<SpriteRenderer>().color = Color.gray; // Placeholder for empty heart
            }
            hearts.Add(newHeart);
        }
    }
}
