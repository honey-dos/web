import React, { Component, ChangeEvent, RefObject, KeyboardEvent } from "react";
import PropTypes from "prop-types";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import { Task } from "../../lib/Task";

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
  onSave: (task: Task) => void;
  onCancel: () => void;
}

interface TaskFormState {
  name: string;
}

const initialState: TaskFormState = {
  name: ""
};

class TaskForm extends Component<TaskFormProps, TaskFormState> {
  static propTypes = {
    classes: PropTypes.object.isRequired,
    onSave: PropTypes.func.isRequired,
    onCancel: PropTypes.func.isRequired
  };
  nameInput: RefObject<HTMLInputElement>;

  constructor(props: TaskFormProps) {
    super(props);
    this.state = initialState;
    this.nameInput = React.createRef();
  }

  componentDidMount() {
    if (this.nameInput && this.nameInput.current) {
      this.nameInput.current.focus();
    }
  }

  handleChange = () => (event: ChangeEvent<HTMLInputElement>) => {
    const name = event.target.value;
    this.setState({ name });
  };

  saveOnEnter = () => (event: KeyboardEvent<HTMLDivElement>) => {
    if (event.keyCode === 13) {
      this.handleSave();
    }
  };

  handleSave = () => {
    const { name } = this.state;
    const task: Task = {
      id: "z",
      name,
      checked: false
    };
    this.props.onSave(task);
  };

  render() {
    const { classes, onCancel } = this.props;
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