import axios from 'axios';


const axiosInstance = axios.create({
    baseURL: 'http://localhost:5174', // Замените на ваш URL
    timeout: 5000, // Настройка таймаута запроса
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*', // Разрешает доступ со всех источников
        // Другие необходимые заголовки могут быть добавлены здесь
    },
});

export default axiosInstance;