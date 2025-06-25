using UnityEngine;
using UnityEngine.UIElements;

public class Slime : MonoBehaviour
{
    [SerializeField]
    SlimeConfiguraci�n config;

    [SerializeField]
    private Vector3 fuerzaDeSeparacion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaDeAlineacion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaDeCohesion = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaObjetivo = Vector3.zero;
    [SerializeField]
    private Vector3 fuerzaDeEvasi�n = Vector3.zero;

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
        DetectarContactoConObst�culos();
        ActualizarTiempoDeVida();
        CalcularFuerzas();
        CalcularFuerzaDeEvasi�n();
        Avanzar();
    }

    private void DetectarContactoConObst�culos()
    {
        if (Physics.OverlapSphere(transform.position, config.radioDeDa�o).Length > 0)
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
        Vector3 fuerza = fuerzaDeSeparacion * config.separaci�nPeso +
                         fuerzaDeAlineacion * config.alineaci�nPeso +
                         fuerzaDeCohesion * config.cohesi�nPeso +
                         fuerzaObjetivo * config.objetivoPeso +
                         fuerzaDeEvasi�n * config.evasi�nPeso;
        fuerza.y = 0f; // Mantener el slime en el plano horizontal
        velocidad += fuerza * Time.deltaTime;
        float rapidezActual = velocidad.magnitude;
        Vector3 direcci�n = velocidad.normalized;
        rapidezActual = Mathf.Clamp(rapidezActual, 0, config.rapidez);
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

                if (sqrDistanciaVecino < config.sqrDistanciaSeparaci�n)
                {
                    float escala = Mathf.Sqrt(sqrDistanciaVecino) / Mathf.Sqrt(config.sqrDistanciaSeparaci�n);
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

        fuerzaObjetivo = (transformObjetivo.position - transform.position).normalized * config.rapidez;
    }

    // Detecci�n de colisiones

    public void CalcularFuerzaDeEvasi�n()
    {
        if(!Est�EnRiesgoDeColisi�n())
        {
            fuerzaDeEvasi�n = Vector3.zero;
            return;
        }

        fuerzaDeEvasi�n = ObtenerDirecci�nLibre();
    }

    private bool Est�EnRiesgoDeColisi�n()
    {
        RaycastHit hit;

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        return Physics.SphereCast(position, 
            config.radioDeColisi�n, 
            forward, out hit, 
            config.distanciaEvasi�nDeColisi�n, 
            config.maskObst�culos);
    }

    private Vector3 ObtenerDirecci�nLibre()
    {
        Vector3[] rayDirections = GeneradorDeDirecciones.GenerarDirecciones();

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        fuerzaDeEvasi�n = forward;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            if (!Physics.SphereCast(ray, config.radioDeColisi�n, 
                config.distanciaEvasi�nDeColisi�n, 
                config.maskObst�culos))
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
            Vector3 destino = origen + dir * config.radioDeDa�o;
            Gizmos.DrawLine(origen, destino);
        }
    }
}
