using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField]
    SlimeConfiguración configuración;

    [SerializeField]
    private Vector3 fuerzaDeSeparacion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaDeAlineacion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaDeCohesion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaObjetivo = Vector3.zero;

    private Vector3 velocidad = Vector3.zero;

    private Transform transformObjetivo;

    private Colonia colonia;

    private void Start()
    {
        colonia = Colonia.Instancia;
        transformObjetivo = colonia.Objetivo;
    }

    private void Update()
    {
        CalcularFuerzas();
        Avanzar();
    }

    public void Avanzar()
    {
        Vector3 fuerza = fuerzaDeSeparacion * configuración.separaciónPeso +
                         fuerzaDeAlineacion * configuración.alineaciónPeso +
                         fuerzaDeCohesion * configuración.cohesiónPeso +
                         fuerzaObjetivo * configuración.objetivoPeso;

        velocidad += fuerza * Time.deltaTime;
        float rapidezActual = velocidad.magnitude;
        Vector3 dirección = velocidad.normalized;
        rapidezActual = Mathf.Clamp(rapidezActual, 0, configuración.rapidez);
        velocidad = dirección * rapidezActual;

        transform.position += velocidad * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocidad);
    }

    public void CalcularFuerzas()
    {
        Vector3 separaciónSuma = Vector3.zero;
        Vector3 posicionesSuma = Vector3.zero;
        Vector3 alineacionSuma = Vector3.zero;
        int slimesVecinos = 0;

        for (int i = 0; i < colonia.slimes.Count; i++)
        {

            if (this != colonia.slimes[i])
            {
                Vector3 posiciónVecino = colonia.slimes[i].transform.position;
                float sqrDistanciaVecino = (transform.position - posiciónVecino).sqrMagnitude;

                posicionesSuma += posiciónVecino;
                alineacionSuma += colonia.slimes[i].transform.forward;

                if (sqrDistanciaVecino < configuración.sqrDistanciaSeparación)
                {
                    float escala = Mathf.Sqrt(sqrDistanciaVecino) / Mathf.Sqrt(configuración.sqrDistanciaSeparación);
                    escala = 1 - escala;
                    separaciónSuma += -(posiciónVecino - transform.position).normalized / escala;

                    slimesVecinos++;
                }
            }
        } 

        if (slimesVecinos > 0)
        {
            fuerzaDeSeparacion = separaciónSuma / slimesVecinos;
            fuerzaDeCohesion = (posicionesSuma / colonia.slimes.Count) - transform.position;
            fuerzaDeAlineacion = alineacionSuma / colonia.slimes.Count;
        }
        else
        {
            fuerzaDeSeparacion = Vector3.zero;
            fuerzaDeCohesion = Vector3.zero;
            fuerzaDeAlineacion = Vector3.zero;
        }

        fuerzaObjetivo = (transformObjetivo.position - transform.position).normalized * configuración.rapidez;
    }

}
