import React, { Component } from "react";
import PropTypes from "prop-types";
import TasksList from "../components/Tasks/List";
import TaskForm from "../components/Tasks/Form";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";

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

class Tasks extends Component<TasksProps, {}> {
  static propTypes = {
    classes: PropTypes.object.isRequired
  };
  render() {
    const { classes } = this.props;
    return (
      <div>
        <TasksList />
        <TaskForm />
        <div className={classes.buttonContainer}>
          <Button
            className={classes.button}
            onClick={() => console.log("add task")}>
            Add Task
          </Button>
        </div>
      </div>
    );
  }
}

export default withStyles(styles)(Tasks);
