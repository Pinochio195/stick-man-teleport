using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class ExportSetting
{
    [MenuItem("Build/BuildTestApk")]
    public static void BuildApkTest()
    {
        EditorUserBuildSettings.buildAppBundle = false;
        var outdir = System.Environment.CurrentDirectory + "/Build/Android/Dev";
        var outputPath = Path.Combine(outdir, $"EG_Dev.apk");
        Debug.Log("outdir :" + outputPath);

        if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);
        if (File.Exists(outputPath)) File.Delete(outputPath);

        BuildReport report = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, outputPath, BuildTarget.Android,
            BuildOptions.None);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build Success :" + outputPath);
            if (!File.Exists(outputPath))
            {
                Debug.LogException(new Exception("Cannot find apk"));
            }
        }
        else
        {
            Debug.LogError("Build Failed");
            
        }
    }

    [MenuItem("Build/BuildReleaseApk")]
    public static void BuildReleaseApk()
    {
        EditorUserBuildSettings.buildAppBundle = false;
        var outdir = System.Environment.CurrentDirectory + "/Build/Android/Release";
        var outputPath = Path.Combine(outdir, $"EG_Release.apk");
        Debug.Log("outdir :" + outputPath);

        if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);
        if (File.Exists(outputPath)) File.Delete(outputPath);

        BuildReport report = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, outputPath, BuildTarget.Android,
            BuildOptions.None);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build Success :" + outputPath);
            if (!File.Exists(outputPath))
            {
                Debug.LogException(new Exception("Cannot find apk"));
            }
        }
        else
        {
            Debug.LogError("Build Failed");
        }
    }

    [MenuItem("Build/BuildReleaseAab")]
    public static void BuildReleaseAab()
    {
        EditorUserBuildSettings.buildAppBundle = true;
        var outdir = System.Environment.CurrentDirectory + "/Build/Android/Release";
        var outputPath = Path.Combine(outdir, $"EG_Release.aab");
        Debug.Log("outdir :" + outputPath);

        if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);
        if (File.Exists(outputPath)) File.Delete(outputPath);

        BuildReport report = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, outputPath, BuildTarget.Android,
            BuildOptions.None);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build Success :" + outputPath);
            if (!File.Exists(outputPath))
            {
                Debug.LogException(new Exception("Cannot find apk"));
            }
        }
        else
        {
            Debug.LogError("Build Failed");
        }
    }
}