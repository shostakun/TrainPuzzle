using UnityEngine;

public class MakeStations : Initializer
{
    public GameObject[] stationPrefabs;
    public GameObject trainPrefab;
    public Transform trainContainer;

    public override void Initialize()
    {
        if (SaveData.inst.stationSettings.Count > 0)
        {
            LoadStations();
        }
        else
        {
            MakeNewStations();
        }
    }

    void LoadStations()
    {
        for (int i = 0; i < SaveData.inst.stationSettings.Count; i++)
        {
            StationSettings settings = SaveData.inst.stationSettings[i];
            foreach (GameObject prefab in stationPrefabs)
            {
                if (prefab.name == settings.prefabName)
                {
                    Track tile = Board.inst.GetTrack(settings.addr);
                    tile.AddInstance(prefab);
                    if (i == 0) MakeTrain(tile.transform.position, tile);
                    break;
                }
            }
        }
    }

    void MakeNewStations()
    {
        // Clear the existing trains.
        foreach (Transform child in trainContainer)
        {
            Destroy(child.gameObject);
        }

        int number = Board.inst.settings.stations;
        Vector3[] positions = new Vector3[number];

        // Choose random positions with adequate spacing.
        float minDistance = Board.inst.settings.minDistance;
        bool valid = false;
        while (!valid)
        {
            for (int i = 0; i < number; i++)
            {
                positions[i] = new Vector3(Random.Range(1, Board.inst.width - 1), 0, Random.Range(1, Board.inst.height - 1));
            }
            valid = true;
            for (int i = 0; i < number - 1; i++)
            {
                for (int j = i + 1; j < number; j++)
                {
                    if (Vector3.Distance(positions[i], positions[j]) < minDistance)
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

        // Place stations and trains.
        for (int i = 0; i < number; i++)
        {
            GameObject stationPrefab = stationPrefabs[Random.Range(0, stationPrefabs.Length)];
            Vector3 position = transform.position + positions[i];
            Track track = Board.inst.GetTrack(position);
            if (track == null)
            {
                Debug.LogError("No track at " + position);
                continue;
            }
            track.isStation = true;
            track.AddInstance(stationPrefab);
            if (i == 0)
            {
                MakeTrain(position, track);
            }
        }
    }

    void MakeTrain(Vector3 position, Track track)
    {
        position.y = trainContainer.position.y;
        GameObject train = Instantiate(trainPrefab, position, Quaternion.identity, trainContainer);
        train.name = trainPrefab.name;
        train.GetComponent<Move>().SetStartTrack(track);
    }
}
