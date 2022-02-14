// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Drawing
open System.IO
open System.Reflection
open System.Text.Json
open PhotinoNET
open System.Text.RegularExpressions



type Message = {Type: string; Body: string}

type Table = {withHeadings: bool; content: string array array}

let sendMessage (wnd:PhotinoWindow) (message:Message) =
    let msg = JsonSerializer.Serialize(message)
    wnd.SendWebMessage(msg) |> ignore

let table2md (table:Table) =
    let arr2mdline arr =
        arr |> String.concat " | " |> printfn "| %s |"
    if table.content.Length > 0 then
        arr2mdline table.content.[0]
        if table.withHeadings then
            table.content.[0] |> Array.map (fun s-> "----") |> arr2mdline
    if table.content.Length > 1 then
        table.content.[1..] |> Array.iter (fun arr-> arr |> arr2mdline)


let sepPat = Regex "\| *---"
let chopSpacePat = Regex " *(.*[^ ]) *"

let chopSpace line = 
    let res = chopSpacePat.Match line
    if res.Success then
        res.Groups.[1].Value
    else
        ""

let md2table (lines: string list) =
    let line2array (line:string) =
        line.Split "|" |> (fun arr->Array.sub arr 1 (arr.Length-2)) |> Array.map chopSpace
    let head = line2array lines.[0]
    let withHeadings = lines.Length > 1 && sepPat.Match(lines.[1]).Success
    if withHeadings then
        let tail = lines.[2..] |> List.map line2array
        {withHeadings=true; content=List.Cons(head, tail) |> List.toArray}
    else
        let tail = lines.[1..] |> List.map line2array
        {withHeadings=false; content=List.Cons(head, tail) |> List.toArray}



    

let onMessage tablejson (wnd:Object) (message:string) =
    let pwnd = wnd :?>PhotinoWindow
    let msg = JsonSerializer.Deserialize<Message>(message)
    match msg.Type with
    | "notifyLoaded" -> sendMessage pwnd {Type="loadTable"; Body=tablejson}
    | "notifyCancel" -> Environment.Exit 1
    | "notifyDone" ->
        let table = JsonSerializer.Deserialize<Table>(msg.Body)
        table2md table
        Environment.Exit 0
    | "notifyDeb" -> printfn "deb: %s" msg.Body
    | _ -> failwithf "Unknown msg type %s" msg.Type

let launchBrowser (tablejson : string)  =
    let onFinish (results:string array) = ()
    let onCancel () = ()


    let asm = Assembly.GetExecutingAssembly()

    let load (url:string) (prefix:string) =
        let fname = url.Substring(prefix.Length)
        asm.GetManifestResourceStream($"mdvtbl.assets.{fname}")

    let win = (new PhotinoWindow(null))

    win.LogVerbosity <- 0
    win.SetTitle("mdvtbl")
        .SetUseOsDefaultSize(false)
        .Center()
        .RegisterCustomSchemeHandler("resjs",
            PhotinoWindow.NetCustomSchemeDelegate(fun sender scheme url contentType ->
                contentType <- "text/javascript"
                load url "resjs:"))
        .RegisterCustomSchemeHandler("rescss", 
            PhotinoWindow.NetCustomSchemeDelegate(fun sender scheme url contentType ->
                contentType <- "text/css"
                load url "rescss:")) |> ignore
                
    let asm = Assembly.GetExecutingAssembly()
    use stream = asm.GetManifestResourceStream("mdvtbl.assets.index.html")
    use sr = new StreamReader(stream)
    let text = sr.ReadToEnd()
    // printfn "content: %s" text

    win.RegisterWebMessageReceivedHandler(System.EventHandler<string>(onMessage tablejson))
        .Center()
        .SetSize(new Size(1200, 700))
        .LoadRawString(text)
        .WaitForClose()

let readLines () =
    fun _ -> Console.ReadLine()
    |>  Seq.initInfinite
    |>  Seq.takeWhile ((<>) null) 
    |>  Seq.filter (fun line-> line.Contains("|"))
    |>  Seq.toList

[<EntryPoint>]
let main argv =
    if argv.Length <> 0 then
        printfn "usage: mdvtbl"
        printfn "Read markdown table from stdin. No argument."
        1
    else
        let lines =  readLines ()
        // let lines =  ["|abc|def|ghi|"; "|----|----|---|"; "|hoge|hoge|ika|"]

        let table =  if lines.Length > 1 then
                        md2table lines
                     else
                        {withHeadings=false; content=[|[|"";""|]|]}
        launchBrowser (JsonSerializer.Serialize table)

        0 // return an integer exit code