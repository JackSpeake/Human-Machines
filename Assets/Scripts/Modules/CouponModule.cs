using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CouponModule : MonoBehaviour
{

    [SerializeField] 
    private TMPro.TMP_Text nothinText;

    [SerializeField]
    private Button claimButton;
    [SerializeField]
    private Text claimButtonText;

    [SerializeField] 
    private TMPro.TMP_Text moduleForSaleText;

    [SerializeField] 
    private Image moduleIconRenderer;

    [SerializeField] 
    private TMPro.TMP_Text discountText;

    [SerializeField] 
    private TMPro.TMP_Text claimText;

    [SerializeField] 
    private TMPro.TMP_Text offerEndsText;

    [SerializeField]
    private bool dealActive;
    [SerializeField]
    private bool dealClaimed = false;

    [SerializeField]
    private int dealAppearanceRange;

    [SerializeField]
    private ShopItem[] items;

    [SerializeField]
    private ShopItem dealTarget;
    [SerializeField]
    private int dealTargetIndex = -1;

    [SerializeField]
    private int activeTargetIndex = -1;
    private float dealTime;
    private int dealPercentage;
    private string moduleForSale;

    // Start is called before the first frame update
    void Start()
    {
        dealActive = false;
        NoDeals();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dealActive) {
            if (Random.Range(1, dealAppearanceRange) == 1) {
                CreateDeal();
            }
            dealActive = true;
        }
        else {
            offerEndsText.text = "Offer Ends In:\n" + SecondsToMinutes((int)dealTime);
            dealTime -= Time.deltaTime;
            if (dealTime <= 0) {
                NoDeals();
            } 
        }
    }

    void NoDeals() {
        claimButton.gameObject.SetActive(false);
        moduleIconRenderer.gameObject.SetActive(false);
        moduleForSaleText.enabled = false;
        discountText.enabled = false;
        claimText.enabled = false;
        offerEndsText.enabled = false;
        nothinText.enabled = true;

        dealActive = false;
    }

    void CreateDeal() {
        dealClaimed = false;
        dealTargetIndex = Random.Range(0, items.Length); 
        dealTarget = items[dealTargetIndex];
        // Change this later to be randomized
        moduleForSale = dealTarget.itemName.ToUpper();

        // Do the same for the module icon
        moduleIconRenderer.sprite = dealTarget.icon;

        dealTime =  Random.Range(10, 120);
        dealPercentage = Random.Range(0, 100);

        moduleForSaleText.text = moduleForSale + " MODULE !!!";
        discountText.text = dealPercentage + "% OFF !!!";
        offerEndsText.text = "Offer Ends In:\n" + SecondsToMinutes((int)dealTime);
        claimText.text = "CLAIM NOW !!!";
        claimButtonText.text = "$$$";
        claimButton.interactable = true;


        claimButton.gameObject.SetActive(true);
        moduleIconRenderer.gameObject.SetActive(true);
        moduleForSaleText.enabled = true;
        discountText.enabled = true;
        claimText.enabled = true;
        offerEndsText.enabled = true;
        nothinText.enabled = false;

    }

    public void ClaimDeal() {
        if (!dealClaimed) {
            if (activeTargetIndex != -1) {
                items[activeTargetIndex].RevertPrice();
            }
            // IMPLEMENT LOGIC SO THAT WHEN BUTTON CLICKED, SETS THE CURRENT DEAL TO THE CURRENT DEAL AFFECTING THE SHOP
            activeTargetIndex = dealTargetIndex;
            items[activeTargetIndex].DiscountPrice((int)(items[activeTargetIndex].originalPrice * (1.0 - ((float)dealPercentage / 100))));
            // IMPLEMENT LOGIC SO THAT THE CURRENT ACTIVE DEAL AFFECTS THE SHOP PRICE
            dealClaimed = true;
            claimButton.interactable = false;
            claimButtonText.text = "( $ _ $ )";
            claimText.text = "THXX !1!!1!11!";
        }
    }
        

    string SecondsToMinutes(int t) {
        if (t % 60 < 10) {
            return (t / 60) + ":0" + (t % 60);
        }
        else {
            return (t / 60) + ":" + (t % 60);
        }

        

    }
}
