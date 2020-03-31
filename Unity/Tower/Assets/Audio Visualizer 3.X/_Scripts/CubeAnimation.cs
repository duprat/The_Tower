using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimation : MonoBehaviour {

	public int band;

	public float startScale;
	public float maxScale;

	void Start () {
		
	}

	void Update () {
		Colorization.Colorize(band, false, true, true, true, this.gameObject);
		transform.localScale = new Vector3 (	transform.localScale.x,
												Audio_1_0.bandBuffer [band] * maxScale + startScale,
												transform.localScale.z);
	}
}
