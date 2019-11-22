import { IdentityError } from './IdentityError'

export class LoginResult {
  constructor(public token: string, public errors: IdentityError[]) {}

  static success = (token: string) => new LoginResult(token, [])
  static failed = (errors: IdentityError[]) => new LoginResult('', errors)
}
