using System.Collections.Generic;
using UnityEngine;

public class SectorContext {

  [SerializeField]
  public Vector2 CurrentSector { get; private set; } = Vector2.zero;
  public List<Vector2> LoadedSectors { get; private set; } = new List<Vector2>();

  public List<Vector2> SetInitialPosition(Vector2 position) {
    CurrentSector = position;
    LoadedSectors.Add(new Vector2(-1, -1));
    LoadedSectors.Add(new Vector2(0, -1));
    LoadedSectors.Add(new Vector2(1, -1));
    LoadedSectors.Add(new Vector2(-1, 0));
    // LoadedSectors.Add(new Vector2(0, 0));
    LoadedSectors.Add(new Vector2(1, 0));
    LoadedSectors.Add(new Vector2(-1, 1));
    LoadedSectors.Add(new Vector2(0, 1));
    LoadedSectors.Add(new Vector2(1, 1));
    return new List<Vector2>(LoadedSectors);
  }

  public List<Vector2> MoveToLoadingZone(Direction direction) {
    var newSectors = new List<Vector2>();
    var newAffectedSectors = CalculateAffectedSectors(direction);
    foreach (var s in newAffectedSectors) {
      if (!LoadedSectors.Contains(s)) {
        newSectors.Add(s);
      }
    }
    LoadedSectors.AddRange(newSectors);
    return newSectors;
  }

  public List<Vector2> MoveToSector(Direction direction) {
    CurrentSector += Constants.LocationMap[direction];
    var unloadDirection = DirectionUtils.OppositeOf(direction);
    var toUnload = CalculateAffectedSectors(unloadDirection);
    return toUnload;
  }

  public List<Vector2> CalculateAffectedSectors(Direction direction) {
    var newSectors = new List<Vector2>();
    var cX = CurrentSector.x;
    var nY = CurrentSector.y;

    if (direction == Direction.NORTH || direction == Direction.SOUTH) {
      if (direction == Direction.NORTH) {
        nY += 2;
      }
      if (direction == Direction.SOUTH) {
        nY -= 2;
      }
      newSectors.Add(new Vector2(cX - 1, nY));
      newSectors.Add(new Vector2(cX, nY));
      newSectors.Add(new Vector2(cX + 1, nY));
    }

    if (direction == Direction.EAST || direction == Direction.WEST) {
      if (direction == Direction.EAST) {
        cX += 2;
      }
      if (direction == Direction.WEST) {
        cX -= 2;
      }
      newSectors.Add(new Vector2(cX, nY - 1));
      newSectors.Add(new Vector2(cX, nY));
      newSectors.Add(new Vector2(cX, nY + 1));
    }

    return newSectors;
  }
}
