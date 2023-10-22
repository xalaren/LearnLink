import {Course} from "../models/Course.ts";
import axios from "axios";

const url = '/api/Courses/'
export async function GetPublicCoursesAsync() {
    try {
        const response = (await axios.get<ValueResponse<Course[]>>(url + "get-public")).data;
        if(!response.success) {
            throw new Error(response.message);
        }

        return response.value;
    }
    catch(error: unknown) {
        throw new Error((error as Error).message);
    }
}