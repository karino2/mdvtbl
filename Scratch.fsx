[|"hoge";"ika"|] |> String.concat "\n"

[|"hoge";"ika"|] |> String.concat "|"


let tmp = [|[|"hoge";"ika"|];[|"hega";"hoga"|];[|"t3";"t4"|]|]


tmp.[1..]

tmp.Length

let tmp2 = ["|abc|def|ghi|"; "|----|----|---|"; ""]

tmp2.Length

tmp2.[1].StartsWith("|---")
tmp2.[0].Split "|"

tmp2.[1..tmp2.Length-1]

let tmp3 = tmp2.[0].Split "|"


Array.sub tmp3 1 (tmp3.Length-2)

type Table = {withHeadings: bool; content: string array array}

let md2table (linesIn: string list) =
    let lines = linesIn |>List.filter (fun line-> line.Contains("|")) 
    let line2array (line:string) =
        line.Split "|" |> (fun arr->Array.sub arr 1 (arr.Length-2))
    let head = line2array lines.[0]
    let withHeadings = lines.Length > 1 && lines.[1].StartsWith "|---"
    if withHeadings then
        let tail = lines.[2..] |> List.map line2array
        {withHeadings=true; content=List.Cons(head, tail) |> List.toArray}
    else
        let tail = lines.[1..] |> List.map line2array
        {withHeadings=false; content=List.Cons(head, tail) |> List.toArray}


md2table tmp2


md2table ["|abc|def|ghi|"; "|----|----|---|"; "|hoge|hoge|ika|"]
md2table ["|abc|def|ghi|"]

md2table ["|abc|def|ghi|"; "|----|----|---|";]

tmp2 |>List.filter (fun line-> line.Contains("|")) 


open System.Text.RegularExpressions

let pat = Regex "\| *---"
pat.Match "|---"


pat.Match "|  ---"

pat.Match "|hoge"


let pat2 = Regex " *(.*[^ ]) *"


pat2.Match " abc "

let res = pat2.Match " abc def "
res.Groups.[1].Value

pat2.Match ""


let chopSpacePat = Regex " *(.*[^ ]) *"

let chopSpace line = 
    let res = chopSpacePat.Match line
    if res.Success then
        res.Groups.[1].Value
    else
        ""


chopSpace " abc def "

chopSpace "abc def "

chopSpace " abc def"

chopSpace "abc"

chopSpace ""
