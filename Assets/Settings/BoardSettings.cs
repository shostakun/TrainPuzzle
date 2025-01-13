using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardSettings", menuName = "Scriptable Objects/BoardSettings")]
public class BoardSettings : ScriptableObject
{
  [BoxGroup("Size")]
  public int width;
  [BoxGroup("Size")]
  public int height;
  [BoxGroup("Stations")]
  public int stations;
  [BoxGroup("Stations")]
  public float minDistance;
  [BoxGroup("Obstacles")]
  public int minObstacles;
  [BoxGroup("Obstacles")]
  public int maxObstacles;
}
