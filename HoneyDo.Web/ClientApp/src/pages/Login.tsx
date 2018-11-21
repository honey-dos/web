import React, { Component } from "react";
import PropTypes from "prop-types";
import firebase from "firebase/app";
import "firebase/auth";
import { getTokenData, setToken, JwtData, logout } from "../lib/jwt";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Typography from "@material-ui/core/Typography";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import Divider from "@material-ui/core/Divider";

const styles = ({ spacing }: Theme) =>
  createStyles({
    button: {
      margin: spacing.unit
    },
    textField: {
      marginLeft: spacing.unit,
      marginRight: spacing.unit
    },
    divider: {
      marginBottom: 20
    }
  });

interface LoginProps {
  classes: { [key: string]: any };
}

interface LoginState {
  user?: JwtData;
  isLoading: boolean;
}

const initialState: LoginState = { isLoading: false };

// Component<{props class or interface}, {state class or interface}, {I don't know what this is yet}>
class Login extends Component<LoginProps, LoginState> {
  static propTypes = {
    classes: PropTypes.object.isRequired
  };

  constructor(props: LoginProps) {
    super(props);
    this.state = initialState;
  }

  componentDidMount() {
    this.setUserData();
  }

  setUserData() {
    const user = getTokenData();
    if (user) {
      this.setState({ user });
    } else {
      this.setState({ user: undefined });
    }
  }

  async onLoginClick(mode: string) {
    this.setState({ isLoading: true });
    const provider = new firebase.auth.GoogleAuthProvider();
    provider.addScope("https://www.googleapis.com/auth/contacts.readonly");
    const firebaseAuth = firebase && firebase.auth();
    if (!firebase || !firebaseAuth.currentUser) {
      return;
    }
    firebaseAuth.useDeviceLanguage();
    try {
      await firebaseAuth.signInWithPopup(provider);
      const idToken = await firebaseAuth.currentUser.getIdToken(false);
      const url = `api/token/${mode}`;
      const tokenRequest = await fetch(url, {
        method: "GET",
        mode: "cors",
        cache: "no-cache",
        headers: {
          "Id-Token": idToken
        }
      });
      const result = await tokenRequest.json();
      setToken(result.token);
      this.setUserData();
      this.setState({ isLoading: false });
    } catch (error) {
      // const { errorCode, errorMessage, email, credential } = error;
      // TODO: what do we do?
      console.error(error);
    }
  }

  logout() {
    logout();
    this.setUserData();
  }

  render() {
    const { classes } = this.props;
    const { user, isLoading } = this.state;
    let view = null;

    if (user) {
      const { id, token, name, expires } = user;
      const isExpired = user.isExpired();
      view = (
        <div>
          <Typography variant="h5">Token Data</Typography>
          <List>
            <ListItem>
              <ListItemText primary="Id" secondary={id} />
            </ListItem>
            <ListItem>
              <ListItemText primary="Name" secondary={name} />
            </ListItem>
            <ListItem>
              <ListItemText
                primary="Expires"
                secondary={expires.toISOString()}
              />
            </ListItem>
            <ListItem>
              <ListItemText
                primary="Expired"
                secondary={expires > new Date() ? "No" : "Yes"}
              />
            </ListItem>
          </List>
          <TextField
            id="token"
            label="Token"
            multiline
            rows={4}
            fullWidth
            value={token}
            className={classes.textField}
            margin="normal"
            variant="outlined"
            inputProps={{ readOnly: true }}
          />
          <br />
          <Button
            className={classes.button}
            onClick={() => this.onLoginClick("login")}>
            Refresh
          </Button>
          <Button
            className={classes.button}
            style={{ marginLeft: 10 }}
            onClick={() => this.logout()}>
            Logout
          </Button>
        </div>
      );
    } else {
      view = (
        <Button
          className={classes.button}
          onClick={() => this.onLoginClick("register")}
          disabled={isLoading}>
          Google
        </Button>
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
