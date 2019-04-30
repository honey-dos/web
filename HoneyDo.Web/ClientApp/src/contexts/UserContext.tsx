// TODO refactor this context to be more geared towards Auth (AuthContext)
import React, { Component } from "react";
import firebase from "firebase/app";
import "firebase/auth";
import { JwtData, setToken, getTokenData, logout } from "../lib/jwt";

export interface IUpdateToken {
  (token: string): void;
}

export interface ILogout {
  (): void;
}

export interface IIsLoggedIn {
  (): boolean;
}

export interface UserContextData {
  jwtData?: JwtData;
  updateToken: IUpdateToken;
  logout: ILogout;
  isLoggedIn: IIsLoggedIn;
  isInitialized: boolean;
}

interface UserProviderState {
  jwtData?: JwtData;
  isInitialized: boolean;
}

export const UserContext = React.createContext<UserContextData>({
  updateToken: () => {},
  logout: () => {},
  isLoggedIn: () => false,
  isInitialized: false
});

const { Provider, Consumer } = UserContext;

export class UserProvider extends Component<{}, UserProviderState> {
  constructor(props: {}) {
    super(props);
    this.state = { isInitialized: false };
  }

  componentDidMount() {
    this.setJwtData();
  }

  updateToken: IUpdateToken = (token: string): void => {
    setToken(token);
    this.setJwtData();
  };

  setJwtData() {
    const jwtData = getTokenData() || undefined;
    if (jwtData) {
      this.setState({ jwtData });
    }
    this.setState({ isInitialized: true });
  }

  logout: ILogout = (): void => {
    logout();
    const firebaseAuth = firebase && firebase.auth();
    firebaseAuth.signOut();
    this.setState({ jwtData: undefined });
  };

  isLoggedIn: IIsLoggedIn = (): boolean => {
    if (this.state.jwtData) {
      return !this.state.jwtData.isExpired();
    }
    return false;
  };

  render() {
    const { children } = this.props;
    const { isInitialized, jwtData } = this.state;

    return (
      <Provider
        value={{
          jwtData: jwtData,
          updateToken: (token: string) => this.updateToken(token),
          logout: () => this.logout(),
          isLoggedIn: () => this.isLoggedIn(),
          isInitialized: isInitialized
        }}>
        {children}
      </Provider>
    );
  }
}

export const UserConsumer = Consumer;
