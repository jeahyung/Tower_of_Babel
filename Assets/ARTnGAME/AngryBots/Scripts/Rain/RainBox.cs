﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Artngame.PDM {
public class RainBox : MonoBehaviour {

		private MeshFilter mf;	
		private Vector3 defaultPosition;
		//private Bounds bounds;

		private RainManager manager;

		private Transform cachedTransform;
		private float cachedMinY;
		private float cachedAreaHeight;
		private float cachedFallingSpeed;

		void Start() {
			manager = transform.parent.GetComponent<RainManager> ();

			//bounds = new Bounds (new Vector3 (transform.position.x, manager.minYPosition, transform.position.z),
			//	new Vector3 (manager.areaSize * 1.35f, Mathf.Max (manager.areaSize, manager.areaHeight)  * 1.35f, manager.areaSize * 1.35f));	

			mf = GetComponent<MeshFilter> ();		
			mf.sharedMesh = manager.GetPreGennedMesh ();

			cachedTransform = transform;
			cachedMinY = manager.minYPosition;
			cachedAreaHeight = manager.areaHeight;
			cachedFallingSpeed = manager.fallingSpeed;

			enabled = false;
		}

		void OnBecameVisible () {
			enabled = true;
		}

		void OnBecameInvisible () {
			enabled = false;
		}

		void Update() {		
			cachedTransform.position -= Vector3.up * Time.deltaTime * cachedFallingSpeed;

			if(cachedTransform.position.y + cachedAreaHeight < cachedMinY) {
				cachedTransform.position = cachedTransform.position + Vector3.up * cachedAreaHeight * 2.0f;
			}
		}

		void OnDrawGizmos () {
			#if UNITY_EDITOR
			// do not display a weird mesh in edit mode
			if (!Application.isPlaying) {
				mf = GetComponent<MeshFilter> ();		
				mf.sharedMesh = null;	
			}
			#endif

			if (transform.parent) {
				Gizmos.color = new Color(0.2f,0.3f,3.0f,0.35f);
				RainManager manager = transform.parent.GetComponent<RainManager> (); //transform.parent.GetComponent (RainManager) as RainManager; //v2.3
				if (manager)
					Gizmos.DrawWireCube (	transform.position + transform.up * manager.areaHeight * 0.5f, 
						new Vector3 (manager.areaSize,manager.areaHeight, manager.areaSize) );
			}
		}
}
}