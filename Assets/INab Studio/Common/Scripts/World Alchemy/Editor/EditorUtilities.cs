using UnityEditor;
using UnityEngine;

public static class EditorUtilities
{
    public static string WorldDissolveLink = "https://u3d.as/38rE";
    public static string WorldDissolveDocs = "https://inabstudios.gitbook.io/world-dissolve/";

    public static string WorldDissolveParticlesLink = "Soon...";

    public static void DrawLinks(string assetLink, string docsLink)
    {
        EditorGUILayout.LabelField("Links", EditorStyles.boldLabel);

        using (var verticalScope = new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            if (GUILayout.Button("Get Help (Discord)"))
            {
                Application.OpenURL("https://discord.gg/rrTsHkB9gS");
            }

            if (GUILayout.Button("Documentation"))
            {
                Application.OpenURL(docsLink);
            }

            if (GUILayout.Button("Leave A Review"))
            {
                Application.OpenURL(assetLink);
            }
        }

        EditorGUILayout.Space();

    }
}
