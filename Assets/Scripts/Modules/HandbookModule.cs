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

    private GameManager gm;

    int stage = 0;

    [SerializeField] private TMPro.TMP_Text pageOne, pageTwo;

    // Start is called before the first frame update
    void Start()
    {
        pageOne.text = overviewPage;
        pageTwo.text = userPages[stage - 1];
        gm = GameManager.Instance;

        pageOne.enabled = true;
        pageTwo.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (stage != gm.stage)
        {
            stage = gm.stage;
            pageTwo.text = userPages[stage - 1];
        }
            
    }

    public void SwapPage()
    {
        pageOne.enabled = !pageOne.enabled;
        pageTwo.enabled = !pageTwo.enabled;
    }
}
