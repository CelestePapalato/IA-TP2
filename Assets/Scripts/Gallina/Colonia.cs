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
    private Gallina gallinaPrefab;
    [SerializeField]
    private Transform objetivo;

    [Header("No tocar")]
    public List<Gallina> gallinas = new List<Gallina>();

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
        gallinas.RemoveAll(gallina => gallina == null);

        int gallinasActuales = gallinas.Count;

        if (gallinasActuales < cantidadDeGallinas)
        {
            int gallinasPorInstanciar = cantidadDeGallinas - gallinasActuales;
            for (int i = 0;i < gallinasPorInstanciar;i++)
            {
                InstanciarGallina();
            }
        }
        else if (gallinasActuales > cantidadDeGallinas)
        {
            int gallinasPorEliminar = gallinasActuales - cantidadDeGallinas;
            for (int i = 0;i < gallinasPorEliminar;i++)
            {
                if (gallinas.Count > 0)
                {
                    Gallina gallinasAEliminar = gallinas[gallinas.Count - 1];
                    gallinas.RemoveAt(gallinas.Count - 1);
                    Destroy(gallinasAEliminar.gameObject);
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

        Gallina nuevoGallina = Instantiate(gallinaPrefab, posicion, rotación, this.transform).GetComponent<Gallina>();
        gallinas.Add(nuevoGallina);
    }
}