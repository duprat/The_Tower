using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public int band, buffer;

    [HideInInspector]
    public bool Transition, Red, Green, Blue;

    [HideInInspector]
    public Color color1, color2;

    [HideInInspector]
    public float coef;

    private float startScale;
    private float maxScale;
    private float initialY;

    private bool verrouille = false;

    void Start()
    {
        initialY = transform.localScale.y;
        startScale = 5;
        maxScale = 10;
    }

    void Update()
    {
        /*
		if (Input.GetKeyDown (KeyCode.Tab)) {
			verrouille = true;
		}
		*/
        if (!verrouille)
        {
            Colorization.Colorize(band, Red, Green, Blue, Transition, this.gameObject);
            transform.localScale = new Vector3(transform.localScale.x,
                                                    (Audio_3_1._currentBandAudio[band] * maxScale + startScale) * initialY,
                                                    transform.localScale.z);
            transform.position = new Vector3(transform.position.x,
                                                    ((Audio_3_1._currentBandAudio[band] * maxScale + startScale) * initialY) / 2,
                                                    transform.position.z);
        }
        /*
		else
		{
			Colorization.Colorize(band, Red, Green, Blue, useBuffer, transitionDown, this.gameObject);
			if (GameObject.Find ("Player").transform.position.y < 10) {
				if (transform.position.y - 1 > (((Audio_3_1._bandBuffer [band] * buffer) +
					(Audio_3_1._freqBand [band] * (1 - buffer))) * maxScale + startScale) / 2) {
					transform.position = new Vector3 (	transform.position.x,
														transform.position.y - 0.5f,
														transform.position.z);
				} else {
					transform.position = new Vector3 (	transform.position.x,
														(((Audio_3_1._bandBuffer [band] * buffer) + (Audio_3_1._freqBand [band] *
														(1 - buffer))) * maxScale + startScale) / 2,
														transform.position.z);
				}
			} else {
				Vector3 position;
				position = GameObject.Find ("Player").transform.position;
				if (GameObject.Find ("Player").transform.position.y >= 50) {
					position.y += Mathf.Sin (Time.time * this.band) * 10;
				} else if (GameObject.Find ("Player").transform.position.y >= 40) {
					position.y += Mathf.Sin (Time.time) * 10;
				}
				if (GameObject.Find ("Player").transform.position.y >= 30) {
					transform.RotateAround (Vector3.zero,Vector3.up,0.25f);
				}
				if (transform.position.y < position.y - 1) {
					transform.position = new Vector3 (	transform.position.x,
														transform.position.y + 0.25f,
														transform.position.z);
				} else if (transform.position.y > position.y + 1){
					transform.position = new Vector3 (	transform.position.x,
														transform.position.y - 0.25f,
														transform.position.z);
				} else {
					transform.position = new Vector3 (	transform.position.x,
														position.y,
														transform.position.z);
				}
			}
			maxScale = GameObject.Find ("Player").transform.position.y * 2 + 1;
			if (GameObject.Find ("Player").transform.position.y > 10){
				maxScale = 25;
			}
			transform.localScale = new Vector3 (	transform.localScale.x,
													((Audio_3_1._bandBuffer [band] * buffer) + (Audio_3_1._freqBand [band] *
													(1 - buffer))) * maxScale + startScale,
													transform.localScale.z);
		}
		*/
    }
}
