import type { AxiosError, InternalAxiosRequestConfig } from 'axios'
import type { AuthResponse, BaseApiResponse } from '@/types'
import axios from 'axios'
import { clearAuthTokens, getAccessToken, getRefreshToken, setAuthTokens } from '@/lib/cookies'
import { CaseConverter } from '@/lib/utils'

interface CustomAxiosRequestConfig extends InternalAxiosRequestConfig {
  _retry?: boolean
}

let isRefreshing = false
let failedQueue: Array<{
  resolve: (value?: unknown) => void
  reject: (reason?: unknown) => void
}> = []

function processQueue(error: unknown, token: string | null = null) {
  failedQueue.forEach((promise) => {
    if (error) {
      promise.reject(error)
    }
    else {
      promise.resolve(token)
    }
  })
  failedQueue = []
}

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

apiClient.interceptors.request.use((config) => {
  const token = getAccessToken()
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  if (config.data) {
    config.data = CaseConverter.toSnakeCase(config.data)
  }
  if (config.params) {
    config.params = CaseConverter.toSnakeCase(config.params)
  }
  return config
})

apiClient.interceptors.response.use(
  (response) => {
    if (response.data) {
      response.data = CaseConverter.toCamelCase(response.data)
    }
    return response
  },
  async (error: AxiosError<{ message?: string }>) => {
    const originalRequest = error.config as CustomAxiosRequestConfig | undefined
    const status = error.response?.status
    const message = error.response?.data?.message ?? error.message

    if (
      status === 401
      && originalRequest
      && !originalRequest._retry
      && !originalRequest.url?.includes('/auth/login')
      && !originalRequest.url?.includes('/auth/register')
      && !originalRequest.url?.includes('/auth/refresh')
    ) {
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject })
        })
          .then((token) => {
            if (originalRequest.headers && token) {
              originalRequest.headers.Authorization = `Bearer ${token}`
            }
            return apiClient(originalRequest)
          })
          .catch(err => Promise.reject(err))
      }

      originalRequest._retry = true
      isRefreshing = true

      const currentRefreshToken = getRefreshToken()
      if (!currentRefreshToken) {
        clearAuthTokens()
        return Promise.reject(new Error(message))
      }

      try {
        const response = await axios.post<BaseApiResponse<unknown>>(
          `${import.meta.env.VITE_API_BASE_URL}/auth/refresh`,
          { refresh_token: currentRefreshToken },
          { headers: { 'Content-Type': 'application/json' } },
        )

        const authData: AuthResponse = CaseConverter.toCamelCase(response.data.data)
        setAuthTokens(authData)

        processQueue(null, authData.accessToken)

        if (originalRequest.headers) {
          originalRequest.headers.Authorization = `Bearer ${authData.accessToken}`
        }

        return apiClient(originalRequest)
      }
      catch (refreshError) {
        processQueue(refreshError, null)
        clearAuthTokens()
        return Promise.reject(refreshError)
      }
      finally {
        isRefreshing = false
      }
    }

    return Promise.reject(new Error(message))
  },
)
