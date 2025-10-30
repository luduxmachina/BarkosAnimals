using UnityEngine;

public class TestWorldGen : MonoBehaviour
{
    [SerializeField]
    private WorldPlaceableObjectsSO database;

    private void Start()
    {
        foreach(var obj in database.worldObjectData)
        {
            Debug.Log($"La matriz [{obj.Name}] es de {obj.B_OcupiedSpace.GetRows()} x {obj.B_OcupiedSpace.GetColums()}");
            
            Debug.Log($"\t Matriz B:");
            for (int i = 0; i < obj.B_OcupiedSpace.GetRows(); i++)
            {
                string str = "\t";
                for (int j = 0; j < obj.B_OcupiedSpace.GetColums(); j++)
                {
                    if (obj.B_OcupiedSpace.GetValue(i, j)) 
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

            Debug.Log($"\t Matriz C:");
            for (int i = 0; i < obj.C_OcupiedSpace.GetRows(); i++)
            {
                string str = "\t";
                for (int j = 0; j < obj.C_OcupiedSpace.GetColums(); j++)
                {
                    if (obj.C_OcupiedSpace.GetValue(i, j))
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
    }
}
