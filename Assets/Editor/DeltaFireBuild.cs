using UnityEditor;
using UnityEngine;

namespace DeltaFire.Editor
{
    public static class DeltaFireBuild
    {
        public static void BuildAndroid()
        {
            DeltaFirePrototypeGenerator.Create();
            const string scene = "Assets/Scenes/DeltaFirePrototype.unity";
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(scene, true) };

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { scene },
                locationPathName = "build/DeltaFire.apk",
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                throw new System.Exception("DeltaFire Android build failed: " + report.summary.result);

            Debug.Log("DeltaFire APK built successfully: " + options.locationPathName);
        }
    }
}
