using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonScript : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private List<Image> starImages = new List<Image>();
    [SerializeField] private Color missingStarColor = Color.black;
    [SerializeField] private GameObject selectedLevelOutline;
    private int levelNum;
    private string levelName;
    private int levelStars;

    private void Start()
    {
        selectedLevelOutline.SetActive(false);
    }

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

    public int GetStarCount()
    {
        return levelStars;
    }

    public void SetStarCount(int count)
    {
        levelStars = count;
        DisplayStars();
    }

    /// <summary>
    /// Sets stars to black if they've not been achieved.
    /// </summary>
    private void DisplayStars()
    {
        for (int i = 0; i < starImages.Count; i++)
        {
            if (i > levelStars - 1)
            {
                starImages[i].color = missingStarColor;
            }
        }
    }

    public void ChangeSelectedLevel()
    {
        foreach (var levelButton in FindObjectsOfType<LevelButtonScript>())
        {
            if (levelButton.selectedLevelOutline.activeSelf)
            {
                levelButton.selectedLevelOutline.SetActive(false);
                break;
            }
        }
        selectedLevelOutline.SetActive(true);
    }
}
