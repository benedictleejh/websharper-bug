namespace WebSharperTutorial.FrontEnd

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Server
open WebSharperTutorial.FrontEnd.Routes

open WebSharperTutorial.FrontEnd.Pages

module Site =
    open WebSharper.UI.Html
    open type WebSharper.UI.ClientServer

    type MainTemplate = Templating.Template<"Templates/Main.html">

    let private MainTemplate ctx action (title: string)  (body: Doc List) =
        Content.Page(
            MainTemplate()
                .Title(title)
                .Body(body)
                .Doc()
        )

    let LoginPage ctx endpoint =
        let body =
            client <@
                let router = InstallRouter ()
                router.View
                |> View.Map (fun endpoint -> Pages.Login.Main router)
                |> Doc.EmbedView
            @>

        MainTemplate ctx endpoint "Login" [ body ]

    let HomePage ctx =
        MainTemplate ctx EndPoint.Home "Home" [
            h1 [] [text "It works!"]
            client <@ div [] [text "Hi there!"] @>
        ]

    [<Website>]
    let Main =
        Sitelet.New
            SiteRouter
            (fun ctx endpoint ->
                let loggedInUser =
                    async {
                        return! ctx.UserSession.GetLoggedInUser ()
                    } |> Async.RunSynchronously

                match loggedInUser with
                | None ->
                    match endpoint with
                    | EndPoint.Home -> HomePage ctx
                    | EndPoint.Login ->
                        LoginPage ctx endpoint
                    | EndPoint.AccessDenied ->
                        MainTemplate
                            ctx
                            EndPoint.Home
                            "Access Denied"
                            [ div [] [ text "Access denied" ] ]
                    | _ ->
                        Content.RedirectTemporary AccessDenied
                | Some u ->
                    match endpoint with
                    | EndPoint.Home -> HomePage ctx
                    | EndPoint.Login ->
                        LoginPage ctx endpoint
                    | EndPoint.Listing ->
                        MainTemplate
                            ctx
                            EndPoint.Home
                            "Listing"
                            [ div [] [ text "Listing - Implementation pending" ] ]
                    | EndPoint.Form c ->
                        MainTemplate
                            ctx
                            EndPoint.Home
                            "Form"
                            [ div [] [ text "Form - Implementation pending" ] ]
                    | _ ->
                        MainTemplate
                            ctx
                            EndPoint.Home
                            "Not implemented"
                            [ div [] [ text "Implementation pending" ] ]
            )
