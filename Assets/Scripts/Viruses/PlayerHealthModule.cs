using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This simply controls the module that visualizes system health
public class PlayerHealthModule : MonoBehaviour
{
    [SerializeField] private HealthModulePanel[] panelsUnsorted;
    private HealthModulePanel[,] panelsSorted;
    [SerializeField] private UnityEngine.UI.Extensions.Gradient2 l1, l2, l3;

    [SerializeField] private float shieldLevel = 0;
    [SerializeField] private float maxShieldLevel = 100;


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

        l1.Offset = -1;
        l2.Offset = -1;
        l3.Offset = -1;
    }

    private void Update()
    {
        if (shieldLevel > maxShieldLevel)
            shieldLevel = maxShieldLevel;

        if (shieldLevel > maxShieldLevel / 3 * 2)
        {
            l3.Offset = (shieldLevel % (maxShieldLevel / 3)) / (maxShieldLevel / 3) * 2 - 1;
            l2.Offset = 1;
            l1.Offset = 1;
        }
        else if (shieldLevel > maxShieldLevel / 3)
        {
            l3.Offset = -1;
            l2.Offset = (shieldLevel % (maxShieldLevel / 3)) / (maxShieldLevel / 3) * 2 - 1;
            l1.Offset = 1;
        }
        else
        {
            l3.Offset = -1;
            l2.Offset = -1;
            l1.Offset = (shieldLevel % (maxShieldLevel / 3)) / (maxShieldLevel / 3) * 2 - 1;
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
