import { FSharpResult$2 } from "../App/src/fable_modules/fable-library.3.7.17/Choice.js";
import { printf, toConsole } from "../App/src/fable_modules/fable-library.3.7.17/String.js";

export function getCustomer(capability) {
    const matchValue = capability();
    if (matchValue.tag === 1) {
        const err = matchValue.fields[0];
        return new FSharpResult$2(1, err);
    }
    else {
        const data = matchValue.fields[0];
        return new FSharpResult$2(0, data);
    }
}

export function updateCustomer(capability) {
    toConsole(printf("Enter new data: "));
    const matchValue = capability("customerData");
    if (matchValue.tag === 1) {
        const err = matchValue.fields[0];
        toConsole(printf(".. %A"))(err);
    }
    else {
        toConsole(printf("Data updated"));
    }
}

export function updatePassword(capability) {
    toConsole(printf("Enter new password: "));
    const matchValue = capability("password");
    if (matchValue.tag === 1) {
        const err = matchValue.fields[0];
        toConsole(printf(".. %A"))(err);
    }
    else {
        toConsole(printf("Password updated"));
    }
}

//# sourceMappingURL=BusinessServices.fs.js.map
