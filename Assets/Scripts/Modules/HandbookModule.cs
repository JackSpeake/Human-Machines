using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandbookModule : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string[] userPages;

    [TextArea]
    [SerializeField] private string overviewPage;

    [TextArea]
    [SerializeField] private string rulesPage;

    private GameManager gm;

    int stage = 1;
    int page = 1;

    [SerializeField] private TMPro.TMP_Text pageOne, pageTwo, pageThree;

    // Start is called before the first frame update
    void Start()
    {
        pageOne.text = overviewPage;
        pageThree.text = rulesPage;
        pageTwo.text = userPages[stage - 1];
        gm = GameManager.Instance;
        stage = 0;

        pageOne.enabled = true;
        pageTwo.enabled = false;
        pageThree.enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        gm = GameManager.Instance;
        if (stage != gm.stage)
        {
            stage = gm.stage;
            pageTwo.text = userPages[stage - 1];
        }
            
    }

    public void SwapPage(int change)
    {
        page += change;

        if (page <= 0)
            page = 3;
        else if (page > 3)
            page = 1;

        switch (page)
        {
            case 1:
                pageOne.enabled = true;
                pageTwo.enabled = false;
                pageThree.enabled = false;
                break;
            case 2:
                pageOne.enabled = false;
                pageTwo.enabled = true;
                pageThree.enabled = false;
                break;
            case 3:
                pageOne.enabled = false;
                pageTwo.enabled = false;
                pageThree.enabled = true;
                break;
        }
           
    }
}