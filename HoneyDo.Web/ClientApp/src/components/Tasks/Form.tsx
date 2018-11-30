import React, { Component, ChangeEvent, RefObject, KeyboardEvent } from "react";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import { Task, TaskFormModel } from "../../lib/Task";

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
  task?: Task;
  classes: { [key: string]: any };
  onSave: (task: TaskFormModel) => void;
  onCancel: () => void;
}

interface TaskFormState {
  name: string;
  hasError: boolean;
}

class TaskForm extends Component<TaskFormProps, TaskFormState> {
  nameInput: RefObject<HTMLInputElement>;

  constructor(props: TaskFormProps) {
    super(props);
    const name = (props.task && props.task.name) || "";
    this.state = { name, hasError: false };
    this.nameInput = React.createRef();
  }

  componentDidMount() {
    if (this.nameInput && this.nameInput.current) {
      this.nameInput.current.focus();
    }
  }

  handleChange = () => (event: ChangeEvent<HTMLInputElement>) => {
    const name = event.target.value;
    this.setState({ name, hasError: false });
  };

  saveOnEnter = () => (event: KeyboardEvent<HTMLDivElement>) => {
    if (event.keyCode === 13) {
      this.handleSave();
    }
  };

  handleSave = () => {
    const { name } = this.state;
    if (!name || name.trim().length === 0) {
      this.setState({ hasError: true });
      return;
    }
    const newTask: TaskFormModel = {
      name
    };
    this.props.onSave(newTask);
  };

  render() {
    const { classes, onCancel } = this.props;
    const { hasError } = this.state;
    return (
      <div>
        <TextField
          id="name"
          label="Name"
          value={this.state.name}
          onChange={this.handleChange()}
          onKeyUp={this.saveOnEnter()}
          margin="normal"
          variant="outlined"
          fullWidth
          inputRef={this.nameInput}
          error={hasError}
          helperText={hasError ? "Name is required." : null}
        />
        <div className={classes.buttonContainer}>
          <Button className={classes.button} onClick={() => this.handleSave()}>
            Save
          </Button>
          <Button className={classes.button} onClick={() => onCancel()}>
            Cancel
          </Button>
        </div>
      </div>
    );
  }
}

export default withStyles(styles)(TaskForm);
