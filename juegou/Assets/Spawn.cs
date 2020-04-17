using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] Figuras;
    // Start is called before the first frame update
    void Start()
    {
        NuevasFiguras();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NuevasFiguras()
    {
        Instantiate(Figuras[Random.Range(0, Figuras.Length)], transform.position, Quaternion.identity);
    }
}
