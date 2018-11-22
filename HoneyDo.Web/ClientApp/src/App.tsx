import React from "react";
import { Switch, Route } from "react-router";
import Layout from "./components/Layout/Layout";
import Home from "./pages/Home";
import Login from "./pages/Login";
import { UserProvider } from "./providers/UserProvider";
// import User from "./pages/User";
// import Todos from "./pages/Todos";

export default () => (
  <UserProvider>
    <Layout>
      <Switch>
        <Route exact path="/" component={Home} />
        {/* <Route path="/todo" component={Todos} />
      <Route path="/user" compoennt={User} /> */}
        <Route path="/login" component={Login} />
      </Switch>
    </Layout>
  </UserProvider>
);
