using UnityEngine;

public static class GeneradorDeDirecciones
{
    const int numeroDeRayos = 300;

    public static Vector3[] GenerarDirecciones()
    {
        Vector3[] direcciones = new Vector3[numeroDeRayos];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numeroDeRayos; i++)
        {
            float t = (float)i / numeroDeRayos;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            direcciones[i] = new Vector3(x, y, z);
        }

        return direcciones;
    }


}
