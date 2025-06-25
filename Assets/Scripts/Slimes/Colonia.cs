using System.Collections.Generic;
using UnityEngine;

public class Colonia : MonoBehaviour
{
    public static Colonia Instancia { get; private set; }

    [Header("Configuraci�n del Cardumen")]
    [SerializeField]
    private int cantidadDeSlimes = 50;
    [SerializeField]
    public float tama�oDelEspacio = 50f;
    [SerializeField]
    private Slime slimePrefab;
    [SerializeField]
    private Transform objetivo;

    [Header("No tocar")]
    public List<Slime> slimes = new List<Slime>();

    public Transform Objetivo { get => objetivo; }

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(this);
            return;
        }
        Instancia = this;

        for (int i = 0; i < cantidadDeSlimes; i++)
        {
            InstanciarSlime();
        }
    }

    public void InstanciarSlime()
    {
        Vector3 posicion = new Vector3(
            Random.Range(-tama�oDelEspacio / 2f, tama�oDelEspacio / 2f),
            transform.position.y,
            Random.Range(-tama�oDelEspacio / 2f, tama�oDelEspacio / 2f)
        );
        Quaternion rotaci�n = Quaternion.Euler(
            0f,
            Random.Range(0f, 360f),
            0f
        );

        Slime nuevoSlime = Instantiate(slimePrefab, posicion, rotaci�n).GetComponent<Slime>();
        slimes.Add(nuevoSlime);
    }

}
