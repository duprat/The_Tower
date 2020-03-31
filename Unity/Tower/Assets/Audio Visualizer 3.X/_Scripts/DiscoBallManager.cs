using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBallManager : MonoBehaviour {
	public int nbRayons;
	public int lenteur;
	public int correctionX;
	public int correctionY;
	private int compteur;
	private int red;
	private int blue;
	private int green;
	private int useBuffer;
	private int transitionDown;
	public GameObject player;
	public GameObject Rayon;
	private Vector3 offset;

	void Start () {
		offset = transform.position;
		float ecart = 30f / nbRayons;
		for (int i = 0 - (nbRayons / 2); i < nbRayons - (nbRayons / 2); i++) {
			for (int j = 0 - (nbRayons / 2); j < nbRayons - (nbRayons / 2); j++) {
				GameObject instanceRayon = (GameObject)Instantiate (Rayon);
				instanceRayon.transform.position = this.transform.position;
				instanceRayon.transform.parent = this.transform;
				instanceRayon.name = "Rayon" + (i + (nbRayons / 2))+ (j + (nbRayons / 2));
				instanceRayon.transform.eulerAngles = new Vector3 (ecart * i, ecart * j, 0);
			}
		}
		transform.eulerAngles = new Vector3 (correctionX, correctionY, 0);
	}

	void Update () {
		transform.position = offset + new Vector3 (0, player.transform.position.y, 0);
		transform.Rotate (0, 0, 1);
		if (compteur == 0) {
			foreach (Transform child in transform){
				red = (int)Random.Range(0,2);
				green = (int)Random.Range(0,2);
				blue = (int)Random.Range(0,2);
				child.GetComponent<Light> ().color = new Color (red * 255, green * 255, blue * 255);
			}
			compteur = lenteur;
		} else {
			compteur--;							
		}
	}
}