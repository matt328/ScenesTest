using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePager : MonoBehaviour {

  [SerializeField]
  private SectorMap sectorMap = null;

  [SerializeField]
  private GameObject player = null;

  [SerializeField]
  private float terrainSize = 100f;

  private SectorContext sectorContext = new SectorContext();

  private void Start() {
    var sectorCoords = sectorContext.SetInitialPosition(Vector2.zero);
    foreach (var sceneCoords in sectorCoords) {
      if (sectorMap.GetSceneNameForSector(sceneCoords) != null) {
        StartCoroutine(LoadScene(sceneCoords));
      } else {
        Debug.LogWarningFormat("No scene exists for coordinates {0}", sceneCoords.ToString());
      }
    }
  }

  public void LoadingEvent(LoadingEvent e) {
    Debug.LogFormat("Loading Event: {0}", e);
    if (e.LoadType == LoadingEventType.CHANGE_SECTOR) {
      ChangeSector(e.Direction);
      return;
    }
    var affectedSectors = sectorContext.CalculateAffectedSectors(e.Direction);
    foreach (var sceneCoords in affectedSectors) {
      if (sectorMap.GetSceneNameForSector(sceneCoords) != null) {
        if (e.LoadType == LoadingEventType.LOAD) {
          if (!sectorContext.LoadedSectors.Contains(sceneCoords)) {
            StartCoroutine(LoadScene(sceneCoords));
          }
        } else {
          if (sectorContext.LoadedSectors.Contains(sceneCoords)) {
            StartCoroutine(UnloadScene(sceneCoords));
          }
        }
      } else {
        Debug.LogWarningFormat("No scene exists for coordinates {0}", sceneCoords.ToString());
      }
    }
  }

  private void ChangeSector(Direction direction) {
    sectorContext.MoveToSector(direction);
    var newSceneName = sectorMap.GetSceneNameForSector(sectorContext.CurrentSector);
    SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(newSceneName));
    SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(newSceneName));
    ShiftColliders(direction);
  }

  private void ShiftColliders(Direction dir) {
    var offsetVector = new Vector3(Constants.LocationMap[dir].x, 0, Constants.LocationMap[dir].y) * terrainSize;
    Debug.LogFormat("Shifting Colliders by {0}", offsetVector.ToString());
    transform.position += offsetVector;
  }

  private IEnumerator UnloadScene(Vector2 sceneCoords) {
    var sceneName = sectorMap.GetSceneNameForSector(sceneCoords);
    AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
    sectorContext.LoadedSectors.Remove(sceneCoords);
    yield return null;
  }

  private IEnumerator LoadScene(Vector2 sceneCoords) {
    var sceneName = sectorMap.GetSceneNameForSector(sceneCoords);
    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    sectorContext.LoadedSectors.Add(sceneCoords);
    yield return null;
  }
}
