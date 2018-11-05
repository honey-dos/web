import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/css/bootstrap-theme.css";
import "./index.css";
import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import firebase from "firebase/app";
import App from "./App";
import registerServiceWorker from "./registerServiceWorker";

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
const rootElement = document.getElementById("root");

const config = {
  apiKey: "AIzaSyA8IvTRwITWjIeSvc0iW5Yl63Q5j6Wyuk0",
  authDomain: "honey-dos.firebaseapp.com",
  databaseURL: "https://honey-dos.firebaseio.com",
  storageBucket: "honey-dos.appspot.com"
};
firebase.initializeApp(config);

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement
);

registerServiceWorker();
