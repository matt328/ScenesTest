using System.Collections.Generic;
using UnityEngine;

public class Constants {
  public static readonly Dictionary<Direction, Vector2> LocationMap = new Dictionary<Direction, Vector2>(){
    { Direction.NORTHWEST, new Vector2(-1, 1)},
    { Direction.NORTH, new Vector2(0, 1)},
    { Direction.NORTHEAST, new Vector2(1, 1)},
    { Direction.WEST, new Vector2(-1, 0)},
    { Direction.CENTER, new Vector2(0, 0)},
    { Direction.EAST, new Vector2(1, 0)},
    { Direction.SOUTHWEST, new Vector2(-1, -1)},
    { Direction.SOUTH, new Vector2(0, -1)},
    { Direction.SOUTHEAST, new Vector2(1, -1)},
  };
}
