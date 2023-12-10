export interface ILinkData {
    title: string;
    path: string;
}

export interface IDropdownData {
    title: string;
    onClick: () => void;
    iconPath?: string;
}