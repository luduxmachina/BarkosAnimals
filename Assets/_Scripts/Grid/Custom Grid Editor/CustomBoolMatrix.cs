using UnityEngine;

[System.Serializable]
public class BoolRow
{
    public bool[] values;
}

[System.Serializable]
public class CustomBoolMatrix
{
    [Min(1)] public int rows;
    [Min(1)] public int columns;

    public BoolRow[] matrix;

    public CustomBoolMatrix(int rows, int columns)
    {
        this.rows = Mathf.Max(1, rows);
        this.columns = Mathf.Max(1, columns);
    }

    public CustomBoolMatrix()
    {
        rows = 1;
        columns = 1;
    }

    public bool SetValue(int row, int colum, bool value)
    {
        if(row < 0 || row >= rows || colum < 0 || colum >= columns)
            return false;
        
        matrix[row].values[colum] = value;
        return true;
    }

    public void DebugMatrix()
    {
        Debug.Log($"La matriz es de {rows} x {columns}");
            
        Debug.Log($"\t Matriz B:");
        for (int i = 0; i < rows; i++)
        {
            string str = "\t";
            for (int j = 0; j < columns; j++)
            {
                if (GetValue(i, j)) 
                {
                    str += "X";
                }
                else
                {
                    str += "O";
                }

                str += "\t";
            }

            Debug.Log(str);
        }
    }

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

    public bool IsFull() => AllAreInState(true);

    public bool IsEmpty() => AllAreInState(false);

    private bool AllAreInState(bool state)
    {
        bool result = true;   

        foreach (var row in matrix)
        {
            if (result != state)
                break;
            foreach (bool value in row.values)
            {
                result = value;
                if (result != state)
                    break;
            }
        }

        return result;
    }
    
    public void Rotate90()
    {
        EnsureSize();

        int newRows = columns;
        int newCols = rows;

        bool[,] rotated = new bool[newRows, newCols];

        // Llenar la matriz rotada
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                rotated[j, rows - 1 - i] = matrix[i].values[j];
            }
        }

        // Reasignar el tamaño
        rows = newRows;
        columns = newCols;

        // Reconstruir estructura matrix[][] según el nuevo tamaño
        matrix = new BoolRow[rows];
        for (int i = 0; i < rows; i++)
        {
            matrix[i] = new BoolRow();
            matrix[i].values = new bool[columns];

            for (int j = 0; j < columns; j++)
                matrix[i].values[j] = rotated[i, j];
        }
        
        EnsureSize();
    }


    public int GetRows() => matrix.Length;
    public int GetColums() => matrix[0].values.Length;
    public bool GetValue(int row, int colum) => matrix[row].values[colum];
}
