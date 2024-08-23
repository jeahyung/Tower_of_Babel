using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.WorldAlchemy
{
    [ExecuteAlways]
    /// <summary>
    /// World dissolve with global and local properties type. Up to 4 masks at a time.
    /// </summary>
    public class WorldDissolve : WorldDissolveBase
    {
        #region staticProperties

        private static List<string> globalIdsKeywords = new List<string>()
        {
            "_WORLDDISSOLVE_ID__1",
            "_WORLDDISSOLVE_ID__2",
            "_WORLDDISSOLVE_ID__3",
            "_WORLDDISSOLVE_ID__4",
        };

        private static int GlobalIdsCount = 2;

        #endregion

        #region privateProperties

        public ShaderType ShadersType
        {
            get
            {
                return propertiesType;
            }
            private set { }
        }
        [SerializeField, Tooltip("Choose between global or local keywords for shader properties.")]
        private ShaderType propertiesType = ShaderType.Local;

        /// <summary>
        /// Indicates whether the ID for global properties is valid and unique.
        /// </summary>
        public bool IsIDValid
        {
            get
            {
                return isIDValid;
            }
            private set { }
        }
        private bool isIDValid = true;
        private List<bool> globalPropertiesIdList = new List<bool>();

        public GlobalPropertiesID GlobalPropertiesId
        {
            get
            {
                return globalPropertiesId;
            }
            private set { }
        }
        [SerializeField, Tooltip("Unique identifier for each set of world dissolve global properties. Allows for up to 2 distinct configurations.")]
        private GlobalPropertiesID globalPropertiesId = GlobalPropertiesID._1;

        [SerializeField, Tooltip("List of masks used in the world dissolve effect. Masks at the top have higher priority.")]
        private List<Mask> masksList = new List<Mask>();

        #endregion

        #region sdfVectors

        // Common
        private Vector4[] positions = new Vector4[MaxMasks];
        private Vector4[] scales = new Vector4[MaxMasks];

        // Plane Rotation
        private Vector4[] upVectors = new Vector4[MaxMasks];

        // Common Rotation
        private Vector4[] forwardVectors = new Vector4[MaxMasks];
        private Vector4[] rightVectors = new Vector4[MaxMasks];

        // Round Cones
        private Vector4[] positions_2 = new Vector4[MaxMasks];

        #endregion

        #region overrideFunctions

        public override void RefreshWorldDissolve()
        {
            base.RefreshWorldDissolve();

            // Additionally update properties type.
            foreach (var material in materialsDictionary)
            {
                DisableGlobalIds(material.Key);
                EnableGlobalId(material.Key);
            }
        }

        public override void OnValidate()
        {
            CheckForGlobalShaderType();

            base.OnValidate();
        }

        protected override void MasksTriggerSubscribe()
        {
            if (useAutoDetection)
            {
                foreach (var mask in masksList)
                {
                    mask.OnMaskTriggerEnter += TriggerEnter;
                    mask.OnMaskTriggerExit += TriggerExit;
                }
            }
        }

        protected override void MasksTriggerUnSubscribe()
        {
            if (useAutoDetection)
            {
                foreach (var mask in masksList)
                {
                    mask.OnMaskTriggerEnter -= TriggerEnter;
                    mask.OnMaskTriggerExit -= TriggerExit;
                }
            }
        }

        public override void UpdateMaterialsParameters()
        {
            if(propertiesType == ShaderType.Local)
            {
                UpdateMaterialsDictionaryProperties();
            }
            else
            {
                UpdateMaterialsGlobalProperties();
            }
        }

        protected override void UpdateParameters()
        {
            int i = 0;

            // Automatically checks if we have enought masks to cover active masks number 
            int numberOfMasks = Mathf.Min(ActiveMasks, masksList.Count);

            for (int defaultIndex = numberOfMasks; defaultIndex < MaxMasks; defaultIndex++)
            {
                ResetToDefaultVectors(ref positions[defaultIndex], ref forwardVectors[defaultIndex], ref rightVectors[defaultIndex], ref upVectors[defaultIndex]);
            }

            foreach (var mask in masksList)
            {
                if (i + 1 > numberOfMasks) break;

                if (mask != null) SetSDFVectors(mask, ref positions[i], ref scales[i], ref forwardVectors[i], ref rightVectors[i], ref upVectors[i], ref positions_2[i]);

                i++;
            }
        }

        protected override void CheckMasksForNulls()
        {
            List<Mask> newMasksList = new List<Mask>();
            foreach (var mask in masksList)
            {
                if (mask != null)
                {
                    newMasksList.Add(mask);
                }
            }
            masksList = newMasksList; 
        }

        protected override void UpdateMasksType()
        {
            foreach (var mask in masksList)
            {
                mask.Type = Type;
                if(UseAutoDetection)
                {
                    mask.UpdateCollider();
                }
            }
        }

        protected override void UpdateMasksDetection()
        {
            foreach (var mask in masksList)
            {
                mask.DetectionLayerMask = detectionLayerMask;
                mask.UseAutoDetection = useAutoDetection;
            }
        }

        #endregion

        #region publicFunctions

        private void CheckForGlobalShaderType()
        {
            if (propertiesType == ShaderType.Global)
            {
                useAutoDetection = false;
                copyEditorLists = true;

                // Checking for repeating global ids in scene
                globalPropertiesIdList.Clear();
                for (int i = 0; i < GlobalIdsCount; i++)
                {
                    globalPropertiesIdList.Add(false);
                }

                foreach (var worldDissolve in FindObjectsOfType<WorldDissolve>())
                {
                    if (worldDissolve != this && worldDissolve.ShadersType == ShaderType.Global)
                    {
                        globalPropertiesIdList[(int)worldDissolve.GlobalPropertiesId] = true;
                    }
                }

                if (globalPropertiesIdList[(int)GlobalPropertiesId] == true)
                {
                    isIDValid = false;
                }
                else
                {
                    isIDValid = true;
                }
            }
        }

        /// <summary>
        /// Changes the type of shader properties used by the materials managed by this script.
        /// </summary>
        /// <param name="newShaderType"></param>
        public void ChangeShadersType(ShaderType newShaderType)
        {
            propertiesType = newShaderType;

            CheckForGlobalShaderType();

            UpdateMaterialsParameters();
        }

        /// <summary>
        /// Updates the ID for global properties, affecting all materials managed by this script.
        /// </summary>
        /// <param name="id">The new global properties ID to set.</param>
        public void ChangeGlobalId(GlobalPropertiesID id)
        {
            globalPropertiesId = id;

            foreach (var material in materialsDictionary)
            {
                DisableGlobalIds(material.Key);
                EnableGlobalId(material.Key);
            }
        }

        /// <summary>
        /// Adds a mask to the dissolve effect, subscribing to its events if auto detection is used.
        /// </summary>
        /// <param name="mask">The mask to add.</param>
        public void AddMask(Mask mask)
        {
            if (mask == null) return;

            if (useAutoDetection)
            {
                mask.UpdateCollider();
                mask.OnMaskTriggerEnter += TriggerEnter;
                mask.OnMaskTriggerExit += TriggerExit;
            }

            mask.Type = Type;
            mask.DetectionLayerMask = detectionLayerMask;
            mask.UseAutoDetection = useAutoDetection;

            masksList.Add(mask);
        }

        /// <summary>
        /// Removes a specific mask from the dissolve effect, unsubscribing from its events.
        /// </summary>
        /// <param name="mask">The mask to remove.</param>
        /// <returns>True if the mask was successfully removed, false otherwise.</returns>
        public bool RemoveMask(Mask mask)
        {
            if (mask == null) return false;

            if (useAutoDetection)
            {
                mask.DestroyCollider();
                mask.OnMaskTriggerEnter -= TriggerEnter;
                mask.OnMaskTriggerExit -= TriggerExit;
            }

            return masksList.Remove(mask);
        }

        /// <summary>
        /// Removes a mask at a specific index from the dissolve effect, unsubscribing from its events.
        /// </summary>
        /// <param name="index">The index of the mask to remove.</param>
        /// <returns>True if a mask was removed, false otherwise.</returns>
        public bool RemoveMask(int index)
        {
            if (masksList.Count < index) return false;

            if (useAutoDetection)
            {
                var mask = masksList[index];
                if (mask != null)
                {
                    mask.DestroyCollider();
                    mask.OnMaskTriggerEnter -= TriggerEnter;
                    mask.OnMaskTriggerExit -= TriggerExit;
                }
            }
            
            masksList.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes all masks from the dissolve effect, unsubscribing from their events.
        /// </summary>
        public void RemoveAllMasks()
        {
            if (useAutoDetection)
            {
                foreach (var mask in masksList)
                {
                    if (mask != null)
                    {
                        mask.DestroyCollider();
                        mask.OnMaskTriggerEnter -= TriggerEnter;
                        mask.OnMaskTriggerExit -= TriggerExit;
                    }
                }
            }

            masksList.Clear();
        }

        /// <summary>
        /// Checks if a specific mask is part of the dissolve effect.
        /// </summary>
        /// <param name="mask">The mask to check for.</param>
        /// <returns>True if the mask is part of the dissolve effect, false otherwise.</returns>
        public bool ContainsMask(Mask mask)
        {
            if (mask == null) return false;

            return masksList.Contains(mask);
        }

        /// <summary>
        /// Returns the count of masks currently part of the dissolve effect.
        /// </summary>
        /// <returns>The number of masks in the dissolve effect.</returns>
        public int MasksListCount()
        {
            return masksList.Count;
        }

        #endregion

        #region materialsKeywords

        /// <summary>
        /// Disables all type keywords in material.
        /// </summary>
        /// <param name="material"></param>
        private void DisableGlobalIds(Material material)
        {
            foreach (var keyword in material.enabledKeywords)
            {
                if (globalIdsKeywords.Contains(keyword.name))
                {
                    material.DisableKeyword(keyword);
                }
            }
        }

        /// <summary>
        /// Enables type keyword based on current mask type.
        /// </summary>
        /// <param name="material"></param>
        private void EnableGlobalId(Material material)
        {
            material.EnableKeyword(globalIdsKeywords[(int)globalPropertiesId]);
        }


        #endregion

        #region materialsProperties

        private void UpdateMaterialsGlobalProperties()
        {
            string prefix = "WorldDissolve_";

            switch (globalPropertiesId)
            {
                case GlobalPropertiesID._1:
                    prefix += "1_";
                    break;
                case GlobalPropertiesID._2:
                    prefix += "2_";
                    break;
                    // GLOBALIDNUMBER
                    //case GlobalPropertiesID._3:
                    //    prefix += "3_";
                    //    break;
                    //case GlobalPropertiesID._4:
                    //    prefix += "4_";
                    //    break;
            }

            Shader.SetGlobalVectorArray(prefix + "Positions", positions);
            Shader.SetGlobalVectorArray(prefix + "Scales", scales);
            Shader.SetGlobalVectorArray(prefix + "Ups", upVectors);
            Shader.SetGlobalVectorArray(prefix + "Rights", rightVectors);
            Shader.SetGlobalVectorArray(prefix + "Forwards", forwardVectors);
            Shader.SetGlobalVectorArray(prefix + "Positions_2", positions_2);
        }

        
        protected override void UpdateCommonProperties(Material material)
        {
            material.SetVectorArray("Positions", positions);
            material.SetVectorArray("Scales", scales);
        }

        protected override void UpdateRotation(Material material)
        {
            material.SetVectorArray("Ups", upVectors);
            material.SetVectorArray("Rights", rightVectors);
            material.SetVectorArray("Forwards", forwardVectors);
        }

        protected override void UpdateUpVector(Material material)
        {
            material.SetVectorArray("Ups", upVectors);
        }

        protected override void UpdateRoundCone(Material material)
        {
            material.SetVectorArray("Positions_2", positions_2);
        }

        #endregion
    }
}