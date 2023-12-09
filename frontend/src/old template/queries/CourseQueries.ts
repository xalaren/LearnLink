import { Course } from "../models/Course.ts";
import axios, {AxiosError} from "axios";
import {handleInternalError} from "./InternalErrorHandler.ts";

const url = '/api/Courses/'
export async function GetPublicCoursesAsync(): Promise<Course[] | undefined> {
    let response: ValueResponse<Course[]>;
    try {
        response = (await axios.get<ValueResponse<Course[]>>(url + "get-public")).data;

        if (!response.success) {
            throw new AxiosError(response.message);
        }

        return response.value;
    }
    catch(e: unknown) {
        handleInternalError(e);
    }
}

export async function GetUserCourses(userId: number, accessToken: string): Promise<Course[] | undefined> {
    try {
        const response = (await axios.get<ValueResponse<Course[]>>(`${url}get-user-courses?&userId=${userId}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
            })).data;

        if (!response.success) {
            throw new AxiosError(response.message);
        }

        return response.value;
    }
    catch(e: unknown) {
        handleInternalError(e);
    }
}

export async function GetSubscribedCourses(userId: number, accessToken: string): Promise<Course[] | undefined> {
    try {
        const response = (await axios.get<ValueResponse<Course[]>>(`${url}get-subscribed?$userId=${userId}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        })).data;

        if (!response.success) {
            throw new AxiosError(response.message);
        }

        return response.value;
    }
    catch(e: unknown) {
        handleInternalError(e);
    }
}''

