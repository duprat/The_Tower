using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamSphere : MonoBehaviour
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

    // Start is called before the first frame update
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
    }
}
