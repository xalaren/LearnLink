import axios from 'axios';


const axiosInstance = axios.create({
    baseURL: 'http://localhost:5174', // Замените на ваш URL
    timeout: 5000, // Настройка таймаута запроса
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*', // Разрешает доступ со всех источников
        // Другие необходимые заголовки могут быть добавлены здесь
    },
    timeoutErrorMessage: 'Превышено время ожидания',
});

axiosInstance.interceptors.response.use(
    response => {
        return response;
    },
    error => {
        if (error.response && error.response.status === 500) {
            const customError = new Error('Сервер недоступен');
            return Promise.reject(customError);
        }
        if (error.response && error.response.status === 404) {
            const customError = new Error('Запрос не найден');
            return Promise.reject(customError);
        }
        if (error.response && error.response.status === 401) {
            const customError = new Error('Вы не авторизованы, либо не имеете доступ');
            return Promise.reject(customError);
        }
        return Promise.reject(error);
    }
);

export default axiosInstance;