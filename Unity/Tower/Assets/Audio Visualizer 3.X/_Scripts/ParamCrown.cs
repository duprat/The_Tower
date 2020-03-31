using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCrown : MonoBehaviour
{


    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public int band, buffer;

    [HideInInspector]
    public bool useBuffer, Transition, Red, Green, Blue;

    private float startScale;
    private float maxScale;

    private bool verrouille = false;

    private float initX, initY, initZ;

    void Start()
    {
        initX = transform.position.x;
        initY = transform.localScale.y;
        initZ = transform.position.z;
        startScale = 5;
        if (useBuffer)
            buffer = 1;
        else
            buffer = 0;
    }

    void Update()
    {
        player = GameObject.Find("Player2.0(Clone)");
        if (player != null)
        {
            /*
			if (Input.GetKeyDown (KeyCode.Tab)) {
				verrouille = true;
			}
			*/
            if (!verrouille)
            {
                maxScale = player.transform.position.y + 1;
                Colorization.Colorize(band, Red, Green, Blue, Transition, this.gameObject);
                transform.localScale = new Vector3(transform.localScale.x,
                                                        (Audio_3_1._currentBandFreq[band] * 10 + startScale) * initY,
                                                        transform.localScale.z);
                transform.position = new Vector3(initX,
                                                        ((Audio_3_1._currentBandFreq[band] * 10 + startScale) / 2) * initY + (float) 1/4,
                                                        initZ) + player.transform.position;
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
}
