//C# Example
using UnityEditor;
using UnityEngine;

using SE = LSymbolEditor;

public class LTerrainEditor : EditorWindow
{
    public static LSystem lSystem; //currently selected LSystem, shared by LSymbolEditor and LRuleEditor
    Vector2 scrollPos;
    int selectedLoD;
    public static int brushIndex = 0;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/L Terrain Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow<LTerrainEditor>();
        EditorWindow.GetWindow<LRuleEditor>(typeof(LTerrainEditor));
        EditorWindow.GetWindow<LSymbolEditor>(typeof(LTerrainEditor));
        EditorWindow.GetWindow<LPatchEditor>(typeof(LTerrainEditor));
    }

    void OnGUI()
    {
        if (lSystem == null) return;

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        string[] options = new string[lSystem.definedSymbols.Count];
        for (int i = 0; i < lSystem.definedSymbols.Count; ++i)
        {
            options[i] = lSystem.definedSymbols[i].name;
        }

        brushIndex = EditorGUILayout.Popup(brushIndex, options);

        for (int i = 0; i < lSystem.systemString.Length; ++i)
        {
            bool selected = EditorGUILayout.Toggle(new GUIContent("LoD " + i), (selectedLoD == i));
            if (selected)
                selectedLoD = i;
        }

        bool genNew = GUILayout.Button("+ Gen New LoD From Selected", GUILayout.ExpandWidth(false));
        if (genNew) GenFromSelectedLoD();

        EditorGUILayout.EndVertical();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        Rect vertR = EditorGUILayout.BeginVertical();

        for (int i = 0; i < lSystem.systemString[selectedLoD].GetLength(0); ++i)
        {
            Rect horiR = EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < lSystem.systemString[selectedLoD].GetLength(1); ++j)
            {
                if (lSystem.systemString[selectedLoD][i,j].tex != null)
                    GUILayout.Button(lSystem.systemString[selectedLoD][i, j].tex, SE.buttonOpts);
                else
                    GUILayout.Button(lSystem.systemString[selectedLoD][i, j].symbol.ToString(), SE.buttonOpts);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    void GenFromSelectedLoD()
    {
        LSymbol[][,] temp = lSystem.systemString;
        lSystem.systemString = new LSymbol[selectedLoD + 2][,];
        //copy previous LoDs over to new array
        for (int i = 0; i < selectedLoD+1; ++i)
        {
            lSystem.systemString[i] = temp[i];
        }
        //generate the new LoD
        lSystem.systemString[selectedLoD+1] = lSystem.IterateLString(lSystem.systemString[selectedLoD]);
    }

    void OnSelectionChange()
    {
        LSystem newSelectedLSystem = Selection.gameObjects[0].GetComponent<LSystem>();
        if (newSelectedLSystem != null)
            lSystem = newSelectedLSystem;
    }
}