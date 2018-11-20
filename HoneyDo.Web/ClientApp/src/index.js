import React from "react";
import ReactDOM from "react-dom";
import { Route, Switch } from "react-router";
import { BrowserRouter } from "react-router-dom";
import firebase from "firebase";
import App from "./App";
import Login from "./pages/Login.tsx";
// import User from "./pages/User";
// import Todos from "./pages/Todos";
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
    <Switch>
      <Route exact path="/" component={App} />
      {/* <Route path="/todo" component={Todos} />
      <Route path="/user" compoennt={User} /> */}
      <Route path="/login" component={Login} />
    </Switch>
  </BrowserRouter>,
  rootElement
);

registerServiceWorker();
