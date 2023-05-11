using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
public class WeaponConfigsEditor : EditorWindow
{
    [MenuItem("Tools/Generate WeaponConfigs")]
    public static void ShowWindow()
    {
        GetWindow<WeaponConfigsEditor>("WeaponConfigs Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate WeaponConfigs", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate"))
        {
            string weaponConfigsPath = "Assets/Scripts/Scriptable/WeaponConfigs.cs";
            string weaponConfigsScript = File.ReadAllText(weaponConfigsPath);
            string structPattern = @"public static WeaponConfig\s+(?<name>\w+)\s+=\s+new\s+WeaponConfig\s+\{[^}]*\b(id\s*=\s*(?<id>-?\d+))[^}]*\b(rating\s*=\s*(?<rating>\d+))";

            Dictionary<string, int> structIds = new Dictionary<string, int>();
            Regex regex = new Regex(structPattern, RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(weaponConfigsScript);
            Debug.Log("Matching");
            int[] counts = new int[] { 0, 0, 0, 0, 0 };

            foreach (Match match in matches)
            {
                string name = match.Groups["name"].Value;
                int id = int.Parse(match.Groups["id"].Value);
                int rating = int.Parse(match.Groups["rating"].Value);
                structIds[name] = id;

                switch (rating)
                {
                    case 1:
                        counts[0]++;
                        break;
                    case 2:
                        counts[1]++;
                        break;
                    case 3:
                        counts[2]++;
                        break;
                    case 4:
                        counts[3]++;
                        break;
                    case 5:
                        counts[4]++;
                        break;
                }
            }
            string[] lines = File.ReadAllLines(weaponConfigsPath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Trim().StartsWith("private const int WHITE_COUNT"))
                {
                    lines[i] = $"    private const int WHITE_COUNT = {counts[0]};";
                }
                else if (line.Trim().StartsWith("private const int GREEN_COUNT"))
                {
                    lines[i] = $"    private const int GREEN_COUNT = {counts[1]};";
                }
                else if (line.Trim().StartsWith("private const int BLUE_COUNT"))
                {
                    lines[i] = $"    private const int BLUE_COUNT = {counts[2]};";
                }
                else if (line.Trim().StartsWith("private const int PURPLE_COUNT"))
                {
                    lines[i] = $"    private const int PURPLE_COUNT = {counts[3]};";
                }
                else if (line.Trim().StartsWith("private const int ORANGE_COUNT"))
                {
                    lines[i] = $"    private const int ORANGE_COUNT = {counts[4]};";
                }
            }
            File.WriteAllLines(weaponConfigsPath, lines);
            AssetDatabase.Refresh();
            // Find the _getWeaponConfig function
            string getWeaponConfigPattern = @"public\s+WeaponConfig\s+_getWeaponConfig\s*\(\s*int\s+id\s*\)\s*{(?<body>.+?)\s*}";
            regex = new Regex(getWeaponConfigPattern, RegexOptions.Singleline);
            Match getWeaponConfigMatch = regex.Match(weaponConfigsScript);
            string getWeaponConfigBody = getWeaponConfigMatch.Groups["body"].Value;

            // Extract the existing cases from the switch statement
            List<int> existingIds = new List<int>();
            regex = new Regex(@"case\s+(?<id>-?\d+):\s*weaponData\s*=\s*WeaponConfigs\.(?<name>\w+);\s*break;", RegexOptions.Multiline);
            MatchCollection caseMatches = regex.Matches(getWeaponConfigBody);
            foreach (Match match in caseMatches)
            {
                existingIds.Add(int.Parse(match.Groups["id"].Value));
            }

            // Find the position of the last case statement in the switch body
            int lastCasePos = getWeaponConfigBody.LastIndexOf("case");

            // Append new cases to the switch body
            foreach (KeyValuePair<string, int> kvp in structIds)
            {
                int id = kvp.Value;
                if (!existingIds.Contains(id))
                {
                    string weaponName = kvp.Key;
                    string caseStatement = $"case {id}:\n\t\t\t\tweaponData = WeaponConfigs.{weaponName};\n\t\t\t\tbreak;\n\t\t\t";
                    getWeaponConfigBody = getWeaponConfigBody.Insert(lastCasePos, caseStatement);
                    existingIds.Add(id);
                }
            }

            // Replace the _getWeaponConfig function in the script file
            getWeaponConfigBody = "public WeaponConfig _getWeaponConfig(int id)\n\t{" + getWeaponConfigBody + "\t\n}\n";
            Debug.Log(getWeaponConfigBody);
            string saveWeaponConfigPattern = @"(?s)public\s+WeaponConfig\s+_getWeaponConfig\s*\(\s*int\s+id\s*\)\s*{.*?}";
            weaponConfigsScript = Regex.Replace(weaponConfigsScript, saveWeaponConfigPattern, getWeaponConfigBody);
            Debug.Log(weaponConfigsScript);
            // Write the modified script file to disk
            File.WriteAllText(weaponConfigsPath, weaponConfigsScript);
            AssetDatabase.Refresh();
        }
    }
}
#endif