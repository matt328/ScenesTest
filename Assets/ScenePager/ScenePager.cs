using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExtensionMethods;
/* TODO:
  1. Clean up events thrown when the colliders move from one sector to the next
  2. Implement origin shifting when changing sectors
  3. Build out huge world for testing
  4. Make collision detectors' transforms based on terrain size.
  5. Pack the whole thing into a prefab
*/

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

    if (e.LoadType == LoadingEventType.LOAD) {
      foreach (var sceneCoords in affectedSectors) {
        if (sectorMap.GetSceneNameForSector(sceneCoords) != null) {
          Debug.LogFormat("Loading SceneCoord: {0}", sceneCoords);
          Debug.LogFormat("LoadedSectors: {0}", sectorContext.LoadedSectors.Print());
          if (!sectorContext.LoadedSectors.Contains(sceneCoords)) {
            StartCoroutine(LoadScene(sceneCoords));
          }
        } else {
          Debug.LogWarningFormat("No scene exists for coordinates {0}", sceneCoords.ToString());
        }
      }
    } else if (e.LoadType == LoadingEventType.UNLOAD) {
      var toUnload = sectorContext.AddAdditionalSectorsForUnload(affectedSectors, e.Direction);
      foreach (var sceneCoords in toUnload) {
        if (sectorMap.GetSceneNameForSector(sceneCoords) != null) {
          if (sectorContext.LoadedSectors.Contains(sceneCoords)) {
            StartCoroutine(UnloadScene(sceneCoords));
          }
        } else {
          Debug.LogWarningFormat("No scene exists for coordinates {0}", sceneCoords.ToString());
        }
      }
    }
  }

  private void ChangeSector(Direction direction) {
    // Inform SectorContext we've moved
    sectorContext.MoveToSector(direction);

    // Move all of our GameObjects that should come with us to the next scene
    // in order to preserve them should the initial scene be unloaded.
    var newSceneName = sectorMap.GetSceneNameForSector(sectorContext.CurrentSector);
    SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(newSceneName));
    SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(newSceneName));

    // Recenter the collision detectors to the new scene's location
    var offsetVector = new Vector3(Constants.LocationMap[direction].x, 0, Constants.LocationMap[direction].y) * terrainSize;
    transform.position += offsetVector;
  }

  private IEnumerator UnloadScene(Vector2 sceneCoords) {
    var sceneName = sectorMap.GetSceneNameForSector(sceneCoords);
    AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
    Debug.LogFormat("Removing Sector: {0}", sceneCoords);
    sectorContext.LoadedSectors.Remove(sceneCoords);
    Debug.LogFormat("LoadedSectors: {0}", sectorContext.LoadedSectors.Print());
    yield return null;
  }

  private IEnumerator LoadScene(Vector2 sceneCoords) {
    Debug.LogFormat("Loading Scene Coords: {0}", sceneCoords);
    var sceneName = sectorMap.GetSceneNameForSector(sceneCoords);
    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    sectorContext.LoadedSectors.Add(sceneCoords);
    yield return null;
  }
}
