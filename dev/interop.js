export function supportsFileSystemWrites() {
	try {
		return 'FileSystemHandle' in globalThis && 
				'requestPermission' in globalThis.FileSystemHandle.prototype;
	} catch {
		return false;
	}
}
