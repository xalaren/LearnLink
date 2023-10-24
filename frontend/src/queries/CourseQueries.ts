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