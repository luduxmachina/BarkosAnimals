using UnityEngine;

[System.Serializable]
public class BoolRow
{
    public bool[] values;
}

[System.Serializable]
public class CustomBoolMatrix
{
    [Min(1)] public int rows = 2;
    [Min(1)] public int columns = 2;

    public BoolRow[] matrix;

    public void EnsureSize()
    {
        if (matrix == null || matrix[0].values  == null || matrix.Length != rows || matrix[0].values.Length != columns)
        {
            System.Array.Resize(ref matrix, rows);
        }

        for (int i = 0; i < rows; i++)
        {
            if (matrix[i] == null)
                matrix[i] = new BoolRow();

            if (matrix[i].values == null || matrix[i].values.Length != columns)
                System.Array.Resize(ref matrix[i].values, columns);
        }
    }

    public void Clear()
    {
        EnsureSize();
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                matrix[i].values[j] = false;
    }

    public void Invert()
    {
        EnsureSize();
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                matrix[i].values[j] = !matrix[i].values[j];
    }
}
