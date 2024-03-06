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
    private TMPro.TMP_Text moduleForSaleText;

    [SerializeField] 
    private SpriteRenderer moduleIconRenderer;

    [SerializeField] 
    private TMPro.TMP_Text discountText;

    [SerializeField] 
    private TMPro.TMP_Text claimText;

    [SerializeField] 
    private TMPro.TMP_Text offerEndsText;

    [SerializeField]
    private bool dealActive;

    [SerializeField]
    private int dealAppearanceRange;

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
        // Change this later to be randomized
        moduleForSale = "ANTIVIRUS";

        // Do the same for the module icon

        dealTime =  Random.Range(10, 120);
        dealPercentage = Random.Range(0, 100);

        moduleForSaleText.text = moduleForSale + " MODULE !!!";
        discountText.text = dealPercentage + "% OFF !!!";
        offerEndsText.text = "Offer Ends In:\n" + SecondsToMinutes((int)dealTime);


        claimButton.gameObject.SetActive(true);
        moduleIconRenderer.gameObject.SetActive(true);
        moduleForSaleText.enabled = true;
        discountText.enabled = true;
        claimText.enabled = true;
        offerEndsText.enabled = true;
        nothinText.enabled = false;

    }

    public void ActivateDeal() {
        // IMPLEMENT LOGIC SO THAT WHEN BUTTON CLICKED, SETS THE CURRENT DEAL TO THE CURRENT DEAL AFFECTING THE SHOP
        // IMPLEMENT LOGIC SO THAT THE CURRENT ACTIVE DEAL AFFECTS THE SHOP PRICE
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
