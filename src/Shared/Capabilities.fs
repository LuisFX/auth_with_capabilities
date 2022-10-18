namespace Shared

module Capabilities =
    open Types

    // each access token gets its own type
    type AccessCustomer = AccessCustomer of CustomerId
    type UpdatePassword = UpdatePassword of CustomerId

    // capabilities
    type GetCustomerCap = unit -> Result<CustomerData,FailureCase>
    type UpdateCustomerCap = CustomerData -> Result<unit,FailureCase>
    type UpdatePasswordCap = Password -> Result<unit,FailureCase>

    type CapabilityProvider = {
            /// given a customerId and User, attempt to get the GetCustomer capability
            getCustomer : CustomerId -> User -> GetCustomerCap option
            /// given a customerId and User, attempt to get the UpdateCustomer capability
            updateCustomer : CustomerId -> User -> UpdateCustomerCap option
            /// given a customerId and User, attempt to get the UpdatePassword capability
            updatePassword : CustomerId -> User -> UpdatePasswordCap option 
        }