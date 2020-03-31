using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCrown : MonoBehaviour
{
    public int nbInstances;
    public float avancement;
    private int nbBarres;
    private float ecart;
    private bool hasBeenCreated = false;
    public GameObject Cube;
    private GameObject Player;
    GameObject[] egalizer;

    readonly bool[,] couleurs = new bool[,]{
        {true, true, false, false},
        {true, true, false, true},
        {false, true, true, false},
        {false, true, true, true},
        {true, false, true, false},
        {true, false, true, true}};

    void Start()
    {
        nbBarres = Audio_3_1._numberOfFrequencyBands * nbInstances;
        egalizer = new GameObject[nbBarres];
        ecart = 360f / nbBarres;
    }

    void CreateCrown()
    {
        for (int i = 0; i < nbBarres; i++)
        {
            GameObject instanceCube = (GameObject)Instantiate(Cube);
            instanceCube.transform.position = this.transform.position;
            instanceCube.transform.parent = this.transform;
            instanceCube.name = "CrownCube" + i;
            instanceCube.GetComponent<ParamCrown>().player = Player;
            instanceCube.GetComponent<ParamCrown>().Red = false;
            instanceCube.GetComponent<ParamCrown>().Green = false;
            instanceCube.GetComponent<ParamCrown>().Blue = false;
            instanceCube.GetComponent<ParamCrown>().Transition = false;
            instanceCube.transform.position = Player.transform.position + Vector3.forward * avancement + Vector3.up * 3;
            instanceCube.transform.RotateAround(Player.transform.position, Vector3.up, (float)(ecart * i));
            instanceCube.GetComponent<ParamCrown>().band = i % Audio_3_1._numberOfFrequencyBands;
            egalizer[i] = instanceCube;
        }
    }
    
    void Update()
    {
        Player = GameObject.Find("Player2.0(Clone)");
        if (Player != null)
        {
            if (!hasBeenCreated)
            {
                CreateCrown();
                hasBeenCreated = true;
            }
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < nbBarres / 6; i++)
                {
                    egalizer[j * nbBarres / 6 + i].GetComponent<ParamCrown>().Red = couleurs[j, 0];
                    egalizer[j * nbBarres / 6 + i].GetComponent<ParamCrown>().Green = couleurs[j, 1];
                    egalizer[j * nbBarres / 6 + i].GetComponent<ParamCrown>().Blue = couleurs[j, 2];
                    egalizer[j * nbBarres / 6 + i].GetComponent<ParamCrown>().Transition = couleurs[j, 3];
                }
            }
        }
    }
}
