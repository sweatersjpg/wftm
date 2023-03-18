using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VendingSystem : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject vendingMachine;
    [SerializeField] private Canvas vendingUICanvas;

    [SerializeField] private TextMeshProUGUI rockCountText;
    [SerializeField] private TextMeshProUGUI woodCountText;
    [SerializeField] private TextMeshProUGUI upgradePricePickaxeText;
    [SerializeField] private TextMeshProUGUI upgradePriceAxeText;
    [SerializeField] private TextMeshProUGUI buyHealthPackText;

    [SerializeField] private float priceUpgradePickaxe;
    [SerializeField] private float priceUpgradeAxe;
    [SerializeField] private float priceBuyHealthPack;

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
    }

    public void UpgradeAxe()
    {
        // upgrade axe system here
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
