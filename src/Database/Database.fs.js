import { Dictionary } from "../App/src/fable_modules/fable-library.3.7.17/MutableMap.js";
import { safeHash, equals } from "../App/src/fable_modules/fable-library.3.7.17/Util.js";
import { AccessToken$1__get_Data } from "../Auth/Authorization.fs.js";
import { tryGetValue } from "../App/src/fable_modules/fable-library.3.7.17/MapUtil.js";
import { FSharpRef } from "../App/src/fable_modules/fable-library.3.7.17/Types.js";
import { FSharpResult$2 } from "../App/src/fable_modules/fable-library.3.7.17/Choice.js";
import { FailureCase } from "../Shared/Types.fs.js";

const db = new Dictionary([], {
    Equals: equals,
    GetHashCode: safeHash,
});

export function getCustomer(accessToken) {
    const id = AccessToken$1__get_Data(accessToken).fields[0];
    let matchValue;
    let outArg = null;
    matchValue = [tryGetValue(db, id, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        const value = matchValue[1];
        return new FSharpResult$2(0, value);
    }
    else {
        return new FSharpResult$2(1, new FailureCase(3, id));
    }
}

export function updateCustomer(accessToken, data) {
    const id = AccessToken$1__get_Data(accessToken).fields[0];
    db.set(id, data);
    return new FSharpResult$2(0, void 0);
}

export function updatePassword(accessToken, password) {
    return new FSharpResult$2(0, void 0);
}

//# sourceMappingURL=Database.fs.js.map