import "typeface-roboto";
import "./index.css";
import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import firebase from "firebase/app";
import registerServiceWorker from "./registerServiceWorker";
import App from "./App";

const baseUrl =
  document.getElementsByTagName("base")[0].getAttribute("href") || undefined;
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
