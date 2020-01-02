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

  public void MoveToSector(Direction direction) {
    CurrentSector += Constants.LocationMap[direction];
  }

  public List<Vector2> AddAdditionalSectorsForUnload(List<Vector2> toUnload, Direction direction) {
    var returnValue = new List<Vector2>(toUnload);
    if (direction == Direction.NORTH || direction == Direction.SOUTH) {
      var highestX = CalculateHighestX(returnValue);
      var lowestX = CalculateLowestX(returnValue);
      var y = returnValue[0].y;
      returnValue.Add(new Vector2(lowestX - 1, y));
      returnValue.Add(new Vector2(highestX + 1, y));
    }
    if (direction == Direction.EAST || direction == Direction.WEST) {
      var highestY = CalculateHighestY(returnValue);
      var lowestY = CalculateLowestY(returnValue);
      var x = returnValue[0].x;
      returnValue.Add(new Vector2(x, lowestY - 1));
      returnValue.Add(new Vector2(x, highestY + 1));
    }
    return returnValue;
  }

  public List<Vector2> CalculateAffectedSectors(Direction direction) {
    var newSectors = new List<Vector2>();
    var currentX = CurrentSector.x;
    var currentY = CurrentSector.y;

    if (direction == Direction.NORTH || direction == Direction.SOUTH) {
      if (direction == Direction.NORTH) {
        currentY += 2;
      }
      if (direction == Direction.SOUTH) {
        currentY -= 2;
      }
      newSectors.Add(new Vector2(currentX - 1, currentY));
      newSectors.Add(new Vector2(currentX, currentY));
      newSectors.Add(new Vector2(currentX + 1, currentY));
    }

    if (direction == Direction.EAST || direction == Direction.WEST) {
      if (direction == Direction.EAST) {
        currentX += 2;
      }
      if (direction == Direction.WEST) {
        currentX -= 2;
      }
      newSectors.Add(new Vector2(currentX, currentY - 1));
      newSectors.Add(new Vector2(currentX, currentY));
      newSectors.Add(new Vector2(currentX, currentY + 1));
    }

    if (direction == Direction.NORTHWEST) {
      // North Sectors
      newSectors.Add(new Vector2(currentX - 1, currentY + 2));
      newSectors.Add(new Vector2(currentX, currentY + 2));
      newSectors.Add(new Vector2(currentX + 1, currentY + 2));
      // West Sectors
      newSectors.Add(new Vector2(currentX - 2, currentY - 1));
      newSectors.Add(new Vector2(currentX - 2, currentY));
      newSectors.Add(new Vector2(currentX - 2, currentY + 1));
      // NorthWest Sector
      newSectors.Add(new Vector2(currentX - 2, currentY + 2));
    }

    if (direction == Direction.NORTHEAST) {
      // North Sectors
      newSectors.Add(new Vector2(currentX - 1, currentY + 2));
      newSectors.Add(new Vector2(currentX, currentY + 2));
      newSectors.Add(new Vector2(currentX + 1, currentY + 2));
      // East Sectors
      newSectors.Add(new Vector2(currentX + 2, currentY - 1));
      newSectors.Add(new Vector2(currentX + 2, currentY));
      newSectors.Add(new Vector2(currentX + 2, currentY + 1));
      // NorthEast Sector
      newSectors.Add(new Vector2(currentX + 2, currentY + 2));
    }

    if (direction == Direction.SOUTHEAST) {
      // South Sectors
      newSectors.Add(new Vector2(currentX - 1, currentY - 2));
      newSectors.Add(new Vector2(currentX, currentY - 2));
      newSectors.Add(new Vector2(currentX + 1, currentY - 2));
      // East Sectors
      newSectors.Add(new Vector2(currentX + 2, currentY - 1));
      newSectors.Add(new Vector2(currentX + 2, currentY));
      newSectors.Add(new Vector2(currentX + 2, currentY + 1));
      // SouthEast Sector
      newSectors.Add(new Vector2(currentX + 2, currentY - 2));
    }

    if (direction == Direction.SOUTHWEST) {
      // South Sectors
      newSectors.Add(new Vector2(currentX - 1, currentY - 2));
      newSectors.Add(new Vector2(currentX, currentY - 2));
      newSectors.Add(new Vector2(currentX + 1, currentY - 2));
      // West Sectors
      newSectors.Add(new Vector2(currentX - 2, currentY - 1));
      newSectors.Add(new Vector2(currentX - 2, currentY));
      newSectors.Add(new Vector2(currentX - 2, currentY + 1));
      // SouthWest Sector
      newSectors.Add(new Vector2(currentX - 2, currentY - 2));
    }

    return newSectors;
  }

  private float CalculateHighestX(List<Vector2> input) {
    var highestX = float.MinValue;
    foreach (var v in input) {
      if (v.x > highestX) {
        highestX = v.x;
      }
    }
    return highestX;
  }

  private float CalculateLowestX(List<Vector2> input) {
    var lowestX = float.MaxValue;
    foreach (var v in input) {
      if (v.x < lowestX) {
        lowestX = v.x;
      }
    }
    return lowestX;
  }

  private float CalculateHighestY(List<Vector2> input) {
    var highestY = float.MinValue;
    foreach (var v in input) {
      if (v.y > highestY) {
        highestY = v.y;
      }
    }
    return highestY;
  }

  private float CalculateLowestY(List<Vector2> input) {
    var lowestY = float.MaxValue;
    foreach (var v in input) {
      if (v.y < lowestY) {
        lowestY = v.y;
      }
    }
    return lowestY;
  }
}
