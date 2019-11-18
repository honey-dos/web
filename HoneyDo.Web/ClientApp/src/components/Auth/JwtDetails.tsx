import Button from "@material-ui/core/Button";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import { createStyles, Theme, withStyles } from "@material-ui/core/styles";
import TextField from "@material-ui/core/TextField";
import Typography from "@material-ui/core/Typography";
import React, { Component, RefObject } from "react";
import { JwtData } from "../../lib/jwt";

const styles = ({ spacing }: Theme) =>
  createStyles({
    button: {
      margin: spacing()
    }
  });

interface JwtDetailsProps {
  jwtData: JwtData;
  onLogout: () => void;
  onRefresh: () => void;
  classes: { button: any };
}

class JwtDetails extends Component<JwtDetailsProps> {
  tokenTextArea: RefObject<HTMLTextAreaElement>;
  constructor(props: JwtDetailsProps) {
    super(props);
    this.tokenTextArea = React.createRef();
  }

  copyToken = () => {
    if (this.tokenTextArea && this.tokenTextArea.current) {
      this.tokenTextArea.current.select();
      document.execCommand("copy");
    }
  };

  render() {
    const { jwtData, classes, onLogout, onRefresh } = this.props;
    const { id, token, name, expires } = jwtData;
    const isExpired = jwtData.isExpired();
    return (
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
            <ListItemText primary="Expires" secondary={expires.toISOString()} />
          </ListItem>
          <ListItem>
            <ListItemText
              primary="Expired"
              secondary={!isExpired ? "No" : "Yes"}
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
          margin="normal"
          variant="outlined"
          inputProps={{ readOnly: true }}
          inputRef={this.tokenTextArea}
        />
        <br />
        <Button className={classes.button} onClick={() => this.copyToken()}>
          Copy Token
        </Button>
        <Button className={classes.button} onClick={onRefresh}>
          Refresh
        </Button>
        <Button
          className={classes.button}
          style={{ marginLeft: 10 }}
          onClick={onLogout}>
          Logout
        </Button>
      </div>
    );
  }
}

export default withStyles(styles)(JwtDetails);
