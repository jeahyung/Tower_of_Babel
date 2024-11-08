﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artngame.PDM {
	[System.Serializable]
public class MoodBoxData {

		public float noiseAmount = 0.375f;
		public float colorMixBlend = 0.0f;
		public Color colorMix = Color.green;
		public float fogY = -10.0f;
		public Color fogColor = Color.black;
		public string adventureString = "";
		public bool outside = false;
}
}