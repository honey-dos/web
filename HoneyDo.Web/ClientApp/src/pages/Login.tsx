import { createStyles, Theme, withStyles } from "@material-ui/core";
import Button from "@material-ui/core/Button";
import Divider from "@material-ui/core/Divider";
import Typography from "@material-ui/core/Typography";
import "firebase/auth";
import React, { Component } from "react";
import JwtDetails from "../components/Auth/JwtDetails";
import RegisterForm from "../components/Auth/RegisterForm";
import { UserContext, UserContextData } from "../contexts/UserContext";
import { login, registerAccount } from "../lib/auth";

const styles = ({ spacing }: Theme) =>
  createStyles({
    button: {
      margin: spacing.unit
    },
    divider: {
      marginBottom: 20
    }
  });

interface LoginProps {
  classes: { [key: string]: any };
}

interface LoginState {
  state: LoginStates;
}

enum LoginStates {
  Processing = "Processing",
  Registering = "Registering",
  WaitingInput = "WaitingInput"
}

const initialState: LoginState = {
  state: LoginStates.WaitingInput
};

// Component<{props class or interface}, {state class or interface}, {I don't know what this is yet}>
class Login extends Component<LoginProps, LoginState> {
  static contextType = UserContext;

  constructor(props: LoginProps) {
    super(props);
    this.state = initialState;
  }

  startRegistration = () => {
    this.setState({ state: LoginStates.Registering });
  };

  cancel = () => {
    this.setState({ state: LoginStates.WaitingInput });
  };

  register = async (name: string, userName: string) => {
    this.setState({ state: LoginStates.Processing });
    const registerResult = await registerAccount(name, userName);
    // TODO do something with account
    const { token, errors } = registerResult;
    if (errors.length) {
      // TODO show errors
      console.log(errors);
      this.setState({ state: LoginStates.WaitingInput });
      return;
    }
    const { updateToken }: UserContextData = this.context;
    updateToken(token);
    this.setState({ state: LoginStates.WaitingInput });
  };

  login = async () => {
    this.setState({ state: LoginStates.Processing });
    const loginResult = await login();
    const { token, errors } = loginResult;
    if (errors.length) {
      // TODO show errors
      console.log(errors);
      this.setState({ state: LoginStates.WaitingInput });
      return;
    }
    const { updateToken }: UserContextData = this.context;
    updateToken(token);
    this.setState({ state: LoginStates.WaitingInput });
  };

  logout = () => {
    const { logout }: UserContextData = this.context;
    logout();
  };

  render() {
    const { classes } = this.props;
    const { state } = this.state;
    const { jwtData }: UserContextData = this.context;
    let view = null;

    if (jwtData) {
      view = (
        <JwtDetails
          jwtData={jwtData}
          onLogout={this.logout}
          onRefresh={this.login}
        />
      );
    } else if (state === LoginStates.Registering) {
      view = <RegisterForm onSubmit={this.register} onCancel={this.cancel} />;
    } else if (state === LoginStates.Processing) {
      view = <div>Please Authenticate using the pop-up.</div>;
    } else {
      view = (
        <div>
          <Button
            className={classes.button}
            onClick={() => this.startRegistration()}>
            Sign-up with Google
          </Button>
          <Button className={classes.button} onClick={() => this.login()}>
            Login with Google
          </Button>
        </div>
      );
    }
    return (
      <div>
        <Typography variant="h4">Log In</Typography>
        <Divider className={classes.divider} />
        {view}
      </div>
    );
  }
}

export default withStyles(styles)(Login);
