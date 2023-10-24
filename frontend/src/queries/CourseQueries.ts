import { Course } from "../models/Course.ts";
import axios, { AxiosError } from "axios";

const url = '/api/Courses/'
export async function GetPublicCoursesAsync() {
    try {
        const response = (await axios.get<ValueResponse<Course[]>>(url + "get-public")).data;
        if (!response.success) {
            throw new Error(response.message);
        }

        return response.value;
    } catch (err) {
        const error = err as AxiosError;
        if (error.response?.status === 500) {
            throw new AxiosError('Сервер недоступен');
        }

        throw new AxiosError(error.message);
    }
}