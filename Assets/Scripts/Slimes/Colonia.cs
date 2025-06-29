using System.Collections.Generic;
using UnityEngine;

public class Colonia: MonoBehaviour
{
    public static Colonia Instancia { get; private set; }

    [Header("Configuración del Cardumen")]
    [SerializeField]
    private int cantidadDeSlimes = 50;
    [SerializeField]
    public float tamañoDelEspacio = 50f;
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
    }

    private void Start()
    {
        AjustarCantidadDeSlimes();
        InvokeRepeating(nameof(AjustarCantidadDeSlimes), 2f, 2f);
    }

    private void AjustarCantidadDeSlimes()
    {
        slimes.RemoveAll(slime => slime == null);

        int slimesActuales = slimes.Count;

        if (slimesActuales < cantidadDeSlimes)
        {
            int slimesPorInstanciar = cantidadDeSlimes - slimesActuales;
            for (int i = 0;i < slimesPorInstanciar;i++)
            {
                InstanciarSlime();
            }
        }
        else if (slimesActuales > cantidadDeSlimes)
        {
            int slimesPorEliminar = slimesActuales - cantidadDeSlimes;
            for (int i = 0;i < slimesPorEliminar;i++)
            {
                if (slimes.Count > 0)
                {
                    Slime slimeAEliminar = slimes[slimes.Count - 1];
                    slimes.RemoveAt(slimes.Count - 1);
                    Destroy(slimeAEliminar.gameObject);
                }
            }
        }

    }

    public void InstanciarSlime()
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

        Slime nuevoSlime = Instantiate(slimePrefab, posicion, rotación, this.transform).GetComponent<Slime>();
        slimes.Add(nuevoSlime);
    }
}