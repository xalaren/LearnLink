const accessKey: string = "accessKey";

export function getAccessToken(): string | null {
    return localStorage.getItem(accessKey);
}

export function setAccessToken(accessToken: string): void {
    clearToken();
    localStorage.setItem(accessKey, accessToken);
}

export function clearToken(): void {
    localStorage.clear();
}