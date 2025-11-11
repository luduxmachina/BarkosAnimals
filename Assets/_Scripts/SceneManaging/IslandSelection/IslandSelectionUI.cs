using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandSelectionUI : MonoBehaviour
{
    [SerializeField]
    IndividualIslandSelectionUI[] individualIslandSelectionInScene;
    [SerializeField]
    AllObjectTypesSO allObjectTypesSO;
    private void Start()
    {
        foreach (var islandUI in individualIslandSelectionInScene)
        {
            //coger tres indices aleatorios?
            //ver maximo numero que puedo coger si es generado o no
            //darle a cada indivudal lo que le corresponde, y pasarle el indice
            Archipelago archipelago = GameFlowManager.instance.generatedIsland ? GameFlowManager.instance.defaultLevel.archipelagos[0]
                : GameFlowManager.instance.currentLevel.archipelagos[GameFlowManager.instance.currentArchipelago];

            int maxIslands = archipelago.numberOfIslands;
            List<int> usedIndexes = new(); 

            
            for(int i = 0; i < 3; i++)
            {
                int randomIndex = Random.Range(0, maxIslands);

                while (usedIndexes.Contains(randomIndex) && !(randomIndex>=maxIslands))
                {
                    randomIndex++;
                }
                usedIndexes.Add(randomIndex);

                individualIslandSelectionInScene[i].InitUI(archipelago.islands[randomIndex], randomIndex, allObjectTypesSO, this);

            }
            individualIslandSelectionInScene[0].SelectIsland();
        }
    }
    public void UpdateUI()
    {
        foreach (var islandUI in individualIslandSelectionInScene)
        {
            islandUI.UpdateUI();
        }
    }
}
