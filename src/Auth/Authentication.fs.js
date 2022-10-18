import { CustomerId, FailureCase, User } from "../Shared/Types.fs.js";
import { Result_Map, FSharpResult$2 } from "../App/src/fable_modules/fable-library.3.7.17/Choice.js";
import { equals } from "../App/src/fable_modules/fable-library.3.7.17/Util.js";

export const customerRole = "Customer";

export const customerAgentRole = "CustomerAgent";

export function makePrincipal(name, role) {
    return new User(name, [role]);
}

export function authenticate(name) {
    switch (name) {
        case "luis":
        case "maxime": {
            return new FSharpResult$2(0, makePrincipal(name, customerRole));
        }
        case "sean": {
            return new FSharpResult$2(0, makePrincipal(name, customerAgentRole));
        }
        default: {
            return new FSharpResult$2(1, new FailureCase(0, name));
        }
    }
}

export function customerIdForName(name) {
    switch (name) {
        case "luis": {
            return new FSharpResult$2(0, new CustomerId(0, 1));
        }
        case "maxime": {
            return new FSharpResult$2(0, new CustomerId(0, 2));
        }
        default: {
            return new FSharpResult$2(1, new FailureCase(2, name));
        }
    }
}

export function orElse(errValue, _arg) {
    if (_arg.tag === 1) {
        return errValue;
    }
    else {
        const x = _arg.fields[0];
        return x;
    }
}

export function customerIdOwnedByPrincipal(customerId, principal) {
    return orElse(false, Result_Map((principalId) => equals(principalId, customerId), customerIdForName(principal.Name)));
}

//# sourceMappingURL=Authentication.fs.js.map
