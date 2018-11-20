import React, { Component } from "react";
import Button from "../components/Button/index";
import firebase from "firebase/app";
import "firebase/auth";
import { getTokenData, setToken, JwtData } from "../lib/jwt";

interface LoginState {
  user?: JwtData;
}

const initialState: LoginState = {};

// Component<{props class or interface}, {state class or interface}, {I don't know what this is yet}>
class Login extends Component<{}, LoginState> {
  constructor(props: {}) {
    super(props);
    this.state = initialState;
  }

  componentDidMount() {
    this.setUserData();
  }

  setUserData() {
    const user = getTokenData();
    if (user) {
      this.setState({ user });
    }
  }

  async onLoginClick(mode: string) {
    const provider = new firebase.auth.GoogleAuthProvider();
    provider.addScope("https://www.googleapis.com/auth/contacts.readonly");
    const firebaseAuth = firebase && firebase.auth();
    if (!firebase || !firebaseAuth.currentUser) {
      return;
    }
    firebaseAuth.useDeviceLanguage();
    try {
      await firebaseAuth.signInWithPopup(provider);
      const idToken = await firebaseAuth.currentUser.getIdToken(false);
      const url = `api/token/${mode}`;
      const tokenRequest = await fetch(url, {
        method: "GET",
        mode: "cors",
        cache: "no-cache",
        headers: {
          "Id-Token": idToken
        }
      });
      const result = await tokenRequest.json();
      setToken(result.token);
      this.setUserData();
    } catch (error) {
      // const { errorCode, errorMessage, email, credential } = error;
      // TODO: what do we do?
      console.error(error);
    }
  }

  render() {
    const { user } = this.state;
    let view = null;
    if (user) {
      const { id, token, name, expires } = user;
      const isExpired = user.isExpired();
      view = (
        <div>
          <h3>Token Data</h3>
          <p>Id: {id}</p>
          <p>Name: {name}</p>
          <p>
            Expires: {expires ? expires.toISOString() : ""}{" "}
            {isExpired ? "(expired)" : "(valid)"}
          </p>
          <textarea rows={6} cols={50} readOnly={true} value={token} />
          <br />
          <Button onClick={() => this.onLoginClick("login")}>Refresh</Button>
        </div>
      );
    } else {
      view = (
        <div>
          <Button onClick={() => this.onLoginClick("register")}>
            Register
          </Button>
          <br />
          <br />
          <Button onClick={() => this.onLoginClick("login")}>Login</Button>
        </div>
      );
    }
    return (
      <div>
        <h1>Login page</h1>
        <hr />
        <br />
        {view}
      </div>
    );
  }
}

export default Login;
