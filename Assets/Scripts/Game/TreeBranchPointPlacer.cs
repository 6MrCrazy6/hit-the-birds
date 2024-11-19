using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TreeBranchPointPlacer : MonoBehaviour
    {
        public GameObject[] birdPrefabs;
        public Transform pointsParent;
        public Transform birdsParent;

        private List<Transform> branchPoints = new List<Transform>();
        private Dictionary<Transform, GameObject> placedBirds = new Dictionary<Transform, GameObject>();

        void Start()
        {
            foreach (Transform point in pointsParent)
            {
                branchPoints.Add(point);
            }

            PlaceBirdsOnPoints();
        }

        void PlaceBirdsOnPoints()
        {
            foreach (Transform point in branchPoints)
            {
                if (point.childCount == 0)
                {
                    List<int> availableIndexes = new List<int>();

                    for (int i = 0; i < birdPrefabs.Length; i++)
                    {
                        availableIndexes.Add(i);
                    }

                    // Исключение индексов соседей
                    foreach (Transform neighbor in GetNeighbors(point))
                    {
                        if (placedBirds.ContainsKey(neighbor))
                        {
                            GameObject neighborBird = placedBirds[neighbor];
                            int neighborIndex = System.Array.IndexOf(birdPrefabs, neighborBird);
                            if (neighborIndex >= 0)
                            {
                                availableIndexes.Remove(neighborIndex);
                            }
                        }
                    }

                    if (availableIndexes.Count == 0)
                    {
                        availableIndexes.Add(Random.Range(0, birdPrefabs.Length));
                    }

                    int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
                    GameObject selectedBirdPrefab = birdPrefabs[randomIndex];

                    GameObject newBird = Instantiate(selectedBirdPrefab, point.position, point.rotation, birdsParent);
                    BirdControll birdControl = newBird.GetComponent<BirdControll>();
                    if (birdControl != null)
                    {
                        birdControl.treePoint = point;
                    }

                    placedBirds[point] = selectedBirdPrefab;
                }
            }
        }

        List<Transform> GetNeighbors(Transform point)
        {
            List<Transform> neighbors = new List<Transform>();

            foreach (Transform otherPoint in branchPoints)
            {
                if (otherPoint != point && Vector3.Distance(otherPoint.position, point.position) < 600f) 
                {
                    neighbors.Add(otherPoint);
                }
            }

            return neighbors;
        }
    }
}
