using UnityEngine;

public static class Randomizer
{
    private readonly static Color[] _colors = new Color[]
    {
        Color.blue,
        Color.cyan,
        Color.green,
        Color.grey,
        Color.magenta,
        Color.red,
        Color.white,
        Color.yellow
    };

    public static Vector3 GetPoint(Transform sourceArea)
    {
        Vector3 sourceAreaCenter = sourceArea.position;
        Vector3 sourceAreaSize = sourceArea.lossyScale;

        float half = 0.5f;

        float minX = sourceAreaCenter.x - sourceAreaSize.x * half;
        float maxX = sourceAreaCenter.x + sourceAreaSize.x * half;
        float minY = sourceAreaCenter.y - sourceAreaSize.y * half;
        float maxY = sourceAreaCenter.y + sourceAreaSize.y * half;
        float minZ = sourceAreaCenter.z - sourceAreaSize.z * half;
        float maxZ = sourceAreaCenter.z + sourceAreaSize.z * half;

        return new Vector3(Random.Range(minX, maxX),
                           Random.Range(minY, maxY),
                           Random.Range(minZ, maxZ));
    }

    public static Color GetColor()
    {
        return _colors[Random.Range(0, _colors.Length)];
    }
}
