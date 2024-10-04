import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5203/api'

export interface RegisterRequest {
    email: string;
    password: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface UserDto {
    id: string;
    email: string;
    userName: string;
}

export async function register(request: RegisterRequest): Promise<UserDto> {
    const response = await axios.post(`${API_BASE_URL}/Auth/register`, request)
    return response.data
}

export async function login(request: LoginRequest): Promise<string> {
    const response = await axios.post(`${API_BASE_URL}/Auth/login`, request)
    return response.data.token
}
