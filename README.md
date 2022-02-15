# mdvtbl

GUI GFM markdown table editor with stdin-stdout.

![demo animation of gif](https://github.com/karino2/mdvtbl/raw/master/screenshot/mdvtbl_demo.gif)

## Install

For Mac, there is binary drop in [Releases](https://github.com/karino2/mdvtbl/releases)

Also, there is homebrew tap for mdvtbl.
You can install follwoing way.

```
$ brew tap karino2/tap
$ brew install karino2/tap/mdvtbl
```

### Troubleshooting

If you see the error saying

```
A fatal error occurred. The required library libhostfxr.dylib could not be found.
```

You need to set DOTNET_ROOT like following (in .zshrc or .bashrc, etc.)

```
$ export DOTNET_ROOT="$(brew --prefix)/opt/dotnet/libexec"
```


## Usage

mdvtbl read from stdin. So just execute this command wait forever.

If you have no markdown, use empty echo like this.

```
$ echo | mdvtvl
```



## Open source libraries

mdvcat uses following libraries.

- [Photino.NET](https://www.nuget.org/packages/Photino.NET/)
- [editor.js](https://github.com/codex-team/editor.js)
- [editor-js/table](https://github.com/editor-js/table) 
