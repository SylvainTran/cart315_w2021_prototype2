using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BoltUIManager.Editor {
  public class InstallWindow : EditorWindow {
    private static Dictionary<string, DependencyPackage> missingPackages = new Dictionary<string, DependencyPackage>();
    private static bool hasMissingPackage = false;

    public static Dictionary<string, DependencyPackage> dependencies = new Dictionary<string, DependencyPackage>() {
      { "Bolt", new DependencyPackage("Bolt", "https://assetstore.unity.com/packages/tools/visual-scripting/bolt-163802") },
      { "DG.Tweening", new DependencyPackage("DOTween (HOTween v2)", "https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676") },
    };

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded() {
      Dictionary<string, DependencyPackage> result = NamespaceExists(dependencies);
      foreach (var item in result) {
        if (item.Value != null) {
          GetWindow(typeof(InstallWindow), true, "Bolt UI Manager Setup");
          break;
        }
      }

      RefreshDependencyState();
    }

    private void OnDestroy() {
      Dictionary<string, DependencyPackage> result = NamespaceExists(dependencies);
      foreach (var item in result) {
        if (item.Value != null) {
          var newWin = Instantiate(this);
          newWin.ShowUtility();
          RefreshDependencyState();
          break;
        }
      }
    }

    static void RefreshDependencyState() {
      missingPackages = NamespaceExists(dependencies);
      foreach (var missingPackage in missingPackages) {
        if (missingPackage.Value != null) {
          hasMissingPackage = true;
        }
      }
    }

    void OnGUI() {
      GUILayout.BeginVertical("box");
      if (hasMissingPackage) {
        GUILayout.Label("Install the missing package(s) before using Bolt UI Manager");
        foreach (var missingPackage in missingPackages) {
          if (missingPackage.Value != null) {
            if (GUILayout.Button(string.Format("Install {0} asset (Open in Browser)", missingPackage.Value.packageName))) {
              Application.OpenURL(missingPackage.Value.packageLink);
            }
          }
        }
      } else {
        GUILayout.Label("Bolt UI Manager is ready for using!");
        if (GUILayout.Button("Close")) {
          this.Close();
        }
      }

      GUILayout.EndVertical();
    }

    public class DependencyPackage {
      public string packageName { get; private set; }
      public string packageLink { get; private set; }

      public DependencyPackage(string packageName, string packageLink) {
        this.packageName = packageName;
        this.packageLink = packageLink;
      }
    }

    public static Dictionary<string, DependencyPackage> NamespaceExists(Dictionary<string, DependencyPackage> dependencyPackages) {
      Dictionary<string, DependencyPackage> result = new Dictionary<string, DependencyPackage>();
      foreach (var dependencyPackage in dependencyPackages) {
        result.Add(dependencyPackage.Key, dependencyPackage.Value);
      }

      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
        foreach (Type type in assembly.GetTypes()) {
          if (type.Namespace != null && result.ContainsKey(type.Namespace)) {
            result[type.Namespace] = null;
          }
        }
      }

      return result;
    }
  }
}