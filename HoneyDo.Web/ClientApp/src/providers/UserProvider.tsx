import React, { Component } from "react";
import firebase from "firebase/app";
import "firebase/auth";
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

  componentDidMount() {
    this.setJwtData();
  }

  updateToken(token: string): void {
    setToken(token);
    this.setJwtData();
  }

  setJwtData() {
    const jwtData = getTokenData() || undefined;
    if (jwtData) {
      this.setState({ jwtData });
    }
  }

  logout(): void {
    logout();
    const firebaseAuth = firebase && firebase.auth();
    firebaseAuth.signOut();
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
