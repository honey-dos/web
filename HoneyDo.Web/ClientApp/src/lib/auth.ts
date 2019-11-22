import firebase from 'firebase/app'
import 'firebase/auth'
import { AccessTokenResult } from '../models/auth/AccessTokenResult'
import { Account } from '../models/auth/Account'
import { FirebaseConfig } from '../models/auth/FirebaseConfig'
import { IdentityError } from '../models/auth/IdentityError'
import { LoginModel } from '../models/auth/LoginModel'
import { LoginResult } from '../models/auth/LoginResult'
import { RegisterModel } from '../models/auth/RegisterModel'
import { RegisterResult } from '../models/auth/RegisterResult'
import { TokenModel } from '../models/auth/TokenModel'
import { TokenResult } from '../models/auth/TokenResult'

export const login = async (): Promise<LoginResult> => {
  let accessTokenResult: AccessTokenResult
  try {
    accessTokenResult = await getProviderToken()
  } catch (error) {
    return LoginResult.failed([])
  }
  if (accessTokenResult.errors.length) {
    return LoginResult.failed(accessTokenResult.errors)
  }
  const loginModel = new LoginModel('GOOGLE', accessTokenResult.accessToken)

  let tokenResult: TokenResult
  try {
    tokenResult = await fetchToken(loginModel)
  } catch (error) {
    return RegisterResult.failed([])
  }

  return LoginResult.success(tokenResult.token)
}

export const registerAccount = async (
  name: string,
  userName: string
): Promise<RegisterResult> => {
  let accessTokenResult: AccessTokenResult
  try {
    accessTokenResult = await getProviderToken()
  } catch (error) {
    return RegisterResult.failed([])
  }
  if (accessTokenResult.errors.length) {
    return RegisterResult.failed(accessTokenResult.errors)
  }

  const url = `api/account/register`
  const registerModel = new RegisterModel(
    name,
    userName,
    'GOOGLE',
    accessTokenResult.accessToken
  )

  const registerRequest = await fetch(url, {
    method: 'POST',
    mode: 'cors',
    cache: 'no-cache',
    headers: {
      'Content-Type': 'application/json; charset=utf-8'
    },
    body: JSON.stringify(registerModel)
  })

  if (registerRequest.status === 400) {
    const rawErrors = await registerRequest.json()
    const errors = rawErrors as IdentityError[]
    return RegisterResult.failed(errors)
  } else if (registerRequest.status !== 201) {
    return RegisterResult.failed([IdentityError.registrationFailed()])
  }

  const account = (await registerRequest.json()) as Account

  // fetch token
  let tokenResult: TokenResult
  try {
    tokenResult = await fetchToken(registerModel)
  } catch (error) {
    return RegisterResult.failed([])
  }

  return RegisterResult.registerSuccess(account, tokenResult.token)
}

const fetchToken = async (model: LoginModel): Promise<TokenResult> => {
  if (!model.accessToken.length) {
    return TokenResult.failed([IdentityError.invalidAccessToken()])
  }

  const url = 'api/auth'
  const tokenRequest = await fetch(url, {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json; charset=utf-8'
    },
    body: JSON.stringify(model)
  })

  if (tokenRequest.status !== 200) {
    const errors = (await tokenRequest.json()) as IdentityError[]
    return TokenResult.failed(errors)
  }

  const tokenModel = (await tokenRequest.json()) as TokenModel
  return TokenResult.success(tokenModel.token)
}

const getProviderToken = async (): Promise<AccessTokenResult> => {
  const firebaseApps = firebase.apps
  if (!firebaseApps.length) {
    // get firebase config
    const url = 'api/auth/firebase-config'
    const configRequest = await fetch(url, {
      method: 'GET',
      mode: 'cors',
      cache: 'no-cache'
    })

    if (configRequest.status !== 200) {
      return AccessTokenResult.failed([IdentityError.firebaseAuthError()])
    }

    const config = (await configRequest.json()) as FirebaseConfig
    firebase.initializeApp(config)
  }

  const provider = new firebase.auth.GoogleAuthProvider()
  provider.addScope('https://www.googleapis.com/auth/contacts.readonly')
  const firebaseAuth = firebase.auth()
  if (!firebaseAuth) {
    return AccessTokenResult.failed([IdentityError.firebaseAuthError()])
  }

  firebaseAuth.useDeviceLanguage()

  try {
    await firebaseAuth.signInWithPopup(provider)
    if (!firebaseAuth.currentUser) {
      return AccessTokenResult.failed([IdentityError.firebaseUserError()])
    }
    const accessToken = await firebaseAuth.currentUser.getIdToken(false)
    return AccessTokenResult.succeeded(accessToken)
  } catch (error) {
    return AccessTokenResult.failed([IdentityError.firebaseCancel()])
  }
}
