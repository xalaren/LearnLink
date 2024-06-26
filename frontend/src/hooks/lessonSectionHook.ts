import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueResponse, VoidResponse } from "../models/response";
import { Section, SectionCodeContent, SectionFileContent, SectionTextContent } from "../models/section";
import { LESSON_SECTIONS_ENDPOINTS_URL } from "../models/constants";
import { AxiosError, AxiosResponse } from "axios";

export function useGetLessonSections() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const getSectionsFromLesson = async (lessonId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<Section[]>>(`${LESSON_SECTIONS_ENDPOINTS_URL}get/fromlesson?lessonId=${lessonId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setLoading(false);
            return (response.data.value);
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setLoading(false);
    }

    return { getSectionsFromLesson, error, loading, resetValues };
}

export function useCreateSection() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const createSectionQuery = async (lessonId: number, section: Section, accessToken: string) => {
        try {
            setLoading(true);

            let response: AxiosResponse<VoidResponse, any>;

            if (section.content.isText) {
                const textContent: SectionTextContent = {
                    id: section.id,
                    order: section.order,
                    text: section.content.text || '',
                    title: section.title
                };

                response = await axiosInstance.post<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}create/text?lessonId=${lessonId}`, textContent, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        Authorization: `Bearer ${accessToken}`,
                    }
                });
            }
            else if (section.content.isCodeBlock) {
                const codeContent: SectionCodeContent = {
                    id: section.id,
                    order: section.order,
                    code: section.content.text || '',
                    codeLanguage: section.content.codeLanguage || '',
                    title: section.title
                };

                response = await axiosInstance.post<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}create/code?lessonId=${lessonId}`, codeContent, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        Authorization: `Bearer ${accessToken}`,
                    }
                });
            }
            else if (section.content.isFile) {
                const fileContent: SectionFileContent = {
                    id: section.id,
                    order: section.order,
                    formFile: section.content.formFile!,
                    title: section.title
                };

                response = await axiosInstance.post<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}create/file?lessonId=${lessonId}`, fileContent, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        Authorization: `Bearer ${accessToken}`,
                    }
                });

            }
            else {
                throw new AxiosError("Неверный формат контента");
            }

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
            setLoading(false);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
        setLoading(false);
    }

    return { createSectionQuery, error, loading, success, resetValues };
}

export function useUpdateSection() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const updateSectionQuery = async (lessonId: number, section: Section, accessToken: string) => {
        try {
            setLoading(true);

            let response: AxiosResponse<VoidResponse, any>;

            if (section.content.isText) {
                const textContent: SectionTextContent = {
                    id: section.id,
                    order: section.order,
                    text: section.content.text || '',
                    title: section.title
                };

                response = await axiosInstance.post<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}update/text?lessonId=${lessonId}`, textContent, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        Authorization: `Bearer ${accessToken}`,
                    }
                });
            }
            else if (section.content.isCodeBlock) {
                const codeContent: SectionCodeContent = {
                    id: section.id,
                    order: section.order,
                    code: section.content.text || '',
                    codeLanguage: section.content.codeLanguage || '',
                    title: section.title
                };

                response = await axiosInstance.post<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}update/code?lessonId=${lessonId}`, codeContent, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        Authorization: `Bearer ${accessToken}`,
                    }
                });
            }
            else if (section.content.isFile && section.content.formFile) {
                const fileContent: SectionFileContent = {
                    id: section.id,
                    order: section.order,
                    formFile: section.content.formFile,
                    title: section.title
                };

                response = await axiosInstance.post<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}update/file?lessonId=${lessonId}`, fileContent, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        Authorization: `Bearer ${accessToken}`,
                    }
                });
            }
            else {
                response = await axiosInstance.post<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}update?lessonId=${lessonId}`, section, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        Authorization: `Bearer ${accessToken}`,
                    }
                });
            }

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
            setLoading(false);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
        setLoading(false);
    }

    return { updateSectionQuery, error, loading, success, resetValues };
}

export function useChangeSectionOrder() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const changeSectionOrder = async (sectionId: number, lessonId: number, increase: boolean, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}update/order?sectionId=${sectionId}&lessonId=${lessonId}&increase=${increase}`, {}, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                }
            });
            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
            setLoading(false);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
        setLoading(false);
    }

    return { changeSectionOrder, error, success, loading, resetValues };
}

export function useRemoveSection() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const sectionRemoveQuery = async (lessonId: number, sectionId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.delete<VoidResponse>(`${LESSON_SECTIONS_ENDPOINTS_URL}remove?lessonId=${lessonId}&sectionId=${sectionId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                }
            });
            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
            setLoading(false);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
        setLoading(false);
    }

    return { sectionRemoveQuery, error, success, loading, resetValues };
}