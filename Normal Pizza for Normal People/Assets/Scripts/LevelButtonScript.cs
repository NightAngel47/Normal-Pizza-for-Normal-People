using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButtonScript : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNameText;
    private int levelNum;
    private string levelName;

    public int GetLevelNum()
    {
        return levelNum;
    }
    
    public void SetLevelNum(int num)
    {
        levelNum = num;
    }

    public string GetLevelName()
    {
        return levelName;
    }

    public void SetLevelName(string n)
    {
        levelName = n;
        levelNameText.text = n;
    }
}
