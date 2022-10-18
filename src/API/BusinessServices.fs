namespace API

module BusinessServices =
    open Domain

    // use the getCustomer capability
    let getCustomer capability =
        match capability() with
        | Ok data -> printfn "%A" data
        | Error err -> printfn ".. %A" err

    // use the updateCustomer capability
    let updateCustomer capability =
        printfn "Enter new data: "
        let customerData = Console.ReadLine() |> CustomerData
        match capability customerData  with
        | Ok _ -> printfn "Data updated" 
        | Error err -> printfn ".. %A" err

    // use the updatePassword capability
    let updatePassword capability =
        printfn "Enter new password: "
        let password = Console.ReadLine() |> Password
        match capability password  with
        | Ok _ -> printfn "Password updated" 
        | Error err -> printfn ".. %A" err