import axios from 'axios'
import { DEFAULT_PAGE, DEFAULT_PAGE_SIZE } from '../constants'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5203/api'

// Create an axios instance
const api = axios.create({
  baseURL: API_BASE_URL
})

// Add a request interceptor
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export interface ShortenedUrl {
    id: string;
    originalUrl: string;
    shortUrlId: string;
    expiryDate: string;
    accessCount: number;
}

export interface PaginatedResponse<T> {
    urls: T[];
    totalCount: number;
    currentPage: number;
    pageSize: number;
}

export interface ShortenUrlRequest {
    originalUrl: string;
    customShortId?: string;
    ttl?: number;
}

export async function shortenUrl(request: ShortenUrlRequest): Promise<ShortenedUrl> {
    const response = await api.post(`${API_BASE_URL}/Urls`, request)
    console.log("response======>>>>>>", response.data)
    return response.data
}

export async function getRecentUrls(page: number = DEFAULT_PAGE, pageSize: number = DEFAULT_PAGE_SIZE): Promise<PaginatedResponse<ShortenedUrl>> {
    try {
        const response = await api.get(`${API_BASE_URL}/Urls`, {
            params: { page, pageSize }
        })
        console.log('Recent URLs response:', response.data)
        return response.data
    } catch (error) {
        console.error('Error fetching recent URLs:', error)
        throw error
    }
}
