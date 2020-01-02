using UnityEngine;

[System.Serializable]
public class Sector {
  [SerializeField]
  public Vector2 Position { get; private set; }
  [SerializeField]
  public string SceneName { get; private set; }

  public Sector(int x, int z, string sceneName) {
    Position = new Vector2(x, z);
    SceneName = sceneName;
  }
}
