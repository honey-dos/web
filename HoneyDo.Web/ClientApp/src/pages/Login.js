import React, { Component } from "react";
import Button from "../components/Button";
import firebase from "firebase/app";
import "firebase/auth";
import { getUserData, setToken } from "../lib/jwt";

class Login extends Component {
  state = {
    user: { isValid: false }
  };

  constructor() {
    super();
    this.onLoginClick = this.onLoginClick.bind(this);
  }

  componentDidMount() {
    this.setUserData();
  }

  setUserData() {
    const user = getUserData();
    if (user && user.isValid) {
      this.setState({ user });
    }
  }

  async onLoginClick(mode) {
    const provider = new firebase.auth.GoogleAuthProvider();
    provider.addScope("https://www.googleapis.com/auth/contacts.readonly");
    firebase.auth().useDeviceLanguage();
    try {
      await firebase.auth().signInWithPopup(provider);
      const idToken = await firebase.auth().currentUser.getIdToken(false);
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
      this.setState({ token: result.token });
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
    if (user.isValid) {
      const { id, token, name } = user;
      view = (
        <div>
          <h3>Token Data</h3>
          <p>Id: {id}</p>
          <p>Name: {name}</p>
          <textarea rows="5" cols="50" readOnly={true} value={token} />
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
