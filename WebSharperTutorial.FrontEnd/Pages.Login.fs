namespace WebSharperTutorial.FrontEnd.Pages

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JQuery
open WebSharper.JavaScript

open WebSharperTutorial.FrontEnd
open WebSharperTutorial.FrontEnd.Routes

[<JavaScript>]
module Login =

    type private LoginFormTemplate = Templating.Template<"Templates/Pages.Login.html">

    let private AlertBox (rvStatusMsg : Var<string option>) =
        rvStatusMsg.View
        |> View.Map (fun msgOpt ->
            match msgOpt with
            | None -> Doc.Empty
            | Some msg ->
                div [ attr.``class`` "alert primary"
                      Attr.Create "role" "alert"
                    ]
                    [ text msg ]
            )
        |> Doc.EmbedView

    let private LoginForm (router : Var<EndPoint>) =
        let rvEmail = Var.Create ""
        let rvPassword = Var.Create ""
        let rvRememberLogin = Var.Create false
        let rvStatusMsg = Var.Create None

        let statusMsgBox = AlertBox rvStatusMsg

        LoginFormTemplate()
            .AlertBox(statusMsgBox)
            .Username(rvEmail)
            .Password(rvPassword)
            .RememberMe(rvRememberLogin)
            .OnLogin(fun _ ->
                // JQuery.Of("form").One("submit", fun elem evt -> evt.PreventDefault()).Ignore
                async {
                    let! response = Remote<Auth.RpcUserSession>.CheckCredentials rvEmail.Value rvPassword.Value rvRememberLogin.Value
                    match response with
                    | Result.Ok c ->
                        rvEmail.Value <- ""
                        rvPassword.Value <- ""
                        rvStatusMsg.Value <- None
                        router.Value <- Listing
                    | Result.Error err ->
                        rvStatusMsg.Value <- Some err
                }
                |> Async.Start
            )
            .OnLogout(fun _ ->
                async {
                    do! Remote<Auth.RpcUserSession>.Logout ()
                    Var.Set router Home
                }
                |> Async.Start
            )
            .Doc()

    let Main router =
        let loginForm = LoginForm router

        div [ attr.``class`` "container" ]
            [
                div [ attr.``class`` "row" ]
                    [ div [ attr.``class`` "cell-sm-12 cell-md-6 mx-auto" ] [ loginForm ] ]
            ]
