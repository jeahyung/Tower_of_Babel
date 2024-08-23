using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace INab.WorldAlchemy
{
    [CustomEditor(typeof(WorldDissolve)), CanEditMultipleObjects]
    public class WorldDissolveEditor : WorldDissolveBaseEditor
    {
        private SerializedProperty masksList;
        private SerializedProperty propertiesType;
        private SerializedProperty globalPropertiesId;

        private bool waitForGlobalID = false;

        WorldDissolve targetWD;

        public override void OnEnable()
        {
            base.OnEnable();

            // Additional properties
            masksList = serializedObject.FindProperty("masksList");
            propertiesType = serializedObject.FindProperty("propertiesType");
            globalPropertiesId = serializedObject.FindProperty("globalPropertiesId");
        }

        public override void Inspector()
        {
            targetWD = ((WorldDissolve)worldDissolve);

            bool local = targetWD.ShadersType == ShaderType.Local ? true : false;

            DrawGeneral(local);
            DrawKeywords(WorldDissolve.MaxMasks);

            if ((int)targetWD.GlobalPropertiesId != globalPropertiesId.enumValueIndex)
            {
                waitForGlobalID = true;
            }


            DrawSettings(!local);
     

            if (worldDissolve.ControlMaterialsProperties)
            {
                // info about using gloabl dissolve diplsacement with performance

                switch (worldDissolve.DissolveType)
                {
                    case DissolveType.Burn:
                    DrawEdgeTypeProperties();
                        break;
                    case DissolveType.Smooth:
                    DrawSmoothTypeProperties();
                        break;
                    case DissolveType.DisplacementOnly:
                        DrawDisplacementOnlyTypeProperties();
                        break;
                }

                if(worldDissolve.UseDisplacement)
                {
                    DrawDisplacementProperties();
                }
            }

            EditorUtilities.DrawLinks(EditorUtilities.WorldDissolveLink, EditorUtilities.WorldDissolveDocs);

        }

        private void DrawGeneral(bool local)
        {
            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

            using (var verticalScope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.Space();

                if (GUILayout.Button("Refresh"))
                {
                    targetWD.RefreshWorldDissolve();
                }

                EditorGUILayout.Space();


                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(masksList);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                if(targetWD.ActiveMasks < targetWD.MasksListCount())
                    EditorGUILayout.HelpBox("Mask list count is greater than Acitve Masks property.", MessageType.Info);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(propertiesType);


                if (local)
                {
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel++;
                    DrawMaterialsList();
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }
                else
                {
                    EditorGUILayout.PropertyField(globalPropertiesId);

                    if(targetWD.IsIDValid == false)
                    {
                        EditorGUILayout.HelpBox("There is other world dissolve script in your scene with the same id, you need to change the global properties id.", MessageType.Warning);
                    }

                    EditorGUILayout.Space();
                    EditorGUI.indentLevel++;
                    DrawMaterialsList();
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.Space();

        }

        public override void UpdateKeywords()
        {
            base.UpdateKeywords();

            if (waitForGlobalID)
            {
                var targetWD = ((WorldDissolve)worldDissolve);
                targetWD.ChangeGlobalId(targetWD.GlobalPropertiesId);
                waitForGlobalID = false;
            }

        }

    }
}