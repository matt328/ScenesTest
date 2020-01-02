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

    public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component {
      if (parent == null) { throw new System.ArgumentNullException(); }
      if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
      List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
      if (list.Count == 0) { return null; }

      for (int i = list.Count - 1; i >= 0; i--) {
        if (list[i].CompareTag(tag) == false) {
          list.RemoveAt(i);
        }
      }
      return list.ToArray();
    }

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component {
      if (parent == null) { throw new System.ArgumentNullException(); }
      if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }

      T[] list = parent.GetComponentsInChildren<T>(forceActive);
      foreach (T t in list) {
        if (t.CompareTag(tag) == true) {
          return t;
        }
      }
      return null;
    }
  }
}
