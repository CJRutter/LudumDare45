using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WorldCell : Tile
{

#if UNITY_EDITOR
    [MenuItem("Assets/Create/WorldCell")]
    public static void CreateWorldCell()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save World Cell", "New World Cell", "Asset", "Save World Cell", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WorldCell>(), path);
    }
#endif

    #region Properties
    public bool Solid { get; set; }
    public float LightBlock { get; set; }
    #endregion Properties        

    #region Fields
    #endregion Fields
}
