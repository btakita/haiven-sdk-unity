#nullable enable
// Unity's AOT.MonoPInvokeCallback, reproduced so the WebGL transport can be
// compiled outside Unity.
//
// The real one is a marker attribute: IL2CPP reads it to decide what needs a
// reverse-pinvoke wrapper, and the C# compiler only ever checks that it is
// applicable to a method. Nothing this build verifies depends on which of the
// two is present, which is what makes the substitution honest rather than a
// test of a fake.

namespace AOT
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    internal sealed class MonoPInvokeCallbackAttribute : System.Attribute
    {
        public MonoPInvokeCallbackAttribute(System.Type type)
        {
            Type = type;
        }

        public System.Type Type { get; }
    }
}

namespace UnityEngine
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public sealed class SerializeField : System.Attribute { }

    public struct Rect
    {
        public Rect(float x, float y, float width, float height) { }
    }

    public sealed class Coroutine { }

    public class MonoBehaviour
    {
        protected Coroutine StartCoroutine(System.Collections.IEnumerator routine) => new Coroutine();
        protected void StopAllCoroutines() { }
        protected int GetInstanceID() => 0;
    }

    public static class GUI
    {
        public static bool enabled { get; set; }
        public static Rect ModalWindow(int id, Rect screenRect, System.Action<int> function, string text) => screenRect;
        public static void DragWindow() { }
    }

    public static class GUILayout
    {
        public static void Label(string text) { }
        public static bool Button(string text) => false;
        public static void Space(float pixels) { }
        public static void BeginHorizontal() { }
        public static void EndHorizontal() { }
    }

    public static class Debug
    {
        public static void LogException(System.Exception exception) { }
    }

    public static class Time
    {
        public static float realtimeSinceStartup => 0;
    }

    public enum RuntimeInitializeLoadType
    {
        AfterSceneLoad,
        BeforeSceneLoad,
        AfterAssembliesLoaded,
        BeforeSplashScreen,
        SubsystemRegistration,
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public sealed class RuntimeInitializeOnLoadMethodAttribute : System.Attribute
    {
        public RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType loadType) { }
    }

    public static class Application
    {
        public static event System.Action<string>? deepLinkActivated;
        public static string persistentDataPath => "";
        public static void OpenURL(string url) { }
        public static void RaiseDeepLink(string url) => deepLinkActivated?.Invoke(url);
    }

    public class AsyncOperation
    {
        public bool isDone => false;
        public event System.Action<AsyncOperation>? completed { add { } remove { } }
    }
}

namespace UnityEngine.Networking
{
    public class UploadHandler { }
    public sealed class UploadHandlerRaw : UploadHandler
    {
        public UploadHandlerRaw(byte[] data) { }
    }

    public class DownloadHandler { public string text => ""; }
    public sealed class DownloadHandlerBuffer : DownloadHandler { }

    public sealed class UnityWebRequest : System.IDisposable
    {
        public enum Result { InProgress, Success, ConnectionError, ProtocolError, DataProcessingError }
        public const string kHttpVerbPOST = "POST";
        public UnityWebRequest(string url, string method) { }
        public UploadHandler? uploadHandler { get; set; }
        public DownloadHandler? downloadHandler { get; set; }
        public int redirectLimit { get; set; }
        public Result result => Result.Success;
        public long responseCode => 200;
        public string? error => null;
        public void SetRequestHeader(string name, string value) { }
        public UnityEngine.AsyncOperation SendWebRequest() => new UnityEngine.AsyncOperation();
        public void Abort() { }
        public void Dispose() { }
    }
}

namespace UnityEditor
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public sealed class MenuItem : System.Attribute
    {
        public MenuItem(string itemName) { }
    }
}
