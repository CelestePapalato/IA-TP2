using System.Collections.Generic;
using UnityEngine;

public class Colonia: MonoBehaviour
{
    public static Colonia Instancia { get; private set; }

    [Header("Configuración del Cardumen")]
    [SerializeField]
    private int cantidadDeGallinas = 50;
    [SerializeField]
    public float tamañoDelEspacio = 50f;
    [SerializeField]
    private Gallina slimePrefab;
    [SerializeField]
    private Transform objetivo;

    [Header("No tocar")]
    public List<Gallina> slimes = new List<Gallina>();

    public Transform Objetivo { get => objetivo; }

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(this);
            return;
        }
        Instancia = this;
    }

    private void Start()
    {
        AjustarCantidadDeGallinas();
        InvokeRepeating(nameof(AjustarCantidadDeGallinas), 2f, 2f);
    }

    private void AjustarCantidadDeGallinas()
    {
        slimes.RemoveAll(slime => slime == null);

        int slimesActuales = slimes.Count;

        if (slimesActuales < cantidadDeGallinas)
        {
            int slimesPorInstanciar = cantidadDeGallinas - slimesActuales;
            for (int i = 0;i < slimesPorInstanciar;i++)
            {
                InstanciarGallina();
            }
        }
        else if (slimesActuales > cantidadDeGallinas)
        {
            int slimesPorEliminar = slimesActuales - cantidadDeGallinas;
            for (int i = 0;i < slimesPorEliminar;i++)
            {
                if (slimes.Count > 0)
                {
                    Gallina slimeAEliminar = slimes[slimes.Count - 1];
                    slimes.RemoveAt(slimes.Count - 1);
                    Destroy(slimeAEliminar.gameObject);
                }
            }
        }

    }

    public void InstanciarGallina()
    {
        Vector3 posicion = new Vector3(
            Random.Range(-tamañoDelEspacio / 2f, tamañoDelEspacio / 2f),
            transform.position.y,
            Random.Range(-tamañoDelEspacio / 2f, tamañoDelEspacio / 2f)
        );
        Quaternion rotación = Quaternion.Euler(
            0f,
            Random.Range(0f, 360f),
            0f
        );

        Gallina nuevoGallina = Instantiate(slimePrefab, posicion, rotación, this.transform).GetComponent<Gallina>();
        slimes.Add(nuevoGallina);
    }
}