using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour {

	public float _startScale, _maxScale;
	public float _red, _green, _blue;

	Material _material;

	void Start () {
		_material = GetComponent<MeshRenderer>().material;
	}

	void Update () {
		transform.localScale = new Vector3((Audio_3_1._currentAmplitude * _maxScale) + _startScale, (Audio_3_1._currentAmplitude * _maxScale) + _startScale, (Audio_3_1._currentAmplitude * _maxScale) + _startScale);
		Color _color = new Color(_red * Audio_3_1._currentAmplitude, _green * Audio_3_1._currentAmplitude, _blue * Audio_3_1._currentAmplitude);
		_material.SetColor("_Color", _color);
		_material.SetColor("_EmissionColor", _color);
	}
}
