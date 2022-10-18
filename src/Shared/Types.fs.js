import { Union, Record } from "../App/src/fable_modules/fable-library.3.7.17/Types.js";
import { union_type, int32_type, record_type, array_type, string_type } from "../App/src/fable_modules/fable-library.3.7.17/Reflection.js";

export class User extends Record {
    constructor(Name, Roles) {
        super();
        this.Name = Name;
        this.Roles = Roles;
    }
}

export function User$reflection() {
    return record_type("Shared.Types.User", [], User, () => [["Name", string_type], ["Roles", array_type(string_type)]]);
}

export class CustomerId extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["CustomerId"];
    }
}

export function CustomerId$reflection() {
    return union_type("Shared.Types.CustomerId", [], CustomerId, () => [[["Item", int32_type]]]);
}

export class CustomerData extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["CustomerData"];
    }
}

export function CustomerData$reflection() {
    return union_type("Shared.Types.CustomerData", [], CustomerData, () => [[["Item", string_type]]]);
}

export class Password extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Password"];
    }
}

export function Password$reflection() {
    return union_type("Shared.Types.Password", [], Password, () => [[["Item", string_type]]]);
}

export class FailureCase extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["AuthenticationFailed", "AuthorizationFailed", "CustomerNameNotFound", "CustomerIdNotFound", "OnlyAllowedOnce", "CapabilityRevoked"];
    }
}

export function FailureCase$reflection() {
    return union_type("Shared.Types.FailureCase", [], FailureCase, () => [[["Item", string_type]], [], [["Item", string_type]], [["Item", CustomerId$reflection()]], [], []]);
}

//# sourceMappingURL=Types.fs.js.map
