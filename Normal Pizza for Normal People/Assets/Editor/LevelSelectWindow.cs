
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectWindow : EditorWindow
{
    private bool shouldLoadNewLevel = false;
    private int levelToLoad = 0;
    
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Level Select")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(LevelSelectWindow));
    }
    void OnGUI()
    {
        GUILayout.Label ("Test Level", EditorStyles.boldLabel);
        levelToLoad = EditorGUILayout.IntField("Selected Level", levelToLoad);
        
        if (GUILayout.Button("Load Test Level"))
        {
            shouldLoadNewLevel = true;
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
    }

    void LoadNewLevel()
    {
        shouldLoadNewLevel = false;
        LevelSelect.selectedLevel = levelToLoad;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
