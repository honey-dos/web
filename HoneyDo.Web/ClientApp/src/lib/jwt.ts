import jwtDecodeToken from 'jwt-decode'

export const setToken = (token: string) => {
  localStorage.setItem('token', token)
}

export const getToken = (): string | null => {
  return localStorage.getItem('token')
}

export const logout = (): void => {
  setToken('')
}

export const decodeToken = (token: string): { [key: string]: any } => {
  return jwtDecodeToken(token)
}

export const getTokenData = (): JwtData | null => {
  const token = getToken()
  if (!token || token.length === 0) {
    return null
  }
  return new JwtData(token)
}

export class JwtData {
  public readonly id = ''
  public readonly name = ''
  public readonly token: string = ''
  public readonly expires: Date
  constructor(token: string) {
    const decoded = decodeToken(token)
    this.id = decoded.jti
    this.name = decoded.sub
    this.token = token
    this.expires = new Date(decoded.exp * 1000)
  }
  isExpired(): boolean {
    return this.expires !== undefined && this.expires <= new Date()
  }
}
