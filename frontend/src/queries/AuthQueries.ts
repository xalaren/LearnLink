import axios from "axios";

const url = '/api/Authentication/'

export async function checkAuthorize(accessToken?: string): Promise<Response> {
    const response = (await axios.get<ValueResponse<boolean>>(`${url}status`, {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    })).data;

    return response;
}