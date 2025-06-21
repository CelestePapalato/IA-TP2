using System.Collections.Generic;
using UnityEngine;

public class Cardumen : MonoBehaviour
{
    public static Cardumen Instancia { get; private set; }

    [Header("Configuraci�n del Cardumen")]
    [SerializeField]
    private int cantidadDePeces = 50;
    [SerializeField]
    public float tama�oDelEspacio = 50f;
    [SerializeField]
    private Pez pezPrefab;
    [SerializeField]
    private Transform objetivo;

    [Header("No tocar")]
    public List<Pez> peces = new List<Pez>();

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(this);
            return;
        }
        Instancia = this;

        for (int i = 0; i < cantidadDePeces; i++)
        {
            Vector3 posicion = new Vector3(
                Random.Range(-tama�oDelEspacio / 2f, tama�oDelEspacio / 2f),
                Random.Range(-tama�oDelEspacio / 2f, tama�oDelEspacio / 2f),
                Random.Range(-tama�oDelEspacio / 2f, tama�oDelEspacio / 2f)
            );
            Quaternion rotaci�n = Quaternion.Euler(
                Random.Range(0f, 360f),
                Random.Range(0f, 360f),
                Random.Range(0f, 360f)
            );

            Pez nuevoPez = Instantiate(pezPrefab, posicion, rotaci�n).GetComponent<Pez>();
            peces.Add(nuevoPez);
        }
    }

}
