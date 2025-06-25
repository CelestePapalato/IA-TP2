using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField]
    SlimeConfiguraci�n configuraci�n;

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
        Vector3 fuerza = fuerzaDeSeparacion * configuraci�n.separaci�nPeso +
                         fuerzaDeAlineacion * configuraci�n.alineaci�nPeso +
                         fuerzaDeCohesion * configuraci�n.cohesi�nPeso +
                         fuerzaObjetivo * configuraci�n.objetivoPeso;

        velocidad += fuerza * Time.deltaTime;
        float rapidezActual = velocidad.magnitude;
        Vector3 direcci�n = velocidad.normalized;
        rapidezActual = Mathf.Clamp(rapidezActual, 0, configuraci�n.rapidez);
        velocidad = direcci�n * rapidezActual;

        transform.position += velocidad * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocidad);
    }

    public void CalcularFuerzas()
    {
        Vector3 separaci�nSuma = Vector3.zero;
        Vector3 posicionesSuma = Vector3.zero;
        Vector3 alineacionSuma = Vector3.zero;
        int slimesVecinos = 0;

        for (int i = 0; i < colonia.slimes.Count; i++)
        {

            if (this != colonia.slimes[i])
            {
                Vector3 posici�nVecino = colonia.slimes[i].transform.position;
                float sqrDistanciaVecino = (transform.position - posici�nVecino).sqrMagnitude;

                posicionesSuma += posici�nVecino;
                alineacionSuma += colonia.slimes[i].transform.forward;

                if (sqrDistanciaVecino < configuraci�n.sqrDistanciaSeparaci�n)
                {
                    float escala = Mathf.Sqrt(sqrDistanciaVecino) / Mathf.Sqrt(configuraci�n.sqrDistanciaSeparaci�n);
                    escala = 1 - escala;
                    separaci�nSuma += -(posici�nVecino - transform.position).normalized / escala;

                    slimesVecinos++;
                }
            }
        } 

        if (slimesVecinos > 0)
        {
            fuerzaDeSeparacion = separaci�nSuma / slimesVecinos;
            fuerzaDeCohesion = (posicionesSuma / colonia.slimes.Count) - transform.position;
            fuerzaDeAlineacion = alineacionSuma / colonia.slimes.Count;
        }
        else
        {
            fuerzaDeSeparacion = Vector3.zero;
            fuerzaDeCohesion = Vector3.zero;
            fuerzaDeAlineacion = Vector3.zero;
        }

        fuerzaObjetivo = (transformObjetivo.position - transform.position).normalized * configuraci�n.rapidez;
    }

}
