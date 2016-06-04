using UnityEditor;

public class ExportDefine
{
#if UNITY_IPHONE
    public const string m_prefix = "iOS";
    public const BuildTarget m_buildTarget = BuildTarget.iPhone;
#elif UNITY_ANDROID
    public const string m_prefix = "android";
    public const BuildTarget m_buildTarget = BuildTarget.Android;
#elif UNITY_WEBPLAYER
    public const string m_prefix = "pc";
    public const BuildTarget m_buildTarget = BuildTarget.WebPlayer;
#else
    public const string m_prefix = "pc";
    public const BuildTarget m_buildTarget = BuildTarget.StandaloneWindows;
#endif

}