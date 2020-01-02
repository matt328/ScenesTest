using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods {
  public static class MyExtensions {
    public static string Print(this List<Vector2> list) {
      var str = System.Environment.NewLine;

      foreach (var v in list) {
        str += (v.ToString() + System.Environment.NewLine);
      }

      return str;
    }

    public static string Print(this HashSet<Vector2> list) {
      var str = System.Environment.NewLine;

      foreach (var v in list) {
        str += (v.ToString() + System.Environment.NewLine);
      }

      return str;
    }
  }
}
