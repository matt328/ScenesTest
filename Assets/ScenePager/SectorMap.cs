using System.Collections.Generic;
using UnityEngine;

public class SectorDictionary : Dictionary<Vector2, string> { }

[CreateAssetMenu(fileName = "SectorMap", menuName = "ScriptableObjects/Sector Map", order = 1)]
public class SectorMap : ScriptableObject {

  [SerializeField]
  private List<Sector> sectors;

  private SectorDictionary sectorDictionary;

  private void Awake() {
    sectorDictionary = new SectorDictionary();
    foreach (var s in sectors) {
      sectorDictionary.Add(s.Position, s.SceneName);
    }
  }

  public string GetSceneNameForSector(Vector2 vector) {
    if (sectorDictionary == null) {
      Awake();
    }
    if (!sectorDictionary.ContainsKey(vector)) {
      return null;
    }
    return sectorDictionary[vector];
  }

}
