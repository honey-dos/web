import React, { Component } from 'react'
import { UserContext } from './UserContext'
import { JwtData } from '../lib/jwt'

export interface IFetch {
  (input: RequestInfo, init?: RequestInit): Promise<Response>
}

export interface FetchContextData {
  fetch: IFetch
}

export const FetchContext = React.createContext<FetchContextData>({
  fetch: fetch
})

const { Provider, Consumer } = FetchContext

export class FetchProvider extends Component {
  static contextType = UserContext

  appFetch = (): IFetch => (
    input: RequestInfo,
    init?: RequestInit
  ): Promise<Response> => {
    const { jwtData }: { jwtData: JwtData } = this.context
    if (!init) {
      init = {}
    }
    init.headers = {
      ...init.headers,
      Authorization: `Bearer ${jwtData.token}`
    }

    return fetch(input, init)
  }

  render() {
    const { children } = this.props

    return (
      <Provider
        value={{
          fetch: this.appFetch()
        }}>
        {children}
      </Provider>
    )
  }
}

export const FetchConsumer = Consumer
