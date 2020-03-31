using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFreqs : MonoBehaviour
{

    public GameObject sampleCubePrefab;

    public int sizeOfSamples;
    public int avancement;

    public float maxScale;

    private GameObject[] _sampleCubes;

    // Start is called before the first frame update
    void Start()
    {
        _sampleCubes = new GameObject[sizeOfSamples];

        for (int i = 0; i < sizeOfSamples; i++)
        {
            GameObject _instanceSampleCube = (GameObject)Instantiate(sampleCubePrefab);
            _instanceSampleCube.transform.position = this.transform.position;
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "Sample Cube" + i;
            this.transform.eulerAngles = new Vector3(0, (float) - 360 * i / _sampleCubes.Length, 0);
            _instanceSampleCube.transform.position = Vector3.forward * avancement;
            _sampleCubes[i] = _instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < sizeOfSamples; i++)
        {
            if (_sampleCubes != null)
            {
                _sampleCubes[i].transform.localScale = new Vector3(1, (Audio_3_1._sampleSpectrum[i] * maxScale * 10000) + 1, 1);
            }
        }
    }
}
