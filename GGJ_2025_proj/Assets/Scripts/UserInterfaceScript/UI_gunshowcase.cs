using UnityEngine;

public class UI_gunshowcase : MonoBehaviour
{
    public GameObject gunIconPrefab; // Assign the prefab in the inspector
    public GameObject ammoIconCanvas; // Assign the AMMOICON canvas in the inspector
    private GameObject current3DModel;
    private RectTransform gunIconAnimTransform;

    void Start()
    {
        // Find the GUNICON_ANIM object in the scene
        GameObject gunIconAnim = GameObject.Find("GUNICON_ANIM");
        if (gunIconAnim != null)
        {
            gunIconAnimTransform = gunIconAnim.GetComponent<RectTransform>();

            // Instantiate the prefab and set it as a child of GUNICON_ANIM
        }
        else
        {
            Debug.LogError("GUNICON_ANIM object not found in the scene.");
        }
    }

    void FixedUpdate()
    {
        if (current3DModel != null)
        {
            // Rotate the 3D model slowly
            current3DModel.transform.Rotate(Vector3.up, 20 * Time.deltaTime);
        }
    }

    public void Set3DModel(GameObject newModel)
    {
        // Destroy the current model if it exists
        if (current3DModel != null)
        {
            Destroy(current3DModel);
        }

        // Instantiate the new model and set it as a child of the AMMOICON canvas
        current3DModel = Instantiate(newModel, ammoIconCanvas.transform);

        // Adjust the scale and position to fit within the bounding box of the canvas
        RectTransform canvasRect = ammoIconCanvas.GetComponent<RectTransform>();
        float scaleFactor = Mathf.Min(canvasRect.rect.width, canvasRect.rect.height) / 2;
        current3DModel.transform.localScale = Vector3.one * scaleFactor;
        current3DModel.transform.localPosition = Vector3.zero;
    }
}