import React from "react";
import Layout from "./components/Layout/Layout";
import { UserProvider } from "./contexts/UserContext";
import { FetchProvider } from "./contexts/FetchContext";
import { createMuiTheme, MuiThemeProvider } from "@material-ui/core";
import Router from "./Router";
import { TaskProvider } from "./contexts/TaskContext";
import { MuiPickersUtilsProvider } from "@material-ui/pickers";
import DateFnsUtils from "@date-io/date-fns";

// when we start to create a bigger more customized theme we should move this configuration to it's own file theme.tsx for example
const theme = createMuiTheme({});

export default () => (
  <MuiPickersUtilsProvider utils={DateFnsUtils}>
    <MuiThemeProvider theme={theme}>
      <UserProvider>
        <FetchProvider>
          <TaskProvider>
            <Layout>
              <Router />
            </Layout>
          </TaskProvider>
        </FetchProvider>
      </UserProvider>
    </MuiThemeProvider>
  </MuiPickersUtilsProvider>
);
