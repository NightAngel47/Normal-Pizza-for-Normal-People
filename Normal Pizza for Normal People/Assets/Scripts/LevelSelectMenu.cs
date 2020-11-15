using System.Collections;
using System.Collections.Generic;
using ACTools.Saving;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    public Sheet1 levelData;
    
    [SerializeField] private int numOfLevelsPerPage = 8;
    [SerializeField] private GameObject levelButton;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numOfLevelsPerPage; i++)
        {
            // only spawn level buttons for days that have data
            if (levelData.dataArray[i].Daynum > 0)
            {
                LevelButtonScript levelButtonScript = Instantiate(levelButton, transform).GetComponent<LevelButtonScript>();
                levelButtonScript.SetLevelNum(i);
                levelButtonScript.SetLevelName($"Day {levelData.dataArray[i].Daynum}");
                levelButtonScript.SetStarCount(LoadData.FromBinaryFile<int>("npnp", $"day_{levelData.dataArray[i].Daynum}_stars"));

                ButtonTransistioner buttonTransistioner = levelButtonScript.GetComponent<ButtonTransistioner>();
                buttonTransistioner.panelHandler = FindObjectOfType<MainMenuHandle>().gameObject;

                // set all levels that are beyond the furthest level to locked state
                if (i >  LoadData.FromBinaryFile<int>("npnp", "FurthestLevel"))
                {
                    buttonTransistioner.SetInteractable(false);
                }
            }
        }
    }
}
