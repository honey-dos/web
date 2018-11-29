import React, { Component } from "react";
import { Switch, Route, Redirect } from "react-router";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Tasks from "./pages/Tasks";
import { UserConsumer, UserContextData } from "./contexts/UserContext";

function PrivateRoute({
  component,
  ...rest
}: {
  component: any;
  [key: string]: any;
}) {
  const Component = component;
  return (
    <UserConsumer>
      {(userContext: UserContextData) => (
        <Route
          {...rest}
          render={props =>
            userContext.isLoggedIn() ? (
              <Component {...props} />
            ) : (
              <Redirect
                to={{
                  pathname: "/login",
                  state: { from: props.location }
                }}
              />
            )
          }
        />
      )}
    </UserConsumer>
  );
}

export default () => (
  <UserConsumer>
    {(userContext: UserContextData) =>
      userContext.isInitialized ? (
        <Switch>
          <Route exact path="/" component={Home} />
          <PrivateRoute path="/tasks" component={Tasks} />
          <Route path="/login" component={Login} />
        </Switch>
      ) : (
        <div>Loading</div>
      )
    }
  </UserConsumer>
);
