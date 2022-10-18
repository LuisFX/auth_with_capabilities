module Main

open System
open Feliz
open App
open Browser.Dom
open Fable.Core.JsInterop
open Feliz.Router
open Feliz.UseElmish
open Elmish

open Auth
open Shared.Types
open Shared.Capabilities
open Database
open API


importSideEffects "./styles/global.scss"

let capabilities = 
        
        // apply the token, if present,
        // to a function which has only the token as a parameter
        let tokenToCap f token =
            token 
            |> Option.map (fun token -> 
                fun () -> f token)

        // apply the token, if present,
        // to a function which has the token and other parameters
        let tokenToCap2 f token =
            token 
            |> Option.map (fun token -> 
                fun x -> f token x)

        let getCustomerOnlyForSameId id (principal:User)  = 
            let accessToken = Authorization.onlyForSameId id principal
            accessToken |> tokenToCap CustomerDatastore.getCustomer 

        let getCustomerOnlyForAgentsInBusinessHours id (principal:User) = 
            let accessToken = Authorization.onlyForAgents id principal
            let cap1 = accessToken |> tokenToCap CustomerDatastore.getCustomer 
            let restriction f = Authorization.onlyIfDuringBusinessHours (DateTime.Now) f
            cap1 |> Authorization.restrict restriction 

        let getCustomerOnlyForSameId_OrForAgentsInBusinessHours id (principal:User) = 
            let cap1 = getCustomerOnlyForSameId id principal 
            let cap2 = getCustomerOnlyForAgentsInBusinessHours id principal 
            Authorization.first [cap1; cap2]

        let updateCustomerOnlyForSameId id principal = 
            let accessToken = Authorization.onlyForSameId id (principal:User)
            accessToken |> tokenToCap2 CustomerDatastore.updateCustomer

        let updateCustomerOnlyForAgentsInBusinessHours id principal = 
            let accessToken = Authorization.onlyForAgents id principal
            let cap1 = accessToken |> tokenToCap2 CustomerDatastore.updateCustomer
            // uncomment to get the restriction
            let restriction f = Authorization.onlyIfDuringBusinessHours (DateTime.Now) f // with restriction
            // let restriction = Some  // no restriction
            cap1 |> Authorization.restrict restriction 

        let updateCustomerOnlyForSameId_OrForAgentsInBusinessHours id (principal:User) = 
            let cap1 = updateCustomerOnlyForSameId id principal 
            let cap2 = updateCustomerOnlyForAgentsInBusinessHours id principal 
            Authorization.first [cap1; cap2]

        let updatePasswordOnlyForSameId id (principal:User) = 
            let accessToken = Authorization.passwordUpdate id principal
            let cap = accessToken |> tokenToCap2 CustomerDatastore.updatePassword
            cap 
            |> Option.map (Authorization.auditable "UpdatePassword" principal.Name) 

        // create the record that contains the capabilities
        {
            getCustomer = getCustomerOnlyForSameId_OrForAgentsInBusinessHours 
            updateCustomer = updateCustomerOnlyForSameId_OrForAgentsInBusinessHours 
            updatePassword = updatePasswordOnlyForSameId 
        }

let getAvailableCapabilities capabilityProvider customerId principal = 
    let getCustomer = capabilityProvider.getCustomer customerId principal 
    let updateCustomer = capabilityProvider.updateCustomer customerId principal 
    let updatePassword = capabilityProvider.updatePassword customerId principal 
    getCustomer,updateCustomer,updatePassword

type Msg =
    | Login of string * string
    | Logout
    | SelectCustomer of User * string
    | GetCustomerDetails of GetCustomerCap option 

type CurrentState = 
    | LoggedOut
    | LoggedIn of User
    | CustomerSelected of User * CustomerId
    | GotCustomerDetails of CustomerData
    | Exit
        
let init() = LoggedOut, Cmd.none

let update capabilityProvider msg state =
    match msg with
    | Logout ->
        LoggedOut, Cmd.none
    | Login (n,p) ->
        match Authentication.authenticate n with
        | Ok principal -> 
            LoggedIn principal, Cmd.none
        | Error err -> 
            printfn ".. %A" err
            state, Cmd.none
    | SelectCustomer (principal, customerName) ->
        match Authentication.customerIdForName customerName with
            | Ok customerId -> 
                // found -- change state
                CustomerSelected (principal,customerId), Cmd.none
            | Error err -> 
                // not found -- stay in originalState 
                printfn ".. %A" err
                state, Cmd.none
    | GetCustomerDetails getCustomerCap ->
            let r =
                match getCustomerCap with
                | Some cap ->
                    BusinessServices.getCustomer cap
                | None ->
                    failwith "Not authorized for this capability"
            match r with
            | Ok d -> 
                GotCustomerDetails d, Cmd.none
            | Error err -> 
                printfn ".. %A" err
                state, Cmd.none
            

[<ReactComponent>]
let App() =
    let state, dispatch = React.useElmish(init, update capabilities, [| |])
    match state with
    | LoggedOut ->
            Html.div [
            Html.h1 "Login"
            Html.input [
                prop.type' "text"
                prop.placeholder "Username"
                prop.value "luisfx"
            ]
            Html.input [
                prop.type' "password"
                prop.placeholder "Password"
                prop.value "123"
            ]
            Html.button [
                prop.text "Login"
                prop.onClick (fun _ -> Login ("luis", "123") |> dispatch)
            ]
        ]
    | LoggedIn principal ->
        Html.div [
            Html.h1 (sprintf "Logged in: %A" principal.Name )

            Html.span "Pick a customer"
            Html.br []
            Html.button [
                prop.text "Luis"
                prop.onClick (fun _ -> SelectCustomer (principal, "luis") |> dispatch)
            ]
            Html.br []
            Html.button [
                prop.text "maxime"
                prop.onClick (fun _ ->  SelectCustomer (principal, "maxime") |> dispatch)
            ]
            Html.br []
            Html.br []
            Html.button [
                prop.text "Logout"
                prop.onClick (fun _ -> Logout |> dispatch)
            ]
        ]
    | CustomerSelected (principal, customerId) ->
        // get the individual component capabilities from the provider
        let getCustomerCap,updateCustomerCap,updatePasswordCap = 
            getAvailableCapabilities capabilities customerId principal

        // get the text for menu options based on capabilities that are present
        let menuOptionActions = 
            [
                getCustomerCap |> Option.map (fun _ -> Html.button [ prop.text "Get"; prop.onClick (fun _ -> GetCustomerDetails getCustomerCap |> dispatch) ] )
                updateCustomerCap |> Option.map (fun _ -> Html.button [ prop.text "Update Customer"; prop.onClick (fun _ -> printfn "Update Customer") ] )
                updatePasswordCap |> Option.map (fun _ -> Html.button [ prop.text "Update Password"; prop.onClick (fun _ -> printfn "Update Password") ] )
            ] 
            |> List.choose id

        Html.div [
            Html.h1 (sprintf "Logged in: %A" principal.Name )
            Html.h2 (sprintf "Customer: %A" customerId )
            Html.br []
            Html.div menuOptionActions
            Html.br []
            Html.button [
                prop.text "Deselect Customer"
                prop.onClick (fun _ -> Login (principal.Name, "") |> dispatch)
            ]
            Html.button [
                prop.text "Logout"
                prop.onClick (fun _ -> Logout |> dispatch)
            ]
        ]
    | GotCustomerDetails d ->
        Html.div (sprintf "Got customer details: %A" d)

[<EntryPoint>]
ReactDOM.render(
    App(),
    document.getElementById "feliz-app"
)