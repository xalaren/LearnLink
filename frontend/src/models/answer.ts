import { FileInfo } from "./fileInfo";
import { UserLiteDetails } from "./userLiteDetails";

export interface Answer {
    id: number;
    objectiveId: number;
    userId: number;
    userDetails: UserLiteDetails;
    uploadDate: string;
    text?: string;
    fileDetails?: FileInfo;
    formFile?: File | null;
}