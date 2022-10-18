namespace Database

module CustomerDatastore = 
    open System.Collections.Generic
    open Shared.Types 
    open Shared.Capabilities
    open Auth.Authorization

    let private db = Dictionary<CustomerId,CustomerData>()

    let getCustomer (accessToken:AccessToken<AccessCustomer>) = 
        // get customer id
        let (AccessCustomer id) = accessToken.Data

        // now get customer data using the id
        match db.TryGetValue id with
        | true, value -> Ok value 
        | false, _ -> Error (CustomerIdNotFound id)

    let updateCustomer (accessToken:AccessToken<AccessCustomer>) (data:CustomerData) = 
        // get customer id
        let (AccessCustomer id) = accessToken.Data

        // update database
        db.[id] <- data
        Ok()

    let updatePassword (accessToken:AccessToken<UpdatePassword>) (password:Password) = 
        Ok()   // dummy implementation