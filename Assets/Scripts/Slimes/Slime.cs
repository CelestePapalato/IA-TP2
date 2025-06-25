using UnityEngine;
using UnityEngine.UIElements;

public class Slime : MonoBehaviour
{
    [SerializeField]
    SlimeConfiguración config;

    [SerializeField]
    private Vector3 fuerzaDeSeparacion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaDeAlineacion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaDeCohesion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaObjetivo = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaDeEvasión = Vector3.zero;

    private Vector3 velocidad = Vector3.zero;

    private Transform transformObjetivo;

    private Colonia colonia;

    private float tiempoVivo = 0f;

    private void Start()
    {
        colonia = Colonia.Instancia;
        transformObjetivo = colonia.Objetivo;
    }

    private void Update()
    {
        DetectarContactoConObstáculos();
        ActualizarTiempoDeVida();
        CalcularFuerzas();
        CalcularFuerzaDeEvasión();
        Avanzar();
    }

    private void DetectarContactoConObstáculos()
    {
        if (Physics.OverlapSphere(transform.position, config.radioDeDaño).Length > 0)
        {
            Muerte();
        }
    }

    private void ActualizarTiempoDeVida()
    {
        tiempoVivo += Time.deltaTime;
        if (tiempoVivo > config.tiempoDeVida)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        colonia.slimes.Remove(this);
        Destroy(gameObject);
        colonia.InstanciarSlime();
    }

    public void Avanzar()
    {
        Vector3 fuerza = fuerzaDeSeparacion * config.separaciónPeso +
                         fuerzaDeAlineacion * config.alineaciónPeso +
                         fuerzaDeCohesion * config.cohesiónPeso +
                         fuerzaObjetivo * config.objetivoPeso +
                         fuerzaDeEvasión * config.evasiónPeso;
        fuerza.y = 0f; // Mantener el slime en el plano horizontal
        velocidad += fuerza * Time.deltaTime;
        float rapidezActual = velocidad.magnitude;
        Vector3 dirección = velocidad.normalized;
        rapidezActual = Mathf.Clamp(rapidezActual, 0, config.rapidez);
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

                if (sqrDistanciaVecino < config.sqrDistanciaSeparación)
                {
                    float escala = Mathf.Sqrt(sqrDistanciaVecino) / Mathf.Sqrt(config.sqrDistanciaSeparación);
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

        fuerzaObjetivo = (transformObjetivo.position - transform.position).normalized * config.rapidez;
    }

    // Detección de colisiones

    public void CalcularFuerzaDeEvasión()
    {
        if(!EstáEnRiesgoDeColisión())
        {
            fuerzaDeEvasión = Vector3.zero;
            return;
        }

        fuerzaDeEvasión = ObtenerDirecciónLibre();
    }

    private bool EstáEnRiesgoDeColisión()
    {
        RaycastHit hit;

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        return Physics.SphereCast(position, 
            config.radioDeColisión, 
            forward, out hit, 
            config.distanciaEvasiónDeColisión, 
            config.maskObstáculos);
    }

    private Vector3 ObtenerDirecciónLibre()
    {
        Vector3[] rayDirections = GeneradorDeDirecciones.GenerarDirecciones();

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        fuerzaDeEvasión = forward;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            if (!Physics.SphereCast(ray, config.radioDeColisión, 
                config.distanciaEvasiónDeColisión, 
                config.maskObstáculos))
            {
                return dir;
            }
        }

        return forward;
    }

    void OnDrawGizmos()
    {
        Vector3[] direcciones = GeneradorDeDirecciones.GenerarDirecciones();
        // Asegura que solo se genere una vez en editor (opcional)
        if (direcciones == null || direcciones.Length == 0)
            direcciones = GeneradorDeDirecciones.GenerarDirecciones();

        Gizmos.color = Color.yellow;

        foreach (Vector3 dir in direcciones)
        {
            Vector3 origen = transform.position;
            Vector3 destino = origen + dir * config.radioDeDaño;
            Gizmos.DrawLine(origen, destino);
        }
    }
}
