namespace WebSharperTutorial.FrontEnd

open System
open WebSharper
open WebSharper.Resources

module AppResources =

    module MetroUi =
        type Js () = inherit BaseResource("/vendor/metroui/metro.js")
        type Css () = inherit BaseResource("/vendor/metroui/metro.css")

    module FrontEndApp =
        type Js () = inherit BaseResource("/app/js/common.js")
        type Css () = inherit BaseResource("/app/css/common.css")

    [<assembly:Require(typeof<MetroUi.Js>);
      assembly:Require(typeof<MetroUi.Css>);
      assembly:Require(typeof<FrontEndApp.Js>);
      assembly:Require(typeof<FrontEndApp.Css>);
      assembly:Require(typeof<JQuery.Resources.JQuery>);
    >]
    do ()
