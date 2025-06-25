using UnityEngine;

public static class GeneradorDeDirecciones
{
    const int numeroDeRayos = 10;

    public static Vector3[] GenerarDirecciones()
    {
        Vector3[] direcciones = new Vector3[numeroDeRayos];

        float incremento = Mathf.PI * 2 / numeroDeRayos;

        for (int i = 0; i < numeroDeRayos; i++)
        {
            float ángulo = incremento * i;

            float x = Mathf.Sin(ángulo);
            float y = 0;
            float z = Mathf.Cos(ángulo);
            direcciones[i] = new Vector3(x, y, z);
        }

        return direcciones;
    }


}
