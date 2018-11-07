import React, { Component } from "react";
import firebase from "firebase/app";
import "firebase/auth";

export class Login extends Component {
  state = {
    token: ""
  };

  constructor() {
    super();

    this.onLoginClick = this.onLoginClick.bind(this);
  }

  onLoginClick = mode => {
    const provider = new firebase.auth.GoogleAuthProvider();
    provider.addScope("https://www.googleapis.com/auth/contacts.readonly");
    firebase.auth().useDeviceLanguage();
    firebase
      .auth()
      .signInWithPopup(provider)
      .then(() => firebase.auth().currentUser.getIdToken(false))
      .then(idToken => {
        const url = `api/token/${mode}`;
        fetch(url, {
          method: "GET",
          mode: "cors",
          cache: "no-cache",
          headers: {
            "Id-Token": idToken
          }
        })
          .then(r => r.json())
          .then(json => {
            this.setState({ token: json.token });
          });
      })
      .catch(error => {
        // const { errorCode, errorMessage, email, credential } = error;
        // TODO: what do we do?
        console.error(error);
      });
  };

  render() {
    const { token } = this.state;
    return (
      <div>
        Login page
        <button onClick={() => this.onLoginClick("register")} type="button">
          Register
        </button>
        <button onClick={() => this.onLoginClick("login")} type="button">
          Login
        </button>
        <div>Token: {token}</div>
      </div>
    );
  }
}
