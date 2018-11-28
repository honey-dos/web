import React, { Component } from "react";
import PropTypes from "prop-types";
import TasksList from "../components/Tasks/List";
import TaskForm from "../components/Tasks/Form";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import { Task } from "../lib/Task";

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

interface TasksProps {
  classes: { [key: string]: any };
}

interface TasksState {
  isFormOpen: boolean;
}

const initialState: TasksState = {
  isFormOpen: false
};

class Tasks extends Component<TasksProps, TasksState> {
  static propTypes = {
    classes: PropTypes.object.isRequired
  };

  constructor(props: TasksProps) {
    super(props);
    this.state = initialState;
  }

  handleSave = (task: Task) => {
    console.log(task);
    this.toggleForm();
  };

  toggleForm = () => {
    const isFormOpen = !this.state.isFormOpen;
    this.setState({ isFormOpen });
  };

  render() {
    const { classes } = this.props;
    const { isFormOpen } = this.state;
    return (
      <div>
        <TasksList />
        {!isFormOpen ? (
          <div className={classes.buttonContainer}>
            <Button
              className={classes.button}
              onClick={() => this.toggleForm()}>
              Add Task
            </Button>
          </div>
        ) : (
          <TaskForm
            onCancel={() => this.toggleForm()}
            onSave={task => this.handleSave(task)}
          />
        )}
      </div>
    );
  }
}

export default withStyles(styles)(Tasks);
