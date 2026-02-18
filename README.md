# BlazorPluginComponents — Dynamic plugin architecture for Blazor

[![License](https://img.shields.io/github/license/phmatray/BlazorPluginComponents)](LICENSE)
[![Stars](https://img.shields.io/github/stars/phmatray/BlazorPluginComponents?style=social)](https://github.com/phmatray/BlazorPluginComponents)

**BlazorPluginComponents** is a demonstration project showing how to dynamically load Razor Class Library (RCL) components in a Blazor Server application **without** registering them at compile time. Load components and full pages as runtime plugins — no project reference needed.

## ✨ Features

- **Dynamic component loading** — Load RCL components at runtime from DLL/assets without a project dependency
- **4 loading strategies** — Standard project reference, folder-based loading, manual file upload, and NuGet Package (Module Manager)
- **Module Manager** — Upload a `.nupkg` file once; the server caches it and serves the component on demand
- **Dynamic page loading** — Load entire Blazor pages dynamically through the Module Manager
- **Blazor Server** — Works with Blazor Server (hosted model)

## 🏗️ Architecture

A Razor Class Library (RCL) published via `dotnet publish` or `dotnet pack` produces:

| Artifact | Description |
|---|---|
| `*.dll` + `*.pdb` | Assembly and debug symbols |
| `wwwroot/` assets | Isolated CSS, JavaScript, images |
| `*.nupkg` | All-in-one NuGet package (via `dotnet pack`) |

The project demonstrates 4 ways to consume these artifacts at runtime:

1. **Standard** — Project reference (compile-time, for baseline comparison)
2. **Folder-based** — Copy RCL files to a folder under `wwwroot`, load dynamically
3. **Manual upload** — Upload DLL + assets via the app's UI
4. **Module Manager** — Upload the `.nupkg` once, the server extracts and caches everything

## 🚀 Quick Start

### Prerequisites

- [.NET 6+ SDK](https://dotnet.microsoft.com/download)
- Git

### Setup

```bash
# Clone the repository
git clone https://github.com/phmatray/BlazorPluginComponents.git
cd BlazorPluginComponents

# Build RazorClassLibrary2 first (post-build step copies assets to Server/wwwroot)
dotnet build RazorClassLibrary2/RazorClassLibrary2.csproj

# Run the server
dotnet run --project Server/BlazorPlugin2.Server.csproj
```

> ⚠️ Build `RazorClassLibrary2` before the server project — its post-build command copies component assets to `Server/wwwroot`. Skipping this step will cause a runtime error.

### Demo

**Dynamic component loading from an RCL folder:**

![Dynamic component loading](https://user-images.githubusercontent.com/9949584/160662593-5f765ee3-149c-4a0c-a0fe-a22d6a3193c7.gif)

**Dynamic page loading via Module Manager:**

![Dynamic page loading](https://user-images.githubusercontent.com/9949584/170790487-02d37b12-465e-4afe-8f4f-365b953b5341.gif)

## 📖 Documentation

Explore each project in the solution to understand each loading strategy:

| Project | Strategy |
|---|---|
| `RazorClassLibrary1` | Standard (compile-time reference) |
| `RazorClassLibrary2` | Folder-based (post-build copy) |
| `RazorClassLibrary3` | Manual upload / Module Manager |
| `Server` | Host application |

## 🤝 Contributing

Pull requests welcome. Fork the repo, create a branch from `master`, and open a PR. For large changes, open an issue first.

## 📄 License

MIT — see [LICENSE](LICENSE)
