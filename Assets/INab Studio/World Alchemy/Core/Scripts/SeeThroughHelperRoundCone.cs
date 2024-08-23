using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace INab.WorldAlchemy
{
    public class SeeThroughHelperRoundCone : SeeThroughHelperBase
    {
        [Serializable]
        public class MaskObject : MaskObjectBase
        {
            [Header("Camera Follow Settings")]
            [Tooltip("Transform of the camera mask.")]
            public Transform cameraMaskTransform;

            public void InitializeCameraMaskScale(float defaultCameraMaskScale)
            {
                if (enableDynamicScaling) AdjustTransformScale(cameraMaskTransform, defaultCameraMaskScale);
            }

        }

        [Header("Masks Configuration")]
        [Tooltip("List of mask objects to manage.")]
        public List<MaskObject> maskObjects = new List<MaskObject>();

        [Header("Performance Tweaks")]
        [Tooltip("Forces an immediate update of mask parameters to mitigate potential lag effects.")]
        public bool forceUpdateMaskParameters = true;
        public WorldDissolve worldDissolve;

        [Header("Custom Camera Update")]
        [Tooltip("Manually triggers updates to custom camera scripts, typically during LateUpdate. See documentation for integration details."),Space]
        public UnityEvent onCameraUpdate;

        [Header("General Settings")]
        public Transform cameraTransform;

        [Header("Camera Mask Behavior")]
        [Tooltip("Enables the mask to follow the camera's position smoothly.")]
        public bool enableCameraMaskFollow = true;

        [Tooltip("Defines the initial scale of the camera mask. Smaller values are generally more effective.")]
        public float initialCameraMaskScale = 0.01f;


        protected void Start()
        {
            SetCameraTransform(cameraTransform); 

            foreach (var maskObject in maskObjects)
            {
                StartCall(maskObject);
                maskObject.InitializeCameraMaskScale(initialCameraMaskScale); 
            }
        }

        protected void LateUpdate()
        {
            onCameraUpdate.Invoke(); // Invoke custom camera updates.

            foreach (var maskObject in maskObjects)
            {
                LateUpdateCall(maskObject);
                if (enableCameraMaskFollow)
                {
                    maskObject.cameraMaskTransform.position = cameraTransform.position;
                }
            }

            // Ensure masks are updated promptly to prevent latency issues.
            if (forceUpdateMaskParameters)
            {
                worldDissolve.ForceUpdateMasksParameters();
            }
        }


        protected override void Update()
        {
            base.Update();

            foreach (var maskObject in maskObjects)
            {
                UpdateCall(maskObject);
            }
        }
    }
}