using UnityEngine;

public enum LoadingEventType {
  LOAD,
  UNLOAD,
  CHANGE_SECTOR
}

public class LoadingEvent {
  public Direction Direction { get; }
  public LoadingEventType LoadType { get; }

  public LoadingEvent(Direction direction, LoadingEventType loadType) {
    Direction = direction;
    LoadType = loadType;
  }

  override public string ToString() {
    return string.Format("Direction: {0}, LoadType: {1}", Direction, LoadType);
  }
}

public class LoadingZone : MonoBehaviour {

  [SerializeField]
  private Direction location = Direction.NONE;

  public void CollisionEntered(LoadingState loadingState) {
    if (loadingState == LoadingState.LOADING) {
      SendMessageUpwards("LoadingEvent", new LoadingEvent(location, LoadingEventType.LOAD));
    } else if (loadingState == LoadingState.LEAVING) {
      SendMessageUpwards("LoadingEvent", new LoadingEvent(location, LoadingEventType.CHANGE_SECTOR));
    }
  }

  public void CollisionExited(LoadingState loadingState) {
    Debug.LogFormat("CollisionExited: {0}", loadingState);
    if (loadingState == LoadingState.PRELOADING) {
      SendMessageUpwards("LoadingEvent", new LoadingEvent(location, LoadingEventType.UNLOAD));
    }
  }

}
