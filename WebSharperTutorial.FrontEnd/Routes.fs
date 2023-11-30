namespace WebSharperTutorial.FrontEnd

open System
open WebSharper
open WebSharper.Sitelets
open WebSharper.UI

module Routes =

    [<JavaScript>]
    type EndPoint =
        | [<EndPoint>] Home
        | [<EndPoint>] Login
        | [<EndPoint>] AccessDenied
        | [<EndPoint>] Listing
        | [<EndPoint>] Form of int64

    [<JavaScript>]
    let SiteRouter: Router<EndPoint>  =
        let link endPoint =
            match endPoint with
            | Home -> []
            | Login -> [ "login" ]
            | AccessDenied -> [ "access-denied" ]
            | Listing -> [ "private"; "listing" ]
            | Form code -> [ "private"; "form"; string code ]

        let route path =
            match path with
            | [] -> Some Home
            | [ "login" ] -> Some Login
            | [ "access-denied" ] -> Some AccessDenied
            | [ "private"; "listing" ] -> Some Listing
            | [ "private"; "form"; code ] -> Some (Form (int64 code))
            | _ -> None

        Router.Create link route

    [<JavaScript>]
    let InstallRouter () =
        let router =
            SiteRouter
            |> Router.Slice
                (fun endpoint ->
                    match endpoint with
                    | AccessDenied -> None
                    | _ -> Some endpoint
                )
                id
            |> Router.Install Home
        router
