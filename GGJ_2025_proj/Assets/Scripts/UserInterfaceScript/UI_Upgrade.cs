using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Upgrade  : MonoBehaviour 
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsUnlocked { get; private set; }

    public Upgrade(string name, string description)
    {
        Name = name;
        Description = description;
        IsUnlocked = false;
    }

    public void Unlock()
    {
        IsUnlocked = true;
    }
}

public class UpgradeTree
{
    public List<Upgrade> Upgrades { get; private set; }

    public UpgradeTree()
    {
        Upgrades = new List<Upgrade>();
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        Upgrades.Add(upgrade);
    }

    public void UnlockUpgrade(string upgradeName)
    {
        Upgrade upgrade = Upgrades.Find(u => u.Name == upgradeName);
        if (upgrade != null)
        {
            upgrade.Unlock();
        }
    }
}

public class UI_Upgrade : MonoBehaviour
{
    public GameObject upgradeButtonPrefab;
    public Transform upgradeButtonContainer;
    private UpgradeTree upgradeTree;

    void Start()
    {
        upgradeTree = new UpgradeTree();
        InitializeUpgrades();
        CreateUpgradeButtons();
    }

    void InitializeUpgrades()
    {
        upgradeTree.AddUpgrade(new Upgrade("Speed Boost", "Increases your speed by 10%"));
        upgradeTree.AddUpgrade(new Upgrade("Health Boost", "Increases your health by 20%"));
        upgradeTree.AddUpgrade(new Upgrade("Damage Boost", "Increases your damage by 15%"));
    }

    void CreateUpgradeButtons()
    {
        foreach (Upgrade upgrade in upgradeTree.Upgrades)
        {
            GameObject button = Instantiate(upgradeButtonPrefab, upgradeButtonContainer);
            button.GetComponentInChildren<UnityEngine.UI.Text>().text = upgrade.Name;
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnUpgradeButtonClicked(upgrade.Name));
        }
    }

    void OnUpgradeButtonClicked(string upgradeName)
    {
        upgradeTree.UnlockUpgrade(upgradeName);
        Debug.Log(upgradeName + " unlocked!");
    }
}