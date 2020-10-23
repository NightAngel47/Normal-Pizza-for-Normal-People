using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonScript : MonoBehaviour
{
    [SerializeField] private int levelNum;
    private string levelName;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
