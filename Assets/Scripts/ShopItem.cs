using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{

    [SerializeField] private GameManager gm;
    [SerializeField] private string name;
    [SerializeField] private int price;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject moduleButton;
    private bool purchased;
    

    // Start is called before the first frame update
    void Start()
    {
        text.text = name + "\n$" + price;
    }

    // Update is called once per frame
    void Update()
    {
        if (!purchased && gm.money < price) {
            text.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else {
            text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    public void Purchase() {
        if (!purchased && gm.money >= price) {
            purchased = true;
            gm.money -= price;
            text.text = name + "\n---";
            moduleButton.SetActive(true);
        }
    }
}
