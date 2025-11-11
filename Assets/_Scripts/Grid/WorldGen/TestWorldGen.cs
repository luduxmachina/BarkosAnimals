using UnityEngine;

public class TestWorldGen : MonoBehaviour
{
    [SerializeField]
    private B_WorldPlaceableObjectsSO database;

    private void Start()
    {
        foreach(var obj in database.PlaceableObjectData)
        {
            Debug.Log($"La matriz [{obj.Name}] es de {obj.Size.x} x {obj.Size.y}");
            
            Debug.Log($"\t Matriz B:");
            for (int i = 0; i < obj.OcupiedSpace.GetRows(); i++)
            {
                string str = "\t";
                for (int j = 0; j < obj.OcupiedSpace.GetColums(); j++)
                {
                    if (obj.OcupiedSpace.GetValue(i, j)) 
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
