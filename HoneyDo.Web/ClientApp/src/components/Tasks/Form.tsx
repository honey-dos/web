import React, { Component, ChangeEvent, RefObject, KeyboardEvent } from "react";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import { Task, TaskFormModel } from "../../lib/Task";
import { DatetimePicker } from "../core/DatetimePicker";
import { withWidth } from "@material-ui/core";

const styles = ({ spacing, breakpoints }: Theme) =>
  createStyles({
    button: {
      margin: spacing.unit,
      float: "right"
    },
    inputContainer: {
      display: "flex",
      [breakpoints.up("md")]: {
        flexWrap: "nowrap"
      },
      [breakpoints.down("sm")]: {
        flexWrap: "wrap"
      }
    },
    input: {
      margin: spacing.unit
    },
    dateInput: {
      flexBasis: 260
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
  onDelete?: () => void;
  width: string;
}

interface TaskFormState {
  name: string;
  hasError: boolean;
  dueDate?: Date;
}

class TaskForm extends Component<TaskFormProps, TaskFormState> {
  nameInput: RefObject<HTMLInputElement>;

  constructor(props: TaskFormProps) {
    super(props);
    const name = (props.task && props.task.name) || "";
    const dueDate = (props.task && props.task.dueDate) || undefined;
    this.state = { name, hasError: false, dueDate };
    this.nameInput = React.createRef();
  }

  componentDidMount() {
    if (this.nameInput && this.nameInput.current) {
      this.nameInput.current.focus();
    }
  }

  handleNameChange = () => (event: ChangeEvent<HTMLInputElement>) => {
    const name = event.target.value;
    this.setState({ name, hasError: false });
  };

  handleDateChange = (dates: Date[]) => {
    const dueDate = dates.length > 0 ? dates[0] : undefined;
    this.setState({ dueDate });
  };

  saveOnEnter = () => (event: KeyboardEvent<HTMLDivElement>) => {
    if (event.keyCode === 13) {
      this.handleSave();
    }
  };

  handleSave = () => {
    const { name, dueDate } = this.state;
    if (!name || name.trim().length === 0) {
      this.setState({ hasError: true });
      return;
    }
    const newTask: TaskFormModel = {
      name,
      dueDate
    };
    this.props.onSave(newTask);
  };

  render() {
    const { classes, task, onCancel, onDelete, width } = this.props;
    const { hasError, dueDate } = this.state;
    return (
      <div>
        <div className={classes.inputContainer}>
          <TextField
            id="name"
            label="Name"
            value={this.state.name}
            onChange={this.handleNameChange()}
            onKeyUp={this.saveOnEnter()}
            margin="normal"
            variant="outlined"
            fullWidth
            inputRef={this.nameInput}
            error={hasError}
            helperText={hasError ? "Name is required." : null}
            className={classes.input}
          />
          <DatetimePicker
            label={"Due Date"}
            date={dueDate}
            fullWidth={width === "xs" || width === "sm"}
            onChange={(dates: Date[]) => this.handleDateChange(dates)}
            className={[classes.input, classes.dateInput].join(" ")}
          />
        </div>
        <div className={classes.buttonContainer}>
          <Button className={classes.button} onClick={() => this.handleSave()}>
            Save
          </Button>
          <Button className={classes.button} onClick={() => onCancel()}>
            Cancel
          </Button>
          {task ? (
            <Button
              className={classes.button}
              onClick={() => (onDelete ? onDelete() : null)}>
              Delete
            </Button>
          ) : null}
        </div>
      </div>
    );
  }
}

export default withStyles(styles)(withWidth()(TaskForm));
