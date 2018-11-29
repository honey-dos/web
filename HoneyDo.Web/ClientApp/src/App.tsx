import React from "react";
import Layout from "./components/Layout/Layout";
import { UserProvider } from "./providers/UserProvider";
import { createMuiTheme, MuiThemeProvider } from "@material-ui/core";
import Router from "./Router";

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
        <Router />
      </Layout>
    </UserProvider>
  </MuiThemeProvider>
);
