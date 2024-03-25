using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameManager gm;
    [SerializeField] public string itemName;
    [SerializeField] public string itemDescription;
    [SerializeField] public Sprite icon;
    [SerializeField] private Sprite purchasedIcon;
    [SerializeField] public Image im;
    [SerializeField] public int originalPrice;
    [SerializeField] public int price;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject moduleButton;
    
    [SerializeField] private YapperState purchaseYapper;
    [SerializeField] private GameObject put;

    [SerializeField] private List<ShopItem> upgrades;
    [SerializeField] private bool isUpgrade;

    private Button purchaseButton;

    private bool purchased = false;
    private bool discounted = false;
    

    // Start is called before the first frame update
    void Start()
    {
        text.text = itemName + "\n$" + price;
        purchaseButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!purchased && gm.money < price) {
            text.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else if (!discounted || purchased) {
            text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    public void Purchase() {
        if (!purchased && gm.money >= price) {
            gm.money -= price;
            if (!isUpgrade) {
                moduleButton.SetActive(true);
            }

            if (upgrades.Count <= 0) {
                purchased = true;
                text.text = itemName + "\n---";
                GameManager.Instance.SendCustomYap(purchaseYapper, 1);
                text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                im.sprite = purchasedIcon;
            }
            else {
                itemName = upgrades[0].itemName;
                itemDescription = upgrades[0].itemDescription;
                originalPrice = upgrades[0].originalPrice;
                price =  upgrades[0].price;
                icon = upgrades[0].icon;
                isUpgrade = true;

                text.text = itemName + "\n$" + price;
                im.sprite = icon;

                upgrades.Remove(upgrades[0]);
                
            }
            
            
            

            
        }
    }

    public void DiscountPrice(int amount) {
        price = amount;
        text.text = itemName + "\n$" + price;
        text.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        discounted = true;
    }

    public void RevertPrice() {
        price = originalPrice;
        text.text = itemName + "\n$" + price;
        text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        discounted = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        put.SetActive(true);
        foreach (Transform child in put.GetComponent<Transform>())
        {
            child.gameObject.SetActive(true);
        }
        put.GetComponent<PopUpText>().SetPopUpText(itemDescription);
        put.GetComponent<PopUpText>().SetPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        put.SetActive(false);
    }
}
