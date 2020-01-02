using System.Collections.Generic;

public enum Direction {
  NONE,
  NORTHWEST,
  NORTH,
  NORTHEAST,
  WEST,
  CENTER,
  EAST,
  SOUTHWEST,
  SOUTH,
  SOUTHEAST
}

public class DirectionUtils {
  private static readonly Dictionary<Direction, Direction> Opposites = new Dictionary<Direction, Direction>() {
    {Direction.NONE, Direction.NONE},
    {Direction.SOUTH, Direction.NORTH},
    {Direction.NORTH, Direction.SOUTH},
    {Direction.EAST, Direction.WEST},
    {Direction.WEST, Direction.EAST},
  };
  public static Direction OppositeOf(Direction direction) {
    return Opposites[direction];
  }
}
