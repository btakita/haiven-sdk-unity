# Unity engine test host

Haiven's source in this repository, including this host, is Apache-2.0. See
[`LICENSE`](../LICENSE). This host does not vendor Unity source.
`com.unity.test-framework` in `Packages/manifest.json` is a Package Manager
dependency, not copied source.

This host consumes `file:../../com.haiven.sdk` through UPM. That path is
relative to `host/Packages` and resolves to `com.haiven.sdk` at the root of
this repository. The package compiles both the Unity bridge (`Haiven.Sdk`) and
the portable core (`Haiven.Sdk.Core`) from source under `Runtime/` and
`Runtime/Core`. Do not publish `Haiven.Sdk.Core.dll` or its NuGet neighbors
into `host/Assets/Plugins`: Unity rejects a precompiled assembly and an
assembly definition that share the name `Haiven.Sdk.Core`. No replacement
socket or fake Unity API is compiled into this host.

## Editor

Use **Unity 6000.0.52f1 (9e4086222921)** with the matching **WebGL Build
Support** module, which includes Unity's IL2CPP/Emscripten toolchain.
[Official editor and module downloads](https://unity.com/releases/editor/whats-new/6000.0.52f1).
Activate the editor with your own Unity license; keep license credentials out of
this repository and out of command logs.

## Run

From the repository root, without an editor, check the package layout:

```sh
node validate-package.mjs
```

With the editor, open `host/` as a Unity project and use the Test Runner
window, or run the same tests in batch mode. `UNITY_EDITOR` here stands for the
6000.0.52f1 editor binary:

```sh
"$UNITY_EDITOR" -batchmode -nographics -projectPath host \
  -runTests -testPlatform EditMode -testResults "$PWD/build/unity-results/EditMode.xml"
"$UNITY_EDITOR" -batchmode -nographics -projectPath host \
  -runTests -testPlatform PlayMode -testResults "$PWD/build/unity-results/PlayMode.xml"
HAIVEN_WEBGL_OUTPUT="$PWD/build/unity-webgl" \
  "$UNITY_EDITOR" -batchmode -nographics -quit -projectPath host \
  -buildTarget WebGL -executeMethod HaivenBuild.WebGL
```

`-runTests` exits on its own when the run finishes, so do not add `-quit` to
the test commands. The NUnit XML report lists each case and its result.
`HaivenBuild.WebGL` requires `HAIVEN_WEBGL_OUTPUT` and fails without it. It
writes the player and a `haiven-build.json` summary to that directory.

The PlayMode socket case connects to a loopback WebSocket fixture at
`ws://127.0.0.1:18765/socket` (`HAIVEN_TEST_SOCKET_URL` overrides it). The
WebGL player expects the server that serves its page to answer a WebSocket at
`/socket` and the game-login handoff endpoints. Neither fixture is in this
repository, so without one the socket case fails and the built player logs a
`HAIVEN_WEBGL_FAIL` line instead of `HAIVEN_WEBGL_PASS`.

Release assets are not built from this host. Haiven's release pipeline
publishes them; see Releases in the repository README.

EditMode asserts local package registration, refused-reply/event presence, and
main-thread dispatch. PlayMode makes two real `SystemNetSocket` connections,
observes callbacks, sends a Unicode frame, and disposes each connection. It also
delivers an auth callback through `HandoffDeepLinkReceiver` after an asynchronous
exchange and requires token storage and completion on Unity's main thread. The
socket case is transport reconnection; the C# core does not yet provide a
higher-level Session state machine.

The release WebGL player uses **High managed stripping** and the package's
actual editor-excluded `.jslib` bridge. Before it logs `HAIVEN_WEBGL_PASS`, the
player requires a real Unicode socket round trip, protocol parsing, a real
`UnityWebRequest` form exchange, the returned token, and completion on the main
thread, and then disposes the socket.

`link.xml` necessity is not established merely by a successful build: a
removal experiment remains part of engine validation.
