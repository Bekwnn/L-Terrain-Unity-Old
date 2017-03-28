//C# Example
using UnityEditor;
using UnityEngine;

using TE = LTerrainEditor;

public class LSymbolEditor : EditorWindow
{
    public static GUILayoutOption[] buttonOpts = new GUILayoutOption[2] { GUILayout.Width(40), GUILayout.Height(40) };
    LSymbol selectedSymbol;

    void OnGUI()
    {
        if (TE.lSystem == null) return;

        foreach (LSymbol symbol in TE.lSystem.definedSymbols)
        {
            bool selected = EditorGUILayout.Toggle(new GUIContent(symbol.name), (selectedSymbol == symbol));
            if (selected)
                selectedSymbol = symbol;
        }

        Rect vertR = EditorGUILayout.BeginVertical();

        if (selectedSymbol != null)
        {
            selectedSymbol.name = EditorGUILayout.TextField("Symbol Name: ", selectedSymbol.name);
            string temp = EditorGUILayout.TextField("Symbol Char: ", selectedSymbol.symbol.ToString());
            selectedSymbol.symbol = (temp.Length > 0)? temp[0] : '?';
            selectedSymbol.tex = EditorGUILayout.ObjectField("Symbol Texture: ", selectedSymbol.tex, typeof(Texture), false) as Texture;
        }

        EditorGUILayout.EndVertical();

        bool defineNewSymbol = GUILayout.Button("+ Add New Symbol");
        if (defineNewSymbol) DefineNewSymbol();

        bool deleteCurrent = GUILayout.Button("- Delete Selected Symbol");
        if (deleteCurrent) DeleteSymbol();
    }

    void DefineNewSymbol()
    {
        LSymbol newSymbol = new LSymbol('?', "Symbol (" + TE.lSystem.definedSymbols.Count + ")");
        TE.lSystem.definedSymbols.Add(newSymbol);
    }

    void DeleteSymbol()
    {
        TE.lSystem.definedSymbols.Remove(selectedSymbol);
        selectedSymbol = null;
    }

        void OnSelectionChange()
    {
        LSystem newSelectedLSystem = Selection.gameObjects[0].GetComponent<LSystem>();
        if (newSelectedLSystem != null)
            TE.lSystem = newSelectedLSystem;
    }
}