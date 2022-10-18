namespace API

module BusinessServices =
    open Shared.Types

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