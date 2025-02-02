using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_ammo : MonoBehaviour
{
    [SerializeField]
    public GameObject mg_obj;
    // [SerializeField]
    private GameManager GM; // attach GM script
    private Text ammoText;
    [SerializeField]
    public GameObject AmmoOBJ; // attach AMMOCOUNT object under UI

    private int ammocount_local;
    private TMP_Text ammoTMP;

    void Start()
    {
        GM = mg_obj.GetComponent<GameManager>();
        // Get the TMP_Text component from AmmoOBJ
        ammoTMP = AmmoOBJ.GetComponent<TMP_Text>();
        // ammocount_local = GM.BubbleResource;
        // Initialize ammo display
        UpdateAmmoDisplay(ammocount_local);
    }

    void Update()
    {
        // ammocount_local = GM.BubbleResource; // get resource
        UpdateAmmoDisplay(ammocount_local);
    }

    public void UpdateAmmoDisplay(int ammoCount)
    {
        if (ammoCount < 0)
        {
            ammoCount = 0;
        }

        if (ammoTMP != null)
        {
            ammoTMP.text = ammoCount.ToString();
        }
        else
        {
            ammoText.text = ammoCount.ToString();
        }
    }
}