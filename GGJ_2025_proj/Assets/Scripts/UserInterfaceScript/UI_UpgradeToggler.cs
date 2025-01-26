using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class UI_UpgradeToggler : MonoBehaviour
{
    public int experience;

    public GameObject menu;
    private bool isMenuActive = false;
    public GameObject Popupa;

    public List<GameObject> Tutorial_BranchOut_Hide = new List<GameObject>();
    public GameObject button;

    private Dictionary<string, bool> earnedUpgrades = new Dictionary<string, bool>
    {
        { "Bubblegun", false },
        { "DBpistol", false },
        { "Shotgun", false },
        { "Chain", false },
        { "LMG", false },
        { "SMG", false },
        { "Minigun", false },
        { "MGL", false },
        { "MLRS", false },
        { "GL", false },
        { "RPG", false }
    };

    private Dictionary<string, bool> earnedUpgrades_UI = new Dictionary<string, bool>
    {
        { "BUBBLEGUN_BUTTON", false },
        { "DBPISTOL_BUTTON", false },
        { "SHOTGUN_BUTTON", false },
        { "CHAIN_BUTTON", false },
        { "LMG_BUTTON", false },
        { "SMG_BUTTON", false },
        { "MINIGUN_BUTTON", false },
        { "MGL_BUTTON", false },
        { "MLRS_BUTTON", false },
        { "GL_BUTTON", false },
        { "RPG_BUTTON", false }
    };

    private List<string> Etrack_list = new List<string> { "GL","MGL", "RPG", "MLRS" };
    private List<string> Btrack_list = new List<string> { "SMG", "LMG", "Minigun" };
    private List<string> Atrack_list = new List<string> { "DBpistol","Shotgun", "Chain" };
    private string btname;

    private (string, bool)[] ownedWeapons = new (string, bool)[0];

    public void buttonclicked()
    {
        PlayerController pc = GameManager.Instance.getPC();
        Debug.Log("clicked");
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            btname = EventSystem.current.currentSelectedGameObject.name;
            button = GameObject.Find(btname);
            if (button != null)
            {
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    Debug.Log($"Button text selected was : {buttonText.text}");
                    ApplyUpgrade(buttonText.text);
                    pc.Upgrade(btname);
                }
            }
            Debug.Log($"this button was clicked {btname}");
        }
        else
        {
            Debug.LogError("No button was clicked or EventSystem.current.currentSelectedGameObject is null.");
        }
        ToggleUPGRADEMenu();
    }

    public bool CanUpgradeSelected(string upgradenName)
    {   
        Debug.Log($"canupgradeselected returned; !earnedUpgrades[upgradenName]");
        return !earnedUpgrades[upgradenName];
    }

    public void ApplyUpgrade(string upgradename)
    {
        Debug.Log($"applying this upgrade {upgradename}");
        if (CanUpgradeSelected(upgradename))
        {
            earnedUpgrades[upgradename] = true;
            AddWeapon(upgradename);

            // if (upgradename == "DBPistol")
            // {
            //     Debug.Log("DPPISTOL WAS SELECTED, SHOWING REST OF TREE");
            //     foreach (var element in Tutorial_BranchOut_Hide)
            //     {
            //         element.SetActive(true);
            //     }
            // }

            // Change the button background color to RED to indicate it's been selected
            if (button != null)
            {
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    if (btname == "BUTTON_ETRACK")
                    {
                        if (Etrack_list != null && Etrack_list.Count > 0)
                        {
                            buttonText.text = Etrack_list[0];
                            Etrack_list.RemoveAt(0);
                        }
                        else
                        {
                            CantUpgradePOPUP(upgradename);
                        }
                    }
                    else if (btname == "BUTTON_BTRACK")
                    {
                        if (Btrack_list != null && Btrack_list.Count > 0)
                        {
                            buttonText.text = Btrack_list[0];
                            Btrack_list.RemoveAt(0);
                        }
                        else
                        {
                            CantUpgradePOPUP(upgradename);
                        }
                    }
                    else if (btname == "BUTTON_ATRACK")
                    {
                        if (Atrack_list != null && Atrack_list.Count > 0)
                        {
                            buttonText.text = Atrack_list[0];
                            Atrack_list.RemoveAt(0);
                        }
                        else
                        {
                            CantUpgradePOPUP(upgradename);
                        }
                    }
                }
            }
        }
        else
        {
            CantUpgradePOPUP(upgradename);
        }
    }

    void Start()
    {
        Popupa = GameObject.Find("POPUP");
        menu = GameObject.Find("UPGRADE_INTERFACE");
        if (menu != null)
        {
            menu.SetActive(false);
        }
        if (Popupa != null)
        {
            Popupa.SetActive(false);
        }
        GameObject buttonEtrack = GameObject.Find("BUTTON_ETRACK");
        GameObject buttonBtrack = GameObject.Find("BUTTON_BTRACK");
        if (buttonEtrack != null)
        {
            Tutorial_BranchOut_Hide.Add(buttonEtrack);
        }
        if (buttonBtrack != null)
        {
            Tutorial_BranchOut_Hide.Add(buttonBtrack);
        }
        foreach (var element in Tutorial_BranchOut_Hide)
        {
            element.SetActive(false);
        }
    }

    public void ToggleUPGRADEMenu()
    {
        isMenuActive = !isMenuActive;
        if (menu != null)
        {
            menu.SetActive(isMenuActive);
        }

        GameManager gm = GameManager.Instance;
        if(gm != null)
        {
            if(gm.CurrentGameState != GameManager.GameState.Playing)
            {
                gm.UnPause();
            }
        }

        // Add and hide Tutorial_BranchOut_Hide elements if not there
        // GameObject buttonEtrack = GameObject.Find("BUTTON_ETRACK");
        // GameObject buttonBtrack = GameObject.Find("BUTTON_BTRACK");
        // if (buttonEtrack != null && !Tutorial_BranchOut_Hide.Contains(buttonEtrack))
        // {
        //     Tutorial_BranchOut_Hide.Add(buttonEtrack);
        // }
        // if (buttonBtrack != null && !Tutorial_BranchOut_Hide.Contains(buttonBtrack))
        // {
        //     Tutorial_BranchOut_Hide.Add(buttonBtrack);
        // }
        // foreach (var element in Tutorial_BranchOut_Hide)
        // {
        //     element.SetActive(false);
        // }

        // // if the Atrack button has a TMP_Text that says DBPistol, hide the rest of the buttons
        // GameObject atrackButton = GameObject.Find("BUTTON_ATRACK");
        // if (atrackButton != null)
        // {
        //     TMP_Text buttonText = atrackButton.GetComponentInChildren<TMP_Text>();
        //     if (buttonText != null && buttonText.text == "DBPistol")
        //     {
        //         foreach (var element in Tutorial_BranchOut_Hide)
        //         {
        //             element.SetActive(false);
        //         }
        //     }
        //     else
        //     {
        //         foreach (var element in Tutorial_BranchOut_Hide)
        //         {
        //             element.SetActive(true);
        //         }
        //     }
        // }
    }

    public void CantUpgradePOPUP(string upgradeName)
    {
        Debug.Log($"Cannot upgrade: {upgradeName}");
        if (Popupa != null && !Popupa.activeInHierarchy)
        {
            Popupa.SetActive(true);
            StartCoroutine(HidePopupAfterDelay(2f));
        }
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Popupa != null)
        {
            Popupa.SetActive(false);
        }
    }

    public bool HasWeapon(string weaponName)
    {
        foreach (var weapon in ownedWeapons)
        {
            if (weapon.Item1 == weaponName)
            {
                return true;
            }
        }
        return false;
    }

    public void AddWeapon(string weaponName)
    {
        if (!HasWeapon(weaponName))
        {
            Array.Resize(ref ownedWeapons, ownedWeapons.Length + 1);
            ownedWeapons[ownedWeapons.Length - 1] = (weaponName, true);
        }
    }
}