export interface User {
  id: number
  username: string
  email: string
  role: string
  createdAt?: string
}

export interface AuthResponse {
  accessToken: string
  refreshToken: string
  accessTokenExpiresAt: string
  refreshTokenExpiresAt: string
  user: User
}
