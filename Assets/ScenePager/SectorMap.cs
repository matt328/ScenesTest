using System.Collections.Generic;
using UnityEngine;

public class SectorDictionary : Dictionary<Vector2, Sector> { }

[CreateAssetMenu(fileName = "SectorMap", menuName = "ScriptableObjects/Sector Map", order = 1)]
public class SectorMap : ScriptableObject {

  [SerializeField]
  private SectorDictionary sectors = new SectorDictionary() {
    {new Vector2(-1, 2), new Sector(-1, 2, "Scene-1_2")},
    {new Vector2(0, 2), new Sector(0, 2, "Scene3")},
    {new Vector2(1, 2), new Sector(1, 2, "Scene1_2")},
    {new Vector2(-1, 1), new Sector(-1, 1, "Scene-1_1")},
    {new Vector2(0, 1), new Sector(0, 1, "Scene2")},
    {new Vector2(1, 1), new Sector(1, 1, "Scene1_1")},
    {new Vector2(-1, 0), new Sector(-1, 0, "Scene-1_0")},
    {new Vector2(0, 0), new Sector(0, 0, "Scene1")},
    {new Vector2(1, 0), new Sector(1, 0, "Scene1_0")},
    {new Vector2(-1, -1), new Sector(-1, -1, "Scene-1_-1")},
    {new Vector2(0, -1), new Sector(0, -1, "Scene0_-1")},
    {new Vector2(1, -1), new Sector(1, -1, "Scene1_-1")},
    {new Vector2(-2, -1), new Sector(-1, -1, "Scene-2_-1")},
    {new Vector2(-2, 0), new Sector(0, -1, "Scene-2_0")},
    {new Vector2(-2, 1), new Sector(1, -1, "Scene-2_1")}
  };

  public string GetSceneNameForSector(Vector2 vector) {
    if (!sectors.ContainsKey(vector)) {
      return null;
    }
    return sectors[vector].SceneName;
  }

}
