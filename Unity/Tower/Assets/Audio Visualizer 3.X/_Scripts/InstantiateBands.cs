using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBands : MonoBehaviour
{
    public int nbInstances;
    public float avancement;
    private int nbBarres;
    private float ecart;
    private bool Red = false;
    private bool Green = false;
    private bool Blue = false;
    private bool Transition = false;
    private bool verrouille = false;
    public GameObject Cube;
    public GameObject Player;
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
        CreateEgalizer();
    }
    void CreateEgalizer()
    {
        for (int i = 0; i < nbBarres; i++)
        {
            GameObject instanceCube = (GameObject)Instantiate(Cube);
            instanceCube.transform.position = this.transform.position;
            instanceCube.transform.parent = this.transform;
            instanceCube.name = "EgalizerCube" + i;
            instanceCube.GetComponent<ParamCube>().player = Player;
            instanceCube.GetComponent<ParamCube>().Red = false;
            instanceCube.GetComponent<ParamCube>().Green = false;
            instanceCube.GetComponent<ParamCube>().Blue = false;
            instanceCube.GetComponent<ParamCube>().Transition = false;
            instanceCube.transform.position = Vector3.forward * avancement;
            instanceCube.transform.RotateAround(Vector3.zero, Vector3.up, (float)(ecart * i));
            instanceCube.GetComponent<ParamCube>().band = i % Audio_3_1._numberOfFrequencyBands;
            egalizer[i] = instanceCube;
        }
    }

    void Update()
    {
        if (!verrouille)
        {
            Red = UIManager.current.red();
            Green = UIManager.current.green();
            Blue = UIManager.current.blue();
            Transition = UIManager.current.transition();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                verrouille = true;
            }
            MajEgalizer(Red, Green, Blue, Transition);
        }
    }

    void MajEgalizer(bool Red, bool Green, bool Blue, bool Transition)
    {
        if (!verrouille)
        {
            if (Red && Green && Blue)
            {
                for (int j = 0; j < 6; j++)
                {
                    for (int i = 0; i < nbBarres / 6; i++)
                    {
                        egalizer[j * nbBarres / 6 + i].GetComponent<ParamCube>().Red = couleurs[j, 0];
                        egalizer[j * nbBarres / 6 + i].GetComponent<ParamCube>().Green = couleurs[j, 1];
                        egalizer[j * nbBarres / 6 + i].GetComponent<ParamCube>().Blue = couleurs[j, 2];
                        egalizer[j * nbBarres / 6 + i].GetComponent<ParamCube>().Transition = couleurs[j, 3];
                    }
                }
            }
            else
            {
                for (int i = 0; i < nbBarres; i++)
                {
                    egalizer[i].GetComponent<ParamCube>().Red = Red;
                    egalizer[i].GetComponent<ParamCube>().Green = Green;
                    egalizer[i].GetComponent<ParamCube>().Blue = Blue;
                    egalizer[i].GetComponent<ParamCube>().Transition = Transition;
                }
            }
        }
    }
}
