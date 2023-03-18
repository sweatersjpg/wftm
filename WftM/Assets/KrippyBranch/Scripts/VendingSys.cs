using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VendingSys : MonoBehaviour
{
    PlayerController player;

    GameObject vendingMachineObj;
    [SerializeField] Canvas vendingUI;

    [SerializeField] TextMeshProUGUI rockCountText;
    [SerializeField] TextMeshProUGUI woodCountText;

    float localRock;
    float localWood;

    bool isPlayerInRange;

    private void Start()
    {
        vendingMachineObj = GameObject.FindGameObjectWithTag("Machine");
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (!vendingMachineObj && !player)
            return;

        if (Vector2.Distance(player.transform.position, vendingMachineObj.transform.position) <= 2)
        {
            isPlayerInRange = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                vendingUI.gameObject.SetActive(true);
            }
        }
        else
        {
            isPlayerInRange = false;
            vendingUI.gameObject.SetActive(false);
        }

        localRock = player.rockCount;
        localWood = player.woodCount;

        rockCountText.text = localRock.ToString();
        woodCountText.text = localWood.ToString();
    }

    public void HideUI()
    {
        vendingUI.gameObject.SetActive(false);
    }

    public void UpgradePickaxe()
    {
        // upgrade sys here
    }

    public void UpgradeAxe()
    {
        // upgrade sys here
    }

    public void BuyHealthPack(float price)
    {
        if (localWood >= price)
        {
            player.woodCount -= price;
            player.foodCount++;
        }
    }
}
