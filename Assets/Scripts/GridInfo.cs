using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{

    public static GridInfo instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool hasGrid;
    public List<InfoRow> theGrid;

    public void CreateGrid()
    {
        hasGrid = true;

        for(int y = 0; y < GridController.instance.blockRows.Count; y++)
        {
            theGrid.Add(new InfoRow());

            for(int x = 0; x < GridController.instance.blockRows[y].blocks.Count; x++)
            {
                theGrid[y].blocks.Add(new BlockInfo());
            }
        }
    }

    public void UpdateInfo(GrowBlock theBlock,int xPos, int yPos)
    {
        theGrid[yPos].blocks[xPos].currentStage = theBlock.currentStage;
        theGrid[yPos].blocks[xPos].isWatered = theBlock.isWatered;
    }

    public void GrowCrop()
    {
        for(int y = 0; y < theGrid.Count; y++)
        {
            for(int x = 0; x < theGrid[y].blocks.Count; x++)
            {
                if(theGrid[y].blocks[x].isWatered == true)
                {
                    switch (theGrid[y].blocks[x].currentStage)
                    {
                        case GrowBlock.GrowthStage.planted:

                        theGrid[y].blocks[x].currentStage = GrowBlock.GrowthStage.growing1;

                        break;

                        case GrowBlock.GrowthStage.growing1:

                        theGrid[y].blocks[x].currentStage = GrowBlock.GrowthStage.ripe;

                        break;


                    }
                    
                    theGrid[y].blocks[x].isWatered = false;
                }
            }
        }
    }
}

[System.Serializable]

public class BlockInfo
{
    public bool isWatered;
    public GrowBlock.GrowthStage currentStage;
}

[System.Serializable]
public class InfoRow
{
    public List<BlockInfo> blocks = new List<BlockInfo>();
}