import { IdentityError } from './IdentityError'

export class AccessTokenResult {
  constructor(public accessToken = '', public errors: IdentityError[] = []) {}
  static failed(errors: IdentityError[]): AccessTokenResult {
    return new AccessTokenResult(undefined, errors)
  }
  static succeeded(accessToken: string): AccessTokenResult {
    return new AccessTokenResult(accessToken)
  }
}
