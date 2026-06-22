// Interop helper for dynamically calling functions from user-provided modules
// This allows the library to not hardcode module paths

const moduleCache = new Map();

export async function importModule(modulePath) {
    if (!moduleCache.has(modulePath)) {
        const module = await import(modulePath);
        moduleCache.set(modulePath, module);
    }
    return moduleCache.get(modulePath);
}

export function callModuleFunction(module, funcName, arg1, arg2) {
    const func = module[funcName];
    if (typeof func !== 'function') {
        console.warn(`Function '${funcName}' not found in module`);
        return "";
    }
    // Handle both 1-arg and 2-arg callbacks
    if (arg2 === undefined) {
        return func(arg1) ?? "";
    }
    return func(arg1, arg2) ?? "";
}
