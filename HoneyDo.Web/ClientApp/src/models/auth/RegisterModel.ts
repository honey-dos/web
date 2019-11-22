import { LoginModel } from './LoginModel'

export class RegisterModel extends LoginModel {
  picture?: string
  constructor(
    public name: string,
    public userName: string,
    public provider: string,
    public accessToken: string
  ) {
    super(provider, accessToken)
  }
}
