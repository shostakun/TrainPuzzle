using UnityEngine;

public class MakeStations : MonoBehaviour
{
    public int number = 2;
    public GameObject[] stationPrefabs;
    public GameObject trainPrefab;
    public Transform trainContainer;
    private Board board;

    void Start()
    {
        board = GetComponentInParent<Board>();
        Vector3[] positions = new Vector3[number];
        bool valid = false;
        while (!valid)
        {
            for (int i = 0; i < number; i++)
            {
                positions[i] = new Vector3(Random.Range(1, board.width - 1), Random.Range(1, board.height - 1), 0);
            }
            valid = true;
            for (int i = 0; i < number - 1; i++)
            {
                for (int j = i + 1; j < number; j++)
                {
                    if (Vector3.Distance(positions[i], positions[j]) < 3)
                    {
                        valid = false;
                        break;
                    }
                }
                if (!valid)
                {
                    break;
                }
            }
        }
        for (int i = 0; i < number; i++)
        {
            GameObject stationPrefab = stationPrefabs[Random.Range(0, stationPrefabs.Length)];
            Vector3 position = transform.position + positions[i];
            GameObject station = Instantiate(stationPrefab, position, Quaternion.identity, transform);
            Tile tile = station.GetComponent<Tile>();
            board.SetTile(tile);
            if (i == 0)
            {
                position.z = trainContainer.position.z;
                GameObject train = Instantiate(trainPrefab, position, Quaternion.identity, trainContainer);
                train.GetComponent<Move>().SetStartTile(tile);
            }
        }
    }
}
