namespace API

open System
open Database
open Shared.Types
open Auth
open Shared.Capabilities

module BusinessServices =

    // use the getCustomer capability
    let getCustomer capability =
        match capability() with
        | Ok data -> Ok data
        | Error err -> Error err

    // use the updateCustomer capability
    let updateCustomer capability =
        printfn "Enter new data: "
        match capability "customerData"  with
        | Ok _ -> printfn "Data updated" 
        | Error err -> printfn ".. %A" err

    // use the updatePassword capability
    let updatePassword capability =
        printfn "Enter new password: "
        match capability "password"  with
        | Ok _ -> printfn "Password updated" 
        | Error err -> printfn ".. %A" err

module Capabilities =
    let allCapabilities = 
        
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
            // MAXIME: The code below would fail to compile, because the AccessToken is of a different type, as expected.
            // match accessToken with
            // | Some token ->
            //     Some (fun password -> CustomerDatastore.updatePassword token password)
            // | None -> None
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

    let getAllCapabilities customerId principal = 
        let getCustomer = allCapabilities.getCustomer customerId principal 
        let updateCustomer = allCapabilities.updateCustomer customerId principal 
        let updatePassword = allCapabilities.updatePassword customerId principal 
        getCustomer, updateCustomer , updatePassword