import { UserLiteDetails } from "./userLiteDetails";

export interface Review {
    id: number;
    grade: number;
    expertUserId: number;
    expertUserDetails?: UserLiteDetails;
    comment?: string;
    reviewDate: string;
}