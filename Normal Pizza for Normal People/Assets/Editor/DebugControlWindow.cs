
using ACTools.Saving;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugControlWindow : EditorWindow
{
    private bool shouldLoadNewLevel = false;
    private int levelToLoad = 0;

    private bool shouldResetStars = false;
    private Vector2 rangeOfLevelsToReset;

    private bool shouldSetFurthestLevel = false;
    private int levelToSetFurthestLevel;
    
    [MenuItem("Window/Debug/Debug Control Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(DebugControlWindow));
    }
    void OnGUI()
    {
        GUILayout.Label ("Test Level", EditorStyles.boldLabel);
        levelToLoad = EditorGUILayout.IntField("Selected Level", levelToLoad);
        
        if (GUILayout.Button("Load Test Level"))
        {
            shouldLoadNewLevel = true;
        }
        
        GUILayout.Label ("Manage Save Data", EditorStyles.boldLabel);
        
        levelToSetFurthestLevel = EditorGUILayout.IntField("Set Furthest Level", levelToSetFurthestLevel);
        
        if (GUILayout.Button("Set Furthest Level"))
        {
            shouldSetFurthestLevel = true;
        }
        
        rangeOfLevelsToReset = EditorGUILayout.Vector2Field("Level Range", rangeOfLevelsToReset);
        
        if (GUILayout.Button("Reset Saved Level Stars Data"))
        {
            shouldResetStars = true;
        }
    }
    
    void Update()
    {
        if (shouldLoadNewLevel)
        {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused)
            {
                LoadNewLevel();
                Repaint();
            }
        }

        if (shouldSetFurthestLevel)
        {
            if (!EditorApplication.isPlaying)
            {
                SetFurthestLevel();
                Repaint();
            }
        }

        if (shouldResetStars)
        {
            if (!EditorApplication.isPlaying)
            {
                ResetStarData();
                Repaint();
            }
        }
    }

    void LoadNewLevel()
    {
        shouldLoadNewLevel = false;
        LevelSelect.selectedLevel = levelToLoad;
        Debug.LogWarning($"Loading Level: {levelToLoad}");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SetFurthestLevel()
    {
        shouldSetFurthestLevel = false;
        Debug.LogWarning($"Setting Furthest Level to Level: {levelToSetFurthestLevel}");
        SaveData.ToBinaryFile("npnp", "FurthestLevel", levelToSetFurthestLevel);
    }

    void ResetStarData()
    {
        shouldResetStars = false;
        for(int i = (int)rangeOfLevelsToReset.x; i <= rangeOfLevelsToReset.y; i++)
        {
            Debug.LogWarning($"Reset Stars on Level: {i}");
            SaveData.ToBinaryFile("npnp", $"day_{i}_stars", 0);
        }
    }
}
