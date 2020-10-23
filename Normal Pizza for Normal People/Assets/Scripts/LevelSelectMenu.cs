using System.Collections;
using System.Collections.Generic;
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

                // set all levels that are beyond the furthest level to locked state
                if (i > PlayerPrefs.GetInt("FurthestLevel", 0))
                {
                    levelButtonScript.GetComponent<ButtonTransistioner>().SetInteractable(false);
                }
            }
        }
    }
}
