namespace App

open Feliz
open Feliz.Router
open Fable.Core.JS

type Components =
    [<ReactComponent>]
    static member LoggedOut() =
        Html.div [
            Html.h1 "Login"
            Html.input [
                prop.type' "text"
                prop.placeholder "Username"
            ]
            Html.input [
                prop.type' "password"
                prop.placeholder "Password"
            ]
            Html.button [
                prop.text "Login"
            ]
        ]


    /// <summary>
    /// The simplest possible React component.
    /// Shows a header with the text Hello World
    /// </summary>
    [<ReactComponent>]
    static member HelloWorld() = Html.h1 "Hello World"

    /// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Counter() =
        let (count, setCount) = React.useState(0)
        Html.div [
            Html.h1 count
            Html.button [
                prop.onClick (fun _ -> setCount(count + 1))
                prop.text "Increment"
            ]
        ]