using UnityEngine;

public enum LoadingState {
  NONE,
  PRELOADING,
  LOADING,
  LEAVING
}

public enum LoadingEventType {
  LOAD,
  UNLOAD,
  CHANGE_SECTOR
}

public class LoadingEvent {
  public Direction Direction { get; private set; }
  public LoadingEventType LoadType { get; private set; }

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
    if (loadingState == LoadingState.PRELOADING) {
      SendMessageUpwards("LoadingEvent", new LoadingEvent(location, LoadingEventType.UNLOAD));
    }
  }

}
