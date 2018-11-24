import React from "react";
import { Switch, Route } from "react-router";
import Layout from "./components/Layout/Layout";
import Home from "./pages/Home";
import Login from "./pages/Login";
import { UserProvider } from "./providers/UserProvider";
import { createMuiTheme, MuiThemeProvider } from "@material-ui/core";

// when we start to create a bigger more customized theme we should move this configuration to it's own file theme.tsx for example
const theme = createMuiTheme({
  typography: {
    useNextVariants: true
  }
});

export default () => (
  <MuiThemeProvider theme={theme}>
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
  </MuiThemeProvider>
);
