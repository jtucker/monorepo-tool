open System
open System.IO
open System.ComponentModel
open System.Text.Json
open System.Text.Json.Serialization

open Spectre.Console.Cli

let jsonOptions = JsonSerializerOptions ()
jsonOptions.Converters.Add (JsonFSharpConverter ())

let serialize toConvert = 
    JsonSerializer.Serialize (toConvert, jsonOptions)

type Project = {
    Name: string
    Paths: string list }

type Monorepo = {
    Name: string
    Uri: Uri option
    Projects: Project list }

type InitSettings () =
    inherit CommandSettings ()

    [<CommandArgument(0, "[PATH]")>]
    [<Description("Path to init monorepo tool settings. Defaults to the current folder.")>]
    member val Path = Environment.CurrentDirectory with get, set

    [<CommandOption("-n|--name")>]
    [<Description("Name of the monorepo. Defaults to the folder name.")>]
    member val Name = "" with get, set

type InitCommand () =
    inherit Command<InitSettings> ()
    override _.Execute(_, settings: InitSettings) = 
        let name = if String.IsNullOrEmpty (settings.Name) then (new DirectoryInfo(settings.Path)).Name else settings.Name
        printfn $"INIT Called on path: {settings.Path} and name: {name}"
        let repoSettings = { Name = name; Uri = None; Projects = List.empty } 
        
        0

[<EntryPoint>]
let main argv =
    let app = CommandApp ()

    app.Configure (fun config ->
        config.AddCommand<InitCommand>("init") |> ignore
    )

    app.Run(argv)
    

