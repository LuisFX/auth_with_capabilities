import { FSharpRef, Record } from "../App/src/fable_modules/fable-library.3.7.17/Types.js";
import { record_type } from "../App/src/fable_modules/fable-library.3.7.17/Reflection.js";
import { customerAgentRole, customerIdOwnedByPrincipal } from "./Authentication.fs.js";
import { UpdatePassword, AccessCustomer } from "../Shared/Capabilities.fs.js";
import { contains } from "../App/src/fable_modules/fable-library.3.7.17/Array.js";
import { stringHash } from "../App/src/fable_modules/fable-library.3.7.17/Util.js";
import { toString, utcNow, hour } from "../App/src/fable_modules/fable-library.3.7.17/Date.js";
import { bind, some } from "../App/src/fable_modules/fable-library.3.7.17/Option.js";
import { tryPick } from "../App/src/fable_modules/fable-library.3.7.17/List.js";
import { printf, toConsole } from "../App/src/fable_modules/fable-library.3.7.17/String.js";
import { FailureCase } from "../Shared/Types.fs.js";
import { FSharpResult$2 } from "../App/src/fable_modules/fable-library.3.7.17/Choice.js";

export class AccessToken$1 extends Record {
    constructor(data) {
        super();
        this.data = data;
    }
}

export function AccessToken$1$reflection(gen0) {
    return record_type("Auth.Authorization.AccessToken`1", [gen0], AccessToken$1, () => [["data", gen0]]);
}

export function AccessToken$1__get_Data(this$) {
    return this$.data;
}

export function onlyForSameId(id, principal) {
    if (customerIdOwnedByPrincipal(id, principal)) {
        return new AccessToken$1(new AccessCustomer(0, id));
    }
    else {
        return void 0;
    }
}

export function onlyForAgents(id, principal) {
    if (contains(customerAgentRole, principal.Roles, {
        Equals: (x, y) => (x === y),
        GetHashCode: stringHash,
    })) {
        return new AccessToken$1(new AccessCustomer(0, id));
    }
    else {
        return void 0;
    }
}

export function onlyIfDuringBusinessHours(time, f) {
    if ((hour(time) >= 8) && (hour(time) <= 17)) {
        return some(f);
    }
    else {
        return void 0;
    }
}

export function passwordUpdate(id, principal) {
    if (customerIdOwnedByPrincipal(id, principal)) {
        return new AccessToken$1(new UpdatePassword(0, id));
    }
    else {
        return void 0;
    }
}

export function first(capabilityList) {
    return tryPick((x) => x, capabilityList);
}

export function restrict(filter, originalCap) {
    return bind(filter, originalCap);
}

export function auditable(capabilityName, principalName, f, x) {
    let timestamp;
    let copyOfStruct = utcNow();
    timestamp = toString(copyOfStruct, "u");
    toConsole(printf("AUDIT: User %s used capability %s at %s"))(principalName)(capabilityName)(timestamp);
    return f(x);
}

export function revokable(f) {
    const allow = new FSharpRef(true);
    const capability = (x) => {
        if (allow.contents) {
            return f(x);
        }
        else {
            return new FSharpResult$2(1, new FailureCase(5));
        }
    };
    const revoker = () => {
        allow.contents = false;
    };
    return [capability, revoker];
}

//# sourceMappingURL=Authorization.fs.js.map
