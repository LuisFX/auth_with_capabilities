module Domain

open System

type CustomerId = CustomerId of int
type CustomerData = CustomerData of string
type Password = Password of string

type FailureCase = 
    | AuthenticationFailed of string
    | AuthorizationFailed
    | CustomerNameNotFound of string
    | CustomerIdNotFound of CustomerId
    | OnlyAllowedOnce
    | CapabilityRevoked

let orElse errValue = function
    | Ok x -> x
    | Error _ -> errValue