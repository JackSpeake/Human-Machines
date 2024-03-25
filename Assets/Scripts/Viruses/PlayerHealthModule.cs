using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This simply controls the module that visualizes system health
public class PlayerHealthModule : MonoBehaviour
{
    [SerializeField] private HealthModulePanel[] panelsUnsorted;
    private HealthModulePanel[,] panelsSorted;
    [SerializeField] private Image l1, l2, l3;

    [SerializeField] private float shieldLevel = 0;
    [SerializeField] private float maxShieldLevel = 100;

    public bool started = false;


    // Start is called before the first frame update
    void Start()
    {
        panelsSorted = new HealthModulePanel[5, 5];
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                panelsSorted[x, y] = panelsUnsorted[(y * 5) + x];
                panelsUnsorted[(y * 5) + x].infected = false;
                panelsUnsorted[(y * 5) + x].x = x;
                panelsUnsorted[(y * 5) + x].y = y;
            }
        }

        started = true;

        l1.enabled = false;
        l2.enabled = false;
        l3.enabled = false;
    }

    private void Update()
    {
        if (shieldLevel > maxShieldLevel)
            shieldLevel = maxShieldLevel;

        // Shield level 3
        if (shieldLevel >= maxShieldLevel / 3 * 2)
        {
            l1.enabled = true;
            l2.enabled = true;
            l3.enabled = true;

            l1.color = new Color
                (l1.color.r,
                l1.color.g,
                l1.color.b,
                1);

            l2.color = new Color
                (l2.color.r,
                l2.color.g,
                l2.color.b,
                1);

            l3.color = new Color
                (l3.color.r,
                l3.color.g,
                l3.color.b,
                shieldLevel / maxShieldLevel);
        }
        // Shield level 2
        else if (shieldLevel >= maxShieldLevel / 3)
        {
            l1.enabled = true;
            l2.enabled = true;
            l3.enabled = false;
            l1.color = new Color
                (l1.color.r,
                l1.color.g,
                l1.color.b,
                1);

            l2.color = new Color
                (l2.color.r,
                l2.color.g,
                l2.color.b,
                shieldLevel % (maxShieldLevel / 3) / (maxShieldLevel / 3));
        }
        // Shield level 1
        else if (shieldLevel > 0)
        {
            l1.enabled = true;
            l2.enabled = false;
            l3.enabled = false;
            
            l1.color = new Color
                (l1.color.r,
                l1.color.g,
                l1.color.b,
                shieldLevel % (maxShieldLevel / 3) / (maxShieldLevel / 3));
        }
        else
        {
            l1.enabled = false;
            l2.enabled = false;
            l3.enabled = false;
        }
    }

    public void SetShieldLevel(int sl)
    {
        shieldLevel = sl;
    }

    public HealthModulePanel GetPanelAtXY(int x, int y)
    {
        return panelsSorted[x, y];
    }

    public List<HealthModulePanel> GetNeighborsOfXY(int x, int y)
    {
        int currX, currY;
        List<HealthModulePanel> toReturn = new List<HealthModulePanel>();

        // Gets each of the possible neighbors manually cause fuck loops
        currX = x + 1; currY = y;
        if (InBounds(currX, currY))
            toReturn.Add(panelsSorted[currX, currY]);

        currX = x - 1; currY = y;
        if (InBounds(currX, currY))
            toReturn.Add(panelsSorted[currX, currY]);

        currX = x; currY = y + 1;
        if (InBounds(currX, currY))
            toReturn.Add(panelsSorted[currX, currY]);

        currX = x; currY = y - 1;
        if (InBounds(currX, currY))
            toReturn.Add(panelsSorted[currX, currY]);

        return toReturn;
    }

    public bool InBounds(int x, int y)
    {
        if (x >= 0 && x < 5 && y >= 0 && y < 5)
        {
            return true;
        }

        return false;
    }

    public HealthModulePanel GetCenterPanel()
    {
        return panelsSorted[2, 2];
    }

    public HealthModulePanel[] GetOutside()
    {
        HealthModulePanel[] toReturn = {panelsSorted[0, 0], panelsSorted[1, 0], panelsSorted[2, 0], panelsSorted[3, 0], panelsSorted[4, 0],
                            panelsSorted[4, 0], panelsSorted[4, 1], panelsSorted[4, 2], panelsSorted[4, 3], panelsSorted[4, 4],
                            panelsSorted[0, 0], panelsSorted[0, 0], panelsSorted[0, 0],
                            panelsSorted[0, 0], panelsSorted[0, 0], panelsSorted[0, 0]};

        return toReturn;
    }

    public HealthModulePanel[,] GetAllPanels()
    {
        return panelsSorted;
    }

    public List<HealthModulePanel> GetInfectedPanels()
    {
        List<HealthModulePanel> toReturn = new List<HealthModulePanel>();

        foreach (HealthModulePanel h in panelsUnsorted)
            if (h.infected)
                toReturn.Add(h);

        return toReturn;
    }
}
