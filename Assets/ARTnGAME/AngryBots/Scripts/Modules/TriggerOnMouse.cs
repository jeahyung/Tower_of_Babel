﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Artngame.PDM {
	
public class TriggerOnMouse : MonoBehaviour {

		public SignalSender mouseDownSignals;
		public SignalSender mouseUpSignals;

		void Update () {
			if (Input.GetMouseButtonDown(0))
				mouseDownSignals.SendSignals (this);

			if (Input.GetMouseButtonUp(0))
				mouseUpSignals.SendSignals (this);
		}
}
}