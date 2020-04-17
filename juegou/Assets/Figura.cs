using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class Figura : MonoBehaviour
{
    SerialPort puerton = new SerialPort("COM5", 9600);


    private float vel;
    private float vel2;
    public float dir;
    public float dir2;
    private float tiempoA;
    public float tiempoCaida = 0.8f;
    // delimita su espacio de accion
    public static int alto = 20;
    public static int ancho = 30;

    public Vector3 puntoRotacion;

    private static Transform[,] grid = new Transform[ancho, alto];

    public static int puntaje = 0;

    public static int nivel = 0;
    // Start is called before the first frame update
    void Start()
    {
        puerton.Open();
        puerton.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // desde aqui son los limites
       if(Input.GetKeyDown(KeyCode.LeftArrow) )
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!limites())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!limites())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        if (Time.time-tiempoA>(Input .GetKeyDown(KeyCode.DownArrow) ?tiempoCaida/20:tiempoCaida))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!limites())
            {
                transform.position -= new Vector3(0, -1, 0);

                nuevaFigura();

                RevisarLineas();
                this.enabled = false;

                FindObjectOfType < Spawn > ().NuevasFiguras();
            }
            tiempoA = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(puntoRotacion), new Vector3(0, 0, 1), -90);
            if(!limites())
            {
                transform.RotateAround(transform.TransformPoint(puntoRotacion), new Vector3(0, 0, 1), 90);
            }
        }// hasta aca
       
        /*if (puerton.IsOpen)
        {
            try
            {
                mover(puerton.ReadLine());

            }
            catch (System.Exception)
            {

            }
        }*/
        Niveles();
        aumentarDificultad();

    }

    /*void cambio(int direccion)
    {
        if(direccion == 1)
        {
            transform.RotateAround(transform.TransformPoint(puntoRotacion), new Vector3(0, 0, 1), -90);
            if (!limites())
            {
                transform.RotateAround(transform.TransformPoint(puntoRotacion), new Vector3(0, 0, 1), 90);
            }
        }
    }*/

    /*void mover(string dataArduino)
    {
        string[] datosArray = dataArduino.Split(char.Parse(","));

        if (datosArray.Length == 2)
        {
            dir = int.Parse(datosArray[0]);
            dir2 = int.Parse(datosArray[1]);
            print(dir + "   " + dir2);
        }

        if (dir >= 500)
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!limites())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }

        if (dir < 500)
        {
            transform.position += new Vector3(1, 0, 0);
            if (!limites())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }

        if (dir2 < 600)
        {
            transform.RotateAround(transform.TransformPoint(puntoRotacion), new Vector3(0, 0, 1), -90);
            if (!limites())
            {
                transform.RotateAround(transform.TransformPoint(puntoRotacion), new Vector3(0, 0, 1), 90);
            }

        }
    }*/
    bool limites()
    {
        foreach(Transform hijo in transform)
        {
            int enteroX = Mathf.RoundToInt(hijo.transform.position.x);
            int enteroY = Mathf.RoundToInt(hijo.transform.position.y);

            if(enteroX<0 || enteroX>=ancho || enteroY<0 || enteroY>=alto)
            {
                return false;
            }
            if (grid[enteroX,enteroY]!=null)
            {
                return false;
            }
        }

        
        return true;
    }
    void nuevaFigura()
    {
        foreach(Transform hijo in transform)
        {
            int enteroX = Mathf.RoundToInt(hijo.transform.position.x);
            int enteroy = Mathf.RoundToInt(hijo.transform.position.y);

            grid[enteroX, enteroy] = hijo;

            if(enteroy>=19)
            {
                puntaje = 0;
                nivel = 0;
                tiempoCaida = 0.8f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }
    void RevisarLineas()
    {
        for(int i= alto-1;i>=0;i--)
        {
            if(TieneLinea(i))
            {
                BorrarLinea(i);
                BajarLinea(i);
            }
        }
    }

    bool TieneLinea(int i)
    {
        for (int j = 0; j < ancho; j++)
        {
            if(grid[j,i]==null)
            {
                return false;
            }
        }
        puntaje += 100;
        Debug.Log(puntaje);
        return true;
    }
    void BorrarLinea(int i)
    {
        for (int j = 0; j < ancho; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }

    }

    void BajarLinea(int i)
    {
        for (int y = i; y < alto; y++)
        {
            for (int j = 0; j < ancho; j++)
            {
                if(grid[j,y]!=null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);

                }
            }
        }
    }
    void Niveles()
    {
        switch(puntaje)
        {
            case 200:
                nivel = 1;
                break;

            case 400:
                nivel = 1;
                break;

            case 600:
                nivel = 1;
                break;
        }
    }
    void aumentarDificultad()
    {
        switch(nivel)

        {
            case 1:
            tiempoCaida = 0.4f;
                break;

            case 2:
                tiempoCaida = 0.2f;
                break;

            case 3:
                tiempoCaida = 0.015f;
                break;


        }
    }
}
