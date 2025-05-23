using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
public class UI_UpgradeToggler : MonoBehaviour
{
    public int experience;

    public GameObject menu;
    private bool isMenuActive = false;
    public GameObject Popupa;

    public List<GameObject> Tutorial_BranchOut_Hide = new List<GameObject>();

    // this was the old way of storing if we can or cant upgrade something
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
    private DoublyLinkedList<string> Etrack_list = new DoublyLinkedList<string>(new List<string> { "GL", "MGL", "RPG", "MLRS" });
    private DoublyLinkedList<string> Btrack_list = new DoublyLinkedList<string>(new List<string> { "SMG", "LMG", "Minigun" });
    private DoublyLinkedList<string> Atrack_list = new DoublyLinkedList<string>(new List<string> { "DBpistol", "Shotgun", "Chain" });
    private DoublyLinkedListNode<string> Etrack_head;
    private DoublyLinkedListNode<string> Btrack_head;
    private DoublyLinkedListNode<string> Atrack_head;
    //private List<string> Etrack_list = new List<string> { "GL","MGL", "RPG", "MLRS" };
    //private List<string> Btrack_list = new List<string> { "SMG", "LMG", "Minigun" };
    //private List<string> Atrack_list = new List<string> { "DBpistol","Shotgun", "Chain" };
    private string btname;
    private (string, bool)[] ownedWeapons = new (string, bool)[0];
    private Stack<(string, string)> recentUpgrades = new Stack<(string, string)>();

    void Start()
    {
        Popupa = GameObject.Find("POPUP");
        menu = GameObject.Find("UPGRADE_INTERFACE");
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
        if (menu != null)
        {
            menu.SetActive(false);
        }
        if (Popupa != null)
        {
            Popupa.SetActive(false);
        }
        foreach (var element in Tutorial_BranchOut_Hide)
        {
            element.SetActive(false);
        }
        // declare heads
        Etrack_head = Etrack_list.Head;
        Btrack_head = Btrack_list.Head;
        Atrack_head = Atrack_list.Head;
    }
    public void buttonclicked()
    {
        PlayerController pc = GameManager.Instance.getPC();
        Debug.Log("clicked");
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            btname = EventSystem.current.currentSelectedGameObject.name;
            GameObject button = GameObject.Find(btname);
            if (button != null)
            {
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    Debug.Log($"Button text selected was : {buttonText.text}");
                    //if (buttonText.text == "DBpistol")
                    pc.Upgrade(btname);
                    foreach (var element in Tutorial_BranchOut_Hide)
                    {
                        element.SetActive(true);
                        
                    }
                    ApplyUpgrade(buttonText.text, button);             
                }
                else
                {
                    Debug.LogError("BUTTON CLICK WAS NULL!");
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
        // Here we must add the determinante for experience/checking if we can or cant apply the upgrade
        // if we cant click out/popup shows

        //
        Debug.Log($"canupgradeselected returned; !earnedUpgrades[upgradenName]");
        return true;
        // return earnedUpgrades.ContainsKey(upgradenName) && !earnedUpgrades[upgradenName];
    }

    public void ApplyUpgrade(string upgradename, GameObject button)
    {
        Debug.Log($"applying this upgrade {upgradename}");
        if (CanUpgradeSelected(upgradename)) // here we check if can or cant upgrade
        // if we cant do popup
        {
            earnedUpgrades[upgradename] = true;
            AddWeapon(upgradename);

            if (button != null)
            {
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    // here we get the head, change to next, set the text value to the head value
                    
                    if (btname == "BUTTON_ETRACK" && Etrack_head.Next != null)
                    {
                        Etrack_head = Etrack_head.Next;
                        buttonText.text = Etrack_head.Value;
                    }
                    else if (btname == "BUTTON_BTRACK" && Btrack_head.Next != null)
                    {
                        Btrack_head = Btrack_head.Next;
                        buttonText.text = Btrack_head.Value;
                    }
                    else if (btname == "BUTTON_ATRACK" && Atrack_head.Next != null)
                    {
                        Atrack_head = Atrack_head.Next;
                        buttonText.text = Atrack_head.Value;
                    }
                    else
                    {
                        CantUpgradePOPUP(upgradename);
                    }
                    // we push to recently upgraded
                    recentUpgrades.Push((upgradename, btname));
                    // tuple, name of (upgrade, button name); 
                    if (recentUpgrades.Count > 3) // we only store the 3 most recent
                    {
                        recentUpgrades = new Stack<(string, string)>(recentUpgrades.ToList().Take(3).Reverse());
                    }
                    // print out the recentUpgrades stack:
                    Debug.Log("Recent Upgrades Stack:");
                    foreach (var upgrade in recentUpgrades)
                    {
                        Debug.Log($"Upgrade: {upgrade.Item1}, Button: {upgrade.Item2}");
                    }
                }
            }
        }
        else
        {
            CantUpgradePOPUP(upgradename);
        }
    }

    public void RevertLastUpgrade()
    {
        if (recentUpgrades.Count > 0)
        {
            var (upgradename, buttonName) = recentUpgrades.Pop();
            earnedUpgrades[upgradename] = false;
            // Randomly pick a track to downgrade
            System.Random random = new System.Random();
            int track = random.Next(3); // 0 for A, 1 for B, 2 for E
            if (track == 0 && Atrack_head.Previous != null)
            {
                Atrack_head = Atrack_head.Previous;
                buttonName = "BUTTON_ATRACK";
            }
            else if (track == 1 && Btrack_head.Previous != null)
            {
                Btrack_head = Btrack_head.Previous;
                buttonName = "BUTTON_BTRACK";
            }
            else if (track == 2 && Etrack_head.Previous != null)
            {
                Etrack_head = Etrack_head.Previous;
                buttonName = "BUTTON_ETRACK";
            }
            GameObject button = GameObject.Find(buttonName);
            if (button != null)
            {
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    if (buttonName == "BUTTON_ETRACK")
                    {
                        buttonText.text = Etrack_head.Value;
                    }
                    else if (buttonName == "BUTTON_BTRACK")
                    {
                        buttonText.text = Btrack_head.Value;
                    }
                    else if (buttonName == "BUTTON_ATRACK")
                    {
                        buttonText.text = Atrack_head.Value;
                    }
                }
            }
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
        // GameObject atrackButton = GameObject.Find("BUTTON_ATRACK");
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


    // needs rework, adds name of gun, boolean tuple as array
    // to determine if it has gun (same as dict at start tbh, swap with weapon logic here)
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
            // Set all other weapons to false
            for (int i = 0; i < ownedWeapons.Length; i++)
            {
                ownedWeapons[i] = (ownedWeapons[i].Item1, false);
            }
            // Add the new weapon and set it to true
            Array.Resize(ref ownedWeapons, ownedWeapons.Length + 1);
            ownedWeapons[ownedWeapons.Length - 1] = (weaponName, true);
        }
    }
}

// Doubly linked list implementation (copilot cooked here)
public class DoublyLinkedListNode<T>
{
    public T Value { get; set; }
    public DoublyLinkedListNode<T> Next { get; set; }
    public DoublyLinkedListNode<T> Previous { get; set; }

    public DoublyLinkedListNode(T value)
    {
        Value = value;
    }
}

public class DoublyLinkedList<T>
{
    public DoublyLinkedListNode<T> Head { get; private set; }
    public DoublyLinkedListNode<T> Tail { get; private set; }

    public DoublyLinkedList(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            AddLast(item);
        }
    }

    public void AddLast(T value)
    {
        var newNode = new DoublyLinkedListNode<T>(value);
        if (Head == null)
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            Tail.Next = newNode;
            newNode.Previous = Tail;
            Tail = newNode;
        }
    }
}