using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExtensionMethods;
using System.Collections.Generic;
/* TODO:
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
  private Terrain terrain;

  [SerializeField]
  private bool debugCollisions = false;

  private SectorContext sectorContext = new SectorContext();

  private void Start() {
    if (!debugCollisions) {
      DisableVisualCollisionDetectors();
    }
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
    var offsetVector = new Vector3(Constants.LocationMap[direction].x, 0, Constants.LocationMap[direction].y) * terrain.terrainData.size.x;
    transform.position += offsetVector;

    // Origin Shifting
    for (int z = 0; z < SceneManager.sceneCount; z++) {
      foreach (var gameObject in SceneManager.GetSceneAt(z).GetRootGameObjects()) {
        gameObject.transform.position -= offsetVector;
      }
    }
  }

  private IEnumerator UnloadScene(Vector2 sceneCoords) {
    var sceneName = sectorMap.GetSceneNameForSector(sceneCoords);
    AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
    sectorContext.LoadedSectors.Remove(sceneCoords);
    yield return null;
  }

  private IEnumerator LoadScene(Vector2 sceneCoords) {
    var sceneName = sectorMap.GetSceneNameForSector(sceneCoords);
    AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

    while (!op.isDone) yield return null;

    sectorContext.LoadedSectors.Add(sceneCoords);
    var offsetVector = new Vector3(sectorContext.CurrentSector.x, 0f, sectorContext.CurrentSector.y) * terrain.terrainData.size.x;
    Debug.LogFormat("OffsetVector: {0}", offsetVector);
    foreach (var gameObject in SceneManager.GetSceneByName(sceneName).GetRootGameObjects()) {
      Debug.LogFormat("Moving objects in scene {0}", sceneName);
      gameObject.transform.position -= offsetVector;
    }

    yield return null;
  }

  private void DisableVisualCollisionDetectors() {
    var collisionAreas = gameObject.FindComponentsInChildrenWithTag<Component>("LoadingZone");
    foreach (var c in collisionAreas) {
      c.GetComponent<MeshRenderer>().enabled = false;
      c.transform.localScale = new Vector3(c.transform.localScale.x, 600, c.transform.localScale.z);
      c.transform.position = new Vector3(c.transform.position.x, 300, c.transform.position.z);
    }
  }
}
