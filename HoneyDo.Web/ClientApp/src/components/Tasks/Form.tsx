import React, { Component, ChangeEvent } from "react";
import PropTypes from "prop-types";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";

const styles = ({ spacing }: Theme) =>
  createStyles({
    button: {
      margin: spacing.unit,
      float: "right"
    },
    buttonContainer: {
      display: "flex",
      flexDirection: "row-reverse"
    }
  });

interface TaskFormProps {
  classes: { [key: string]: any };
}

interface TaskFormState {
  name: string;
}

const initialState: TaskFormState = {
  name: ""
};

class TaskForm extends Component<TaskFormProps, TaskFormState> {
  static propTypes = {
    classes: PropTypes.object.isRequired
  };

  constructor(props: TaskFormProps) {
    super(props);
    this.state = initialState;
  }

  handleChange = () => (event: ChangeEvent<HTMLInputElement>) => {
    const name = event.target.value;
    this.setState({ name });
  };

  render() {
    const { classes } = this.props;
    return (
      <div>
        <TextField
          id="name"
          label="Name"
          value={this.state.name}
          onChange={this.handleChange()}
          margin="normal"
          variant="outlined"
          fullWidth
        />
        <div className={classes.buttonContainer}>
          <Button
            className={classes.button}
            onClick={() => console.log("save task")}>
            Save
          </Button>
          <Button
            className={classes.button}
            onClick={() => console.log("cancel task")}>
            Cancel
          </Button>
        </div>
      </div>
    );
  }
}

export default withStyles(styles)(TaskForm);
