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
    [SerializeField] private string itemDescription;
    [SerializeField] public Sprite icon;
    [SerializeField] public int originalPrice;
    [SerializeField] public int price;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject moduleButton;
    
    [SerializeField] private YapperState purchaseYapper;
    [SerializeField] private GameObject put;

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
            purchased = true;
            gm.money -= price;
            text.text = itemName + "\n---";
            moduleButton.SetActive(true);
            GameManager.Instance.SendCustomYap(purchaseYapper, 1);
            text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
        put.GetComponent<PopUpText>().SetPopUpText(itemDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        put.SetActive(false);
    }
}
