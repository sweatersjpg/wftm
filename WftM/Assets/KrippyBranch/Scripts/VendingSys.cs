using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VendingSys : MonoBehaviour
{
    PlayerController playerController;
    GameObject vendingMachine;
    [SerializeField] private Canvas vendingUICanvas;

    [SerializeField] private TextMeshProUGUI rockCountText;
    [SerializeField] private TextMeshProUGUI woodCountText;
    [SerializeField] private TextMeshProUGUI upgradePricePickaxeText;
    [SerializeField] private TextMeshProUGUI upgradePriceAxeText;
    [SerializeField] private TextMeshProUGUI buyHealthPackText;

    [SerializeField] private float priceUpgradePickaxe;
    [SerializeField] private float priceUpgradeAxe;
    [SerializeField] private float priceBuyHealthPack;

    [Space]
    [SerializeField] float pickaxeUpgrade = 0.5f;
    [SerializeField] float axeUpgrade = 0.5f;

    private bool isPlayerInRange;

    private void Start()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        if (vendingMachine == null)
        {
            vendingMachine = GameObject.FindGameObjectWithTag("Machine");
        }

        if (vendingUICanvas != null)
        {
            vendingUICanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!vendingMachine || !playerController)
        {
            return;
        }

        float distance = Vector2.Distance(playerController.transform.position, vendingMachine.transform.position);

        if (distance <= 2)
        {
            isPlayerInRange = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                vendingUICanvas.gameObject.SetActive(true);
            }
        }
        else
        {
            isPlayerInRange = false;
            vendingUICanvas.gameObject.SetActive(false);
        }

        UpdateTextValues();
    }

    private void UpdateTextValues()
    {
        rockCountText.text = playerController.rockCount.ToString();
        woodCountText.text = playerController.woodCount.ToString();
        upgradePricePickaxeText.text = priceUpgradePickaxe.ToString();
        upgradePriceAxeText.text = priceUpgradeAxe.ToString();
        buyHealthPackText.text = priceBuyHealthPack.ToString();
    }

    public void HideUI()
    {
        if (vendingUICanvas != null)
        {
            vendingUICanvas.gameObject.SetActive(false);
        }
    }

    public void UpgradePickaxe()
    {
        // upgrade pickaxe system here

        if (playerController.rockCount >= priceUpgradePickaxe)
        {
            playerController.rockCount -= priceUpgradePickaxe;
            playerController.mineDamage += pickaxeUpgrade;

            priceUpgradePickaxe = Mathf.Min(20, priceUpgradePickaxe + 5);
        }
    }

    public void UpgradeAxe()
    {
        // upgrade axe system here

        if (playerController.rockCount >= priceUpgradeAxe)
        {
            playerController.rockCount -= priceUpgradeAxe;
            playerController.chopDamage += axeUpgrade;

            priceUpgradeAxe = Mathf.Min(20, priceUpgradeAxe + 5);
        }
    }

    public void BuyHealthPack()
    {
        if (playerController.woodCount >= priceBuyHealthPack)
        {
            playerController.woodCount -= priceBuyHealthPack;
            playerController.foodCount++;
        }
    }
}
