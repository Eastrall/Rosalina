#if UNITY_EDITOR
using System;
using UnityEditor;

/// <summary>
/// Provides utilities for Rosalina.
/// </summary>
internal static class RosalinaEditorUtilities
{
    /// <summary>
    /// Shows a progress bar.
    /// </summary>
    /// <param name="title">Progress bar title.</param>
    /// <param name="message">Progress bar message.</param>
    /// <param name="currentProgress">Current progress.</param>
    /// <param name="totalProgress">Total progress.</param>
    public static void ShowProgressBar(string title, string message, float currentProgress, float totalProgress)
    {
        float progress = Math.Clamp((currentProgress / totalProgress) * 100, 0, 100);

        ShowProgressBar(title, message, progress);
    }

    /// <summary>
    /// Shows a progress bar.
    /// </summary>
    /// <param name="title">Progress bar title.</param>
    /// <param name="message">Progress bar message.</param>
    /// <param name="progress">Current progress between 0 and 100.</param>
    public static void ShowProgressBar(string title, string message, float progress)
    {
        EditorUtility.DisplayProgressBar(title, message, Math.Clamp(progress, 0, 100));
    }

    /// <summary>
    /// Hides the progress bar.
    /// </summary>
    public static void HideProgressBar() => EditorUtility.ClearProgressBar();
}

#endif