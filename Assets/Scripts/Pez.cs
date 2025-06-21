using UnityEngine;

public class Pez : MonoBehaviour
{
    [SerializeField]
    PezConfiguración configuración;

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
        int pecesVecinos = 0;

        for (int i = 0; i < cardumen.peces.Count; i++)
        {

            if (this != cardumen.peces[i])
            {
                Vector3 posiciónPezVecino = cardumen.peces[i].transform.position;
                float sqrDistanciaPezVecino = (transform.position - posiciónPezVecino).sqrMagnitude;

                posicionesSuma += posiciónPezVecino;
                alineacionSuma += cardumen.peces[i].transform.forward;

                if (sqrDistanciaPezVecino < configuración.sqrDistanciaSeparación)
                {
                    float escala = Mathf.Sqrt(sqrDistanciaPezVecino) / Mathf.Sqrt(configuración.sqrDistanciaSeparación);
                    escala = 1 - escala;
                    separaciónSuma += -(posiciónPezVecino - transform.position).normalized / escala;

                    pecesVecinos++;
                }
            }
        } 

        if (pecesVecinos > 0)
        {
            fuerzaDeSeparacion = separaciónSuma / pecesVecinos;
            fuerzaDeCohesion = (posicionesSuma / cardumen.peces.Count) - transform.position;
            fuerzaDeAlineacion = alineacionSuma / cardumen.peces.Count;
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
