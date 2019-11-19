import { AuthResult } from './AuthResult'

export class IdentityError {
  constructor(public code: string, public description = '') {}

  static providerTokenFailed(): IdentityError {
    return new IdentityError(AuthResult.ProviderTokenFailed)
  }

  static invalidAccessToken(): IdentityError {
    return new IdentityError(AuthResult.InvalidAccessToken)
  }

  static firebaseAuthError(): IdentityError {
    return new IdentityError(
      AuthResult.FirebaseAuthError,
      'An error occurred in our configuration, please try again later.'
    )
  }

  static firebaseCancel(): IdentityError {
    return new IdentityError(
      AuthResult.FirebaseCancel,
      'The pop-up login was closed.'
    )
  }

  static firebaseUserError(): IdentityError {
    return new IdentityError(
      AuthResult.FirebaseUserError,
      'An error occurred while verifying your identity with an external Provider (e.g. Google), please try again later.'
    )
  }

  static registrationFailed(): IdentityError {
    return new IdentityError(
      AuthResult.RegistrationFailed,
      'An error occurred while trying to create your account, please try again later.'
    )
  }
}
