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

        foreach (LRule rule in TE.lSystem.rules)
        {
            bool selected = EditorGUILayout.Toggle(new GUIContent(rule.name), (selectedRule == rule));
            if (selected)
                selectedRule = rule;
        }

        if (selectedRule != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Matches symbol ", GUILayout.ExpandWidth(false));

            if (selectedRule.matchVal.tex != null)
                GUILayout.Button(selectedRule.matchVal.tex, SE.buttonOpts);
            else
                GUILayout.Button(selectedRule.matchVal.symbol.ToString(), SE.buttonOpts);

            GUILayout.Label(" and replaces with:");

            GUILayout.EndHorizontal();

            Rect vertR = EditorGUILayout.BeginVertical();

            for (int i = 0; i < 5; ++i)
            {
                Rect horiR = EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < 5; ++j)
                {
                    //use texture if has one, use symbol if not
                    if (selectedRule.replacementVals[i, j].tex != null)
                        GUILayout.Button(selectedRule.replacementVals[i, j].tex, SE.buttonOpts);
                    else
                        GUILayout.Button(selectedRule.replacementVals[i, j].symbol.ToString(), SE.buttonOpts);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        bool makeNewRule = GUILayout.Button("+ Add New Rule");
        if (makeNewRule) MakeNewRule();

        bool deleteCurrent = GUILayout.Button("- Delete Selected Rule");
        if (deleteCurrent) DeleteRule();
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