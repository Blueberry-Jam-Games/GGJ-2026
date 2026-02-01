using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Build.Profile;
using UnityEngine;
using System.Linq;

using System.Collections.Generic;

public class JenkinsBuild : Editor
{
    static void SetActiveProfileByName(string profileName)
    {
        var guids = AssetDatabase.FindAssets("t:BuildProfile");

        var profile = guids
            .Select(g => AssetDatabase.LoadAssetAtPath<BuildProfile>(
                AssetDatabase.GUIDToAssetPath(g)))
            .FirstOrDefault(p => p.name == profileName);

        if (profile == null)
        {
            Debug.LogError($"Build Profile '{profileName}' not found.");
            EditorApplication.Exit(1);
            return;
        }

        BuildProfile.SetActiveBuildProfile(profile);
        Debug.Log($"Active Build Profile set to: {profile.name}");
    }

    [MenuItem("Blueberry Jam/Build/Windows")]
    public static void BuildWindows()
    {
        BuildPlayerOptions build_player_options = new BuildPlayerOptions();
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        string[] scenes_from_settings = new string[scenes.Count];
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes_from_settings[i] = scenes[i].path;
            //Debug.Log("scene: " + scenes[i].path);
        }

        build_player_options.scenes = scenes_from_settings;
        build_player_options.locationPathName = "builds/build.exe";
        build_player_options.target = BuildTarget.StandaloneWindows64;

        build_player_options.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(build_player_options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Windows Build succeeded");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Windows Build failed");
        }
    }

    [MenuItem("Blueberry Jam/Build/WebGL")]
    public static void BuildWebGL()
    {
        SetActiveProfileByName("WebGL-CI");

        var profile = BuildProfile.GetActiveBuildProfile();
        if (profile == null)
        {
            Debug.LogError("No active Build Profile!");
            EditorApplication.Exit(1);
            return;
        }

        var scenes = profile.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.WebGL,
            locationPathName = "builds"
        });
    }

    [MenuItem("Blueberry Jam/Build/Linux")]
    public static void BuildLinux()
    {
        BuildPlayerOptions build_player_options = new BuildPlayerOptions();
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        string[] scenes_from_settings = new string[scenes.Count];
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes_from_settings[i] = scenes[i].path;
        }

        build_player_options.scenes = scenes_from_settings;
        build_player_options.locationPathName = "Build/Linux/Blueberry-Jam-Core";
        build_player_options.target = BuildTarget.StandaloneLinux64;

        build_player_options.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(build_player_options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Another Test Connor");
            Debug.Log("Testing" + string.IsNullOrEmpty(summary.outputPath));
            Debug.Log("Linux Build succeeded" + summary.outputPath);
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Linux Build failed");
        }
    }

    [MenuItem("Blueberry Jam/Build/Android")]
    public static void BuildAndroid()
    {
        BuildPlayerOptions build_player_options = new BuildPlayerOptions();
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        string[] scenes_from_settings = new string[scenes.Count];
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes_from_settings[i] = scenes[i].path;
        }

        build_player_options.scenes = scenes_from_settings;
        build_player_options.locationPathName = "builds/build.apk";
        build_player_options.target = BuildTarget.Android;

        build_player_options.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(build_player_options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Another Test Connor");
            Debug.Log("Testing" + string.IsNullOrEmpty(summary.outputPath));
            Debug.Log("Android Build succeeded" + summary.outputPath);
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Android Build failed");
        }
    }
}
