import { Record, Union } from "../App/src/fable_modules/fable-library.3.7.17/Types.js";
import { Password$reflection, FailureCase$reflection, CustomerData$reflection, User$reflection, CustomerId$reflection } from "./Types.fs.js";
import { record_type, option_type, lambda_type, unit_type, union_type } from "../App/src/fable_modules/fable-library.3.7.17/Reflection.js";
import { FSharpResult$2 } from "../App/src/fable_modules/fable-library.3.7.17/Choice.js";

export class AccessCustomer extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["AccessCustomer"];
    }
}

export function AccessCustomer$reflection() {
    return union_type("Shared.Capabilities.AccessCustomer", [], AccessCustomer, () => [[["Item", CustomerId$reflection()]]]);
}

export class UpdatePassword extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["UpdatePassword"];
    }
}

export function UpdatePassword$reflection() {
    return union_type("Shared.Capabilities.UpdatePassword", [], UpdatePassword, () => [[["Item", CustomerId$reflection()]]]);
}

export class CapabilityProvider extends Record {
    constructor(getCustomer, updateCustomer, updatePassword) {
        super();
        this.getCustomer = getCustomer;
        this.updateCustomer = updateCustomer;
        this.updatePassword = updatePassword;
    }
}

export function CapabilityProvider$reflection() {
    return record_type("Shared.Capabilities.CapabilityProvider", [], CapabilityProvider, () => [["getCustomer", lambda_type(CustomerId$reflection(), lambda_type(User$reflection(), option_type(lambda_type(unit_type, union_type("Microsoft.FSharp.Core.FSharpResult`2", [CustomerData$reflection(), FailureCase$reflection()], FSharpResult$2, () => [[["ResultValue", CustomerData$reflection()]], [["ErrorValue", FailureCase$reflection()]]])))))], ["updateCustomer", lambda_type(CustomerId$reflection(), lambda_type(User$reflection(), option_type(lambda_type(CustomerData$reflection(), union_type("Microsoft.FSharp.Core.FSharpResult`2", [unit_type, FailureCase$reflection()], FSharpResult$2, () => [[["ResultValue", unit_type]], [["ErrorValue", FailureCase$reflection()]]])))))], ["updatePassword", lambda_type(CustomerId$reflection(), lambda_type(User$reflection(), option_type(lambda_type(Password$reflection(), union_type("Microsoft.FSharp.Core.FSharpResult`2", [unit_type, FailureCase$reflection()], FSharpResult$2, () => [[["ResultValue", unit_type]], [["ErrorValue", FailureCase$reflection()]]])))))]]);
}

//# sourceMappingURL=Capabilities.fs.js.map
