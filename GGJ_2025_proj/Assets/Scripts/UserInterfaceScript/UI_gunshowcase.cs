// using UnityEngine;

// public class UI_gunshowcase : MonoBehaviour
// {
//     public GameObject gunIconPrefab; // Assign the prefab in the inspector
//     public GameObject ammoIconCanvas; // Assign the AMMOICON canvas in the inspector
//     private GameObject current3DModel;
//     private RectTransform gunIconAnimTransform;
//     [SerializedField]
//     public GameObject gun_label;
//     [SerializedField]
//     public GameObject gun_icon;
//     [SerializedField]
//     private GameManager GM;  // PLACEHOLDER, here we would put the script that handles weapons/weapon changes
//     void Start()
//     {
//         // Find the GUNICON_ANIM object in the scene
//         // GameObject gun_label = GameObject.Find("ammolabel");
//         // GameObject gun_icon = GameObject.Find("GUIB");
//         // if (gunIconAnim != null)
//         // {
//         //     // gunIconAnimTransform = gunIconAnim.GetComponent<RectTransform>();

            


//         // }
//         // else
//         // {
//         //     Debug.LogError("GUNICON_ANIM object not found in the scene.");
//         // }
//     }
//     void FixedUpdate()
//     {
//         if (current3DModel != null)
//         {
//             // Rotate the 3D model slowly
//             current3DModel.transform.Rotate(Vector3.up, 20 * Time.deltaTime);
//         }
//     }
//     public void Set3DModel(GameObject newModel)
//     {
//         // Destroy the current model if it exists
//         if (current3DModel != null)
//         {
//             Destroy(current3DModel);
//         }

//         // Instantiate the new model and set it as a child of the AMMOICON canvas
//         current3DModel = Instantiate(newModel, ammoIconCanvas.transform);

//         // Adjust the scale and position to fit within the bounding box of the canvas
//         RectTransform canvasRect = ammoIconCanvas.GetComponent<RectTransform>();
//         float scaleFactor = Mathf.Min(canvasRect.rect.width, canvasRect.rect.height) / 2;
//         current3DModel.transform.localScale = Vector3.one * scaleFactor;
//         current3DModel.transform.localPosition = Vector3.zero;
//     }
// }