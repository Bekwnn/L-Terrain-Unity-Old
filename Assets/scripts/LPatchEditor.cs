using UnityEditor;
using UnityEngine;

using TE = LTerrainEditor;
using SE = LSymbolEditor;

public class LPatchEditor : EditorWindow {

    LPatch selectedPatch;
    int curMatchIdx = -1;

    void OnGUI()
    {
        if (TE.lSystem == null) return;

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();

        string[] options = new string[TE.lSystem.definedSymbols.Count];
        for (int i = 0; i < TE.lSystem.definedSymbols.Count; ++i)
        {
            options[i] = TE.lSystem.definedSymbols[i].name;
        }

        TE.brushIndex = EditorGUILayout.Popup(TE.brushIndex, options);

        foreach (LPatch patch in TE.lSystem.definedPatches)
        {
            bool selected = EditorGUILayout.Toggle(new GUIContent(patch.name), (selectedPatch == patch));
            if (selected)
                selectedPatch = patch;
        }

        bool makeNew = GUILayout.Button("+ Add New Patch");
        if (makeNew) MakeNewPatch();

        bool deleteCurrent = GUILayout.Button("- Delete Selected Patch");
        if (deleteCurrent) DeletePatch();

        GUILayout.EndVertical();

        if (selectedPatch != null)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Matches symbol ", GUILayout.ExpandWidth(false));

            GUIContent buttonContent;
            if (selectedPatch.matchVal.tex != null) buttonContent = new GUIContent(selectedPatch.matchVal.tex, selectedPatch.matchVal.name);
            else buttonContent = new GUIContent(selectedPatch.matchVal.symbol.ToString(), selectedPatch.matchVal.name);
            bool tilePressed = GUILayout.Button(buttonContent, SE.buttonOpts);

            if (tilePressed)
            {
                selectedPatch.matchVal = TE.lSystem.definedSymbols[TE.brushIndex];
            }

            GUILayout.Label(" and replaces with patch contents:", GUILayout.ExpandWidth(true));

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();

            selectedPatch.name = EditorGUILayout.TextField("Patch Name: ", selectedPatch.name);
            selectedPatch.minHeight = EditorGUILayout.DelayedFloatField("Min Height: ", selectedPatch.minHeight);
            selectedPatch.maxHeight = EditorGUILayout.DelayedFloatField("Max Height: ", selectedPatch.maxHeight);
            selectedPatch.tex = EditorGUILayout.ObjectField("Terrain Texture: ", selectedPatch.tex, typeof(Texture), false) as Texture;
            selectedPatch.prefab = EditorGUILayout.ObjectField("Prefab Objects: ", selectedPatch.prefab, typeof(GameObject), false) as GameObject;

            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
    }

    void MakeNewPatch()
    {
        LPatch newPatch = new LPatch();
        newPatch.name = "Patch (" + TE.lSystem.definedPatches.Count + ")";
        newPatch.matchVal = TE.lSystem.definedSymbols[0];
        TE.lSystem.definedPatches.Add(newPatch);
    }

    void DeletePatch()
    {
        TE.lSystem.definedPatches.Remove(selectedPatch);
        selectedPatch = null;
    }

    void OnSelectionChange()
    {
        LSystem newSelectedLSystem = Selection.gameObjects[0].GetComponent<LSystem>();
        if (newSelectedLSystem != null)
            TE.lSystem = newSelectedLSystem;
    }
}
