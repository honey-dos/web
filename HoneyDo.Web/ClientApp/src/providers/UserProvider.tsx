import React, { Component } from "react";
import { JwtData, setToken, getTokenData, logout } from "../lib/jwt";

export interface UserContextData {
  jwtData?: JwtData;
  updateToken: (token: string) => void;
  logout: () => void;
}

interface UserProviderState {
  jwtData?: JwtData;
}

export const UserContext = React.createContext<UserContextData>({
  updateToken: () => {},
  logout: () => {}
});

const { Provider, Consumer } = UserContext;

export class UserProvider extends Component<{}, UserProviderState> {
  constructor(props: {}) {
    super(props);
    this.state = {};
  }

  updateToken(token: string): void {
    setToken(token);
    const jwtData = getTokenData() || undefined;
    this.setState({ jwtData });
  }

  logout(): void {
    logout();
    this.setState({ jwtData: undefined });
  }

  render() {
    const { children } = this.props;

    return (
      <Provider
        value={{
          jwtData: this.state.jwtData,
          updateToken: (token: string) => this.updateToken(token),
          logout: () => this.logout()
        }}>
        {children}
      </Provider>
    );
  }
}

export const UserConsumer = Consumer;
