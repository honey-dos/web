import { Account } from './Account'
import { IdentityError } from './IdentityError'
import { LoginResult } from './LoginResult'

export class RegisterResult extends LoginResult {
  constructor(
    token: string,
    errors: IdentityError[],
    public account?: Account
  ) {
    super(token, errors)
  }

  static registerSuccess = (account: Account, token: string) =>
    new RegisterResult(token, [], account)
  static failed = (errors: IdentityError[]) => new RegisterResult('', errors)
}
