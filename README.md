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
$ echo | mdvtbl
```

If you have markdown table file, you can use cat and pipe.

```
$ cat table.md | mdvtbl
```

### How to make first row title

At upper left corner of the table, there is a menu for changing title behaviour.
You select "With heading", the first row becomes title row.

### Tips for mac (clipboard)

mdvtbl is designed to use with clipboard.

In mac, I write following shell script named cbtbl.command (file extension matters)

```
#!/usr/bin/env zsh

pbpaste | mdvtbl | pbcopy
```

Then, you can edit in your favorite editor or textarea in following way:

1. Select markdown table region in your editor that you want to edit
2. Cmd+C
3. Launch cbtbl.command via Spotlight
4. Edit table in GUI
5. Press Done
6. Cmd+V in editor.


## Open source libraries

mdvcat uses following libraries.

- [Photino.NET](https://www.nuget.org/packages/Photino.NET/)
- [editor.js](https://github.com/codex-team/editor.js)
- [editor-js/table](https://github.com/editor-js/table) 
