//C# Example
using UnityEditor;
using UnityEngine;

using TE = LTerrainEditor;
using SE = LSymbolEditor;

public class LRuleEditor : EditorWindow
{
    LRule selectedRule;

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

        foreach (LRule rule in TE.lSystem.rules)
        {
            bool selected = EditorGUILayout.Toggle(new GUIContent(rule.name), (selectedRule == rule));
            if (selected)
                selectedRule = rule;
        }

        bool makeNewRule = GUILayout.Button("+ Add New Rule");
        if (makeNewRule) MakeNewRule();

        bool deleteCurrent = GUILayout.Button("- Delete Selected Rule");
        if (deleteCurrent) DeleteRule();

        GUILayout.EndVertical();

        if (selectedRule != null)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Matches symbol ", GUILayout.ExpandWidth(false));

            GUIContent buttonContent;
            if (selectedRule.matchVal.tex != null) buttonContent = new GUIContent(selectedRule.matchVal.tex, selectedRule.matchVal.name);
            else buttonContent = new GUIContent(selectedRule.matchVal.symbol.ToString(), selectedRule.matchVal.name);
            bool tilePressed = GUILayout.Button(buttonContent, SE.buttonOpts);

            if (tilePressed)
            {
                selectedRule.matchVal = TE.lSystem.definedSymbols[TE.brushIndex];
            }

            GUILayout.Label(" and replaces with:");

            GUILayout.EndHorizontal();

            Rect vertR = EditorGUILayout.BeginVertical();

            for (int i = 0; i < 5; ++i)
            {
                Rect horiR = EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < 5; ++j)
                {
                    GUIContent buttonContent2;
                    if (selectedRule.replacementVals[i, j].tex != null) buttonContent2 = new GUIContent(selectedRule.replacementVals[i, j].tex, selectedRule.replacementVals[i,j].name);
                    else buttonContent2 = new GUIContent(selectedRule.replacementVals[i, j].symbol.ToString(), selectedRule.replacementVals[i, j].name);
                    bool tilePressed2 = GUILayout.Button(buttonContent2, SE.buttonOpts);

                    if (tilePressed2)
                    {
                        selectedRule.replacementVals[i, j] = TE.lSystem.definedSymbols[TE.brushIndex];
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }
    }

    void MakeNewRule()
    {
        LRule newRule = LRule.CreatePropegateRule(TE.lSystem.definedSymbols[0], TE.lSystem.definedSymbols[0]);
        newRule.name = "Rule (" + TE.lSystem.rules.Count + ")";
        TE.lSystem.rules.Add(newRule);
    }

    void DeleteRule()
    {
        TE.lSystem.rules.Remove(selectedRule);
        selectedRule = null;
    }

    void OnSelectionChange()
    {
        LSystem newSelectedLSystem = Selection.gameObjects[0].GetComponent<LSystem>();
        if (newSelectedLSystem != null)
            TE.lSystem = newSelectedLSystem;
    }
}