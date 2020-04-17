using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class dataArduino : MonoBehaviour
{
    SerialPort puerton = new SerialPort("COM5", 9600);
 
    // Start is called before the first frame update
    void Start()
    {
        puerton.Open();
        puerton.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (puerton.IsOpen)
        {
            try
            {
                print(puerton.ReadByte());

            }
            catch(System.Exception)
            {

            }
        }
    }
}
