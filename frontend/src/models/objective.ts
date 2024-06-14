export interface Objective {
    id: number;
    title: string;
    text: string;
    formFile: File | null;
    fileContentId: number;
    fileUrl?: string;
    fileName?: string;
    fileExtension?: string;
}