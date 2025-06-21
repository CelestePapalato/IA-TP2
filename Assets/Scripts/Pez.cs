using UnityEngine;

public class Pez : MonoBehaviour
{
    [SerializeField]
    PezConfiguraci�n configuraci�n;

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

    private Cardumen cardumen;

    private void Start()
    {
        cardumen = Cardumen.Instancia;
        transformObjetivo = cardumen.Objetivo;
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
        int pecesVecinos = 0;

        for (int i = 0; i < cardumen.peces.Count; i++)
        {

            if (this != cardumen.peces[i])
            {
                Vector3 posici�nPezVecino = cardumen.peces[i].transform.position;
                float sqrDistanciaPezVecino = (transform.position - posici�nPezVecino).sqrMagnitude;

                posicionesSuma += posici�nPezVecino;
                alineacionSuma += cardumen.peces[i].transform.forward;

                if (sqrDistanciaPezVecino < configuraci�n.sqrDistanciaSeparaci�n)
                {
                    float escala = Mathf.Sqrt(sqrDistanciaPezVecino) / Mathf.Sqrt(configuraci�n.sqrDistanciaSeparaci�n);
                    escala = 1 - escala;
                    separaci�nSuma += -(posici�nPezVecino - transform.position).normalized / escala;

                    pecesVecinos++;
                }
            }
        } 

        if (pecesVecinos > 0)
        {
            fuerzaDeSeparacion = separaci�nSuma / pecesVecinos;
            fuerzaDeCohesion = (posicionesSuma / cardumen.peces.Count) - transform.position;
            fuerzaDeAlineacion = alineacionSuma / cardumen.peces.Count;
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
