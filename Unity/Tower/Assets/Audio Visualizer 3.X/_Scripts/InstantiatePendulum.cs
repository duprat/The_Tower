using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePendulum : MonoBehaviour
{
    public int nbInstances;
    public float avancement;
    private int nbSpheres;

    private bool Red = false;
    private bool Green = false;
    private bool Blue = false;
    private bool Transition = false;
    private bool verrouille = false;

    public GameObject Sphere;
    public GameObject Player;
    GameObject[] pendulum;

    readonly bool[,] couleurs = new bool[,]{
        {true, true, false, false},
        {true, true, false, true},
        {false, true, true, false},
        {false, true, true, true},
        {true, false, true, false},
        {true, false, true, true}};

    void Start()
    {
        nbSpheres = Audio_3_1._numberOfFrequencyBands * nbInstances;
        pendulum = new GameObject[nbSpheres];
        CreatePendulum();
    }
    void CreatePendulum()
    {
        for (int i = 0; i < nbSpheres; i++)
        {
            Debug.LogWarning(nbSpheres);
            GameObject instanceSphere = (GameObject)Instantiate(Sphere);
            instanceSphere.transform.position = this.transform.position;
            instanceSphere.transform.parent = this.transform;
            instanceSphere.name = "PendulumSphere" + i;
            instanceSphere.GetComponent<ParamSphere>().player = Player;
            instanceSphere.GetComponent<ParamSphere>().Red = false;
            instanceSphere.GetComponent<ParamSphere>().Green = false;
            instanceSphere.GetComponent<ParamSphere>().Blue = false;
            instanceSphere.GetComponent<ParamSphere>().Transition = false;
            instanceSphere.transform.position = Vector3.forward * avancement;
            instanceSphere.transform.position += Vector3.up * 10 * (i + 1);
            instanceSphere.GetComponent<ParamSphere>().band = i % Audio_3_1._numberOfFrequencyBands;
            pendulum[i] = instanceSphere;
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
                    for (int i = 0; i < nbSpheres / 6; i++)
                    {
                        pendulum[j * nbSpheres / 6 + i].GetComponent<ParamSphere>().Red = couleurs[j, 0];
                        pendulum[j * nbSpheres / 6 + i].GetComponent<ParamSphere>().Green = couleurs[j, 1];
                        pendulum[j * nbSpheres / 6 + i].GetComponent<ParamSphere>().Blue = couleurs[j, 2];
                        pendulum[j * nbSpheres / 6 + i].GetComponent<ParamSphere>().Transition = couleurs[j, 3];
                    }
                }
            }
            else
            {
                for (int i = 0; i < nbSpheres; i++)
                {
                    pendulum[i].GetComponent<ParamSphere>().Red = Red;
                    pendulum[i].GetComponent<ParamSphere>().Green = Green;
                    pendulum[i].GetComponent<ParamSphere>().Blue = Blue;
                    pendulum[i].GetComponent<ParamSphere>().Transition = Transition;
                }
            }
        }
    }
}
